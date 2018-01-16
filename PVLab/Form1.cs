using LiveCharts;
using PicoPinnedArray;
using PicoStatus;
using PS5000AImports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using LiveCharts.Wpf;
using LiveCharts.WinForms;


namespace PVLab
{
    public partial class PVLab : Form
    {


        int[] sampleTime;
        private short _handle;
        public const int BUFFER_SIZE = 1024;
        public const int MAX_CHANNELS = 4;
        public const int QUAD_SCOPE = 4;
        public const int DUAL_SCOPE = 2;
        public short _overflow = 0;
        short[][] appBuffers;
        short[][] buffers;
        int sampleIntervall;

        uint _timebase;
        short _oversample = 1;
        bool _scaleVoltages = true;

        ushort[] inputRanges = { 10, 20, 50, 100, 200, 500, 1000, 2000, 5000, 10000, 20000, 50000 };
        bool _ready = false;
        short _trig = 0;
        uint _trigAt = 0;
        int _sampleCount = 0;
        uint _startIndex = 0;
        bool _autoStop;
        private ChannelSettings[] _channelSettings;
        private int _channelCount;
        private Imports.Range _firstRange;
        private Imports.Range _lastRange;
        private int _digitalPorts;
        private Imports.ps5000aBlockReady _callbackDelegate;
        private string StreamFile = "stream.txt";
        private string BlockFile = "block.txt";
        Imports.DeviceResolution resolution;
        int[] samples;
        int[] time;
        short _maxValue;
        int[] minValue;
        int[] maxValue;
        public static string s;

        struct ChannelSettings
        {
            public Imports.Range range;
            public bool enabled;
        }

        public PVLab()
        {
            InitializeComponent();

            // Initiate GUI
            InitiateGUI();
        }



        #region Initiating the combox box and other things

        /// <summary>
        /// Here we are initiating the combox box with the Enums available in the mapp
        /// </summary>
        private void InitiateGUI()
        {
            // Set up  combo boxes
            cbVoltage.DataSource = Enum.GetValues(typeof(Imports.Range));
            cbRes.DataSource = Enum.GetValues(typeof(Imports.DeviceResolution));
            cbChannels.DataSource = Enum.GetValues(typeof(Imports.Channel));
            cbCoupling.DataSource = Enum.GetValues(typeof(Imports.Coupling));
            cbStreaming.DataSource = Enum.GetValues(typeof(Imports.streaminType));
        }
        #endregion


        #region UI events
        private void btnStreaming_Click(object sender, EventArgs e)
        {
            // Stream and plot
            Streaming();
        }
        private void btnOpen_Click(object sender, EventArgs e)
        {
            StringBuilder UnitInfo = new StringBuilder(80);

            short handle;

            string[] description = {
                           "Driver Version    ",
                           "USB Version       ",
                           "Hardware Version  ",
                           "Variant Info      ",
                           "Serial            ",
                           "Cal Date          ",
                           "Kernel Ver        ",
                           "Digital Hardware  ",
                           "Analogue Hardware "
                         };

            Imports.DeviceResolution resolution = (Imports.DeviceResolution)cbRes.SelectedIndex;
            //Imports.DeviceResolution resolution = Imports.DeviceResolution.PS5000A_DR_8BIT;


            if (_handle > 0)
            {
                Imports.CloseUnit(_handle);
                txtStatus.Text = "";
                _handle = 0;
                btnOpen.Text = "Open";
            }
            else
            {
                uint status = Imports.OpenUnit(out handle, null, resolution);

                if (handle > 0)
                {
                    _handle = handle;

                    if (status == StatusCodes.PICO_POWER_SUPPLY_NOT_CONNECTED)
                    {
                        status = Imports.ChangePowerSource(_handle, status);
                    }
                    else if (status != StatusCodes.PICO_OK)
                    {
                        MessageBox.Show("Cannot open device error code: " + status.ToString(), "Error Opening Device", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                    }
                    else
                    {
                        // Do nothing - power supply connected
                    }

                    txtStatus.Text += "Handle            " + _handle.ToString() + "\r\n" + Environment.NewLine;

                    for (int i = 0; i < 9; i++)
                    {
                        short requiredSize;
                        Imports.GetUnitInfo(_handle, UnitInfo, 80, out requiredSize, (uint)i);
                        txtStatus.Text += (description[i] + UnitInfo + "\r\n") + Environment.NewLine;
                    }
                    btnOpen.Text = "Close";
                }
            }
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            RunBlock();

        }
        #endregion

        #region BlockCallBack for capturing data
        private void BlockCallback(short handle, short status, IntPtr pVoid)
        {
            // flag to say done reading data
            if (status != (short)StatusCodes.PICO_CANCELLED)
                _ready = true;
        }
        #endregion

        #region RunBlock Function to capture measurement
        /// <summary>
        ///  we use this method to do a runblock
        /// </summary>
        private void RunBlock()
        {

            Imports.MaximumValue(_handle, out _maxValue);

            // Setting channel. Is needed for everytime you want to do something. eg when you change the voltage range, resulotion.
            uint status;
            status = Imports.SetChannel(_handle, (Imports.Channel)cbChannels.SelectedIndex, 1, (Imports.Coupling)cbCoupling.SelectedIndex, (Imports.Range)cbVoltage.SelectedIndex, 0);


            // SetsimplerTrigger function
            short enable = 0;
            uint delay = 0;
            short threshold = 25000;
            short auto = 0;
            status = Imports.SetSimpleTrigger(_handle, enable, (Imports.Channel)cbChannels.SelectedIndex, threshold, Imports.ThresholdDirection.Rising, delay, auto);

            _ready = false;
            _callbackDelegate = BlockCallback;
            _channelCount = 1;

            bool retry;
            uint sampleCount = 1000;
            PinnedArray<short>[] minPinned = new PinnedArray<short>[_channelCount];
            PinnedArray<short>[] maxPinned = new PinnedArray<short>[_channelCount];

            int timeIndisposed;
            short[] minBuffers = new short[sampleCount];

            short[] maxBuffers = new short[sampleCount];
            minPinned[0] = new PinnedArray<short>(minBuffers);
            maxPinned[0] = new PinnedArray<short>(maxBuffers);
            status = Imports.SetDataBuffers(_handle, (Imports.Channel)cbChannels.SelectedIndex, maxBuffers, minBuffers, (int)sampleCount, 0, Imports.RatioMode.None);

            txtStatus.Text += ("BlockData\n") + Environment.NewLine;

            /*  find the maximum number of samples, the time interval (in timeUnits),
             *		 the most suitable time units, and the maximum _oversample at the current _timebase*/
            int timeInterval;
            int maxSamples;
            while (Imports.GetTimebase(_handle, _timebase, (int)sampleCount, out timeInterval, out maxSamples, 0) != 0)
            {
                txtStatus.Text += ("Timebase selection\n") + Environment.NewLine;
                _timebase++;

            }
            txtStatus.Text += ("Timebase Set\n") + Environment.NewLine;

            /* Start it collecting, then wait for completion*/
            _ready = false;
            _callbackDelegate = BlockCallback;

            sampleIntervall = int.Parse(txtSamplingInterval.Text);
            _timebase = (uint)(125000000 * sampleIntervall * Math.Pow(10, -9)) + 2;

            do
            {
                retry = false;
                status = Imports.RunBlock(_handle, 0, (int)sampleCount, _timebase, out timeIndisposed, 0, _callbackDelegate, IntPtr.Zero);

                if (status == (short)StatusCodes.PICO_POWER_SUPPLY_CONNECTED || status == (short)StatusCodes.PICO_POWER_SUPPLY_NOT_CONNECTED || status == (short)StatusCodes.PICO_POWER_SUPPLY_UNDERVOLTAGE)
                {
                    status = Imports.ChangePowerSource(_handle, status);
                    retry = true;
                }
                else
                {
                    txtStatus.Text += ("Run Block Called\n") + Environment.NewLine;
                }
            }
            while (retry);

            txtStatus.Text += ("Waiting for Data\n") + Environment.NewLine;

            while (!_ready)
            {
                Thread.Sleep(100);
            }

            Imports.Stop(_handle);

            if (_ready)
            {
                short overflow;
                status = Imports.GetValues(_handle, 0, ref sampleCount, 1, Imports.DownSamplingMode.None, 0, out overflow);
                samples = new int[sampleCount];
                sampleTime = new int[sampleCount];
                if (status == (short)StatusCodes.PICO_OK)
                {
                    string data;
                    int x;

                    txtStatus.Text += ("Have Data\n") + Environment.NewLine;

                    for (x = 0; x < sampleCount; x++)
                    {
                        data = maxBuffers[x].ToString();
                        samples[x] = int.Parse(data);

                    }
                    DrawingChart();

                }
                else
                {
                    txtStatus.Text += ("No Data\n") + Environment.NewLine;

                }
            }
            else
            {
                txtStatus.Text += ("data collection aborted\n") + Environment.NewLine;
            }

            Imports.Stop(_handle);

            foreach (PinnedArray<short> p in minPinned)
            {
                if (p != null)
                    p.Dispose();
            }
            foreach (PinnedArray<short> p in maxPinned)
            {
                if (p != null)
                    p.Dispose();
            }
        }
        #endregion

        #region Plot Function
        /// <summary>
        ///  Calls Plot form and passes data to it to make a graph
        /// </summary>
        void DrawingChart()
        {

            plot Plot = new plot();

            if (samples != null)
            {
                Plot.Samples = samples;
                Plot.Draw();
                Plot.Show();

            }
        }
        #endregion

        #region Streaming. Shouldn't be used for BlockCapture
        /// <summary>
        /// Online streaming without capturing. 
        /// </summary>
        void Streaming()
        {
            int totalSamples = 0;
            uint triggeredAt = 0;
            uint status;
            uint sampleIntervall = uint.Parse(txtSamplingInterval.Text);
            status = Imports.SetChannel(_handle, (Imports.Channel)cbChannels.SelectedIndex, 1, (Imports.Coupling)cbCoupling.SelectedIndex, (Imports.Range)cbVoltage.SelectedIndex, 0);

            int sampleCount = 1024 * 100;
            _channelCount = 1; // only channel A
            appBuffers = new short[_channelCount * 2][];
            buffers = new short[_channelCount * 2][];

            PinnedArray<short>[] appBuffersPinned = new PinnedArray<short>[_channelCount * 2];


            buffers[0] = new short[sampleCount];
            buffers[1] = new short[sampleCount];

            appBuffers[0] = new short[sampleCount];
            appBuffers[1] = new short[sampleCount];

            appBuffersPinned[0] = new PinnedArray<short>(appBuffers[0]);
            appBuffersPinned[1] = new PinnedArray<short>(appBuffers[1]);

            status = Imports.SetDataBuffers(_handle, (Imports.Channel)(0), buffers[0], buffers[1], sampleCount, 0, Imports.RatioMode.None);
            status = Imports.RunStreaming(_handle, ref sampleIntervall, Imports.ReportedTimeUnits.NanoSeconds, 0, 1000000, 1, 1, Imports.RatioMode.None, (uint)sampleCount);
            status = Imports.GetStreamingLatestValues(_handle, streamingCallback, IntPtr.Zero);

            maxValue = new int[_startIndex + _sampleCount];
            minValue = new int[_startIndex + _sampleCount];

            if (_ready && _sampleCount > 0)
            {
                if (_trig > 0)
                {
                    triggeredAt = (uint)totalSamples + _trigAt;
                }
                totalSamples += _sampleCount;

                for (uint i = _startIndex; i < (_startIndex + _sampleCount); i++)
                {
                    maxValue[i] = adc_to_mv(appBuffersPinned[0].Target[i], (int)_channelSettings[(int)(Imports.Channel.ChannelA)].range);
                    minValue[i] = adc_to_mv(appBuffersPinned[1].Target[i], (int)_channelSettings[(int)(Imports.Channel.ChannelA)].range);
                }
                //Draw
                DrawingChart();
            }
        }
        #endregion

        #region Callback Function. is Needed for streaming.
        //Callback funktion for Streaming
        public void streamingCallback(short handle,
                                int noOfSamples,
                                uint startIndex,
                                short overflow,
                                uint triggerAt,
                                short triggered,
                                short autoStop,
                                IntPtr pVoid)
        {
            // used for streaming
            _sampleCount = noOfSamples;
            _startIndex = startIndex;
            _autoStop = autoStop != 0;

            _ready = true;

            // flags to show if & where a trigger has occurred
            _trig = triggered;

            if (_trig != 0)
                _trigAt = triggerAt;

            if (overflow != 0)
                _overflow = overflow;

            Array.Copy(buffers[0], _startIndex, appBuffers[0], _startIndex, _sampleCount); //max
            Array.Copy(buffers[1], _startIndex, appBuffers[1], _startIndex, _sampleCount);//min
        }
        #endregion


        #region Converters. Converts ADC and milivolts
        /// <summary>
        ///  Are used to converts between values. Are usable in triggered state
        /// </summary>
        /// <param name="v"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        // Convert an 16-bit ADC count into milivolts
        int adc_to_mv(short v, int range)
        {
            return (v * inputRanges[range]) / _maxValue;
        }

        // Convert a millivolt value into a 16-bit ADC count
        short mv_to_adc(short mv, short ch)
        {
            return (short)((mv * _maxValue) / inputRanges[ch]);
        }
        #endregion


    }

}




