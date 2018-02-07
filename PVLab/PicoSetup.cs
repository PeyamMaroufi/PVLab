using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using PicoPinnedArray;
using PicoStatus;
using PS5000AImports;

namespace PVLab
{
    public struct ChannelSettings
    {
        public Imports.Range range;
        public bool enabled;
    }


    class SettingUpBlockRecordAndStream
    {

        #region Private and public members
        public int[] sampleTime;
        private short _handle;
        public const int BUFFER_SIZE = 1024;
        public const int MAX_CHANNELS = 4;
        public const int QUAD_SCOPE = 4;
        public const int DUAL_SCOPE = 2;
        public short _overflow = 0;



        // Graph class
        Plot plot;
        StreamingClass StreamingSet;


        uint _timebase;
        short _oversample = 1;
        bool _scaleVoltages = true;

        ushort[] inputRanges = { 10, 20, 50, 100, 200, 500, 1000, 2000, 5000, 10000, 20000, 50000 };
        bool _ready = false;
        short _trig = 0;
        uint _trigAt = 0;
        int _sampleCount;
        uint _startIndex = 0;
        bool _autoStop;
        private ChannelSettings[] _channelSettings;
        private int _channelCount;
        private int _digitalPorts;
        private Imports.ps5000aBlockReady _callbackDelegate;
        double[] samples;
        int[] time;
        short _maxValue;
        int[] minValue;
        int[] maxValue;
        public static string s;

        public Imports.Channel SelChannel { get; set; }
        public Imports.Range SelVolt { get; set; }
        public Imports.Coupling SelCoup { get; set; }
        public Imports.DeviceResolution resolution { get; set; }
        public uint SampleInterval { get; set; }
        public int SelChannelIndex { get; set; }
        public int SelRangeIndex { get; set; }
        PinnedArray<short>[] appBuffersPinned;
        int bufferSize = 1024 * 100;
        uint streamingSamples = 60000;
        public short[][] channelBuffers;
        public short[][] buffers;
        private short handle;
        private ChannelSettings[] _channelSettings1;

        public static int t = 1;
        public static int d;
        #endregion

        #region Construction
        public SettingUpBlockRecordAndStream()
        {



        }
        #endregion

        #region Open device method

        public void OpenUnit()
        {
            // The string builder is need to save the string data about the device
            StringBuilder UnitInfo = new StringBuilder(80);

            // handel is the device itself. That represent the device beein connected and available
            short handle;

            // String array of the description
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

            // This is actually not needed since we close the unit from UI. but for safety we let it be here.
            if (_handle > 0)
            {
                Imports.CloseUnit(_handle);
                _handle = 0;

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
                        t = 0;

                    }
                    else
                    {
                        // Do nothing - power supply connected
                    }


                    for (int i = 0; i < 9; i++)
                    {
                        short requiredSize;
                        Imports.GetUnitInfo(_handle, UnitInfo, 80, out requiredSize, (uint)i);
                        s += (description[i] + UnitInfo + "\n") + Environment.NewLine;

                        // We make a new channelsetting in order to make the rest to work
                        // channelsetting is a structure defined earlier.
                        _channelSettings = new ChannelSettings[1];
                        _channelSettings[0].enabled = true;
                        _channelSettings[0].range = SelVolt;


                    }

                    d = 1;

                }
            }
        }
        #endregion

        #region Close unit method
        /// <summary>
        ///  Closing the unit and inactivate the handel
        /// </summary>
        public void CloseUnit()
        {
            Imports.CloseUnit(_handle);
            _handle = 0;
        }
        #endregion

        #region Block capture
        /// <summary>
        /// The reason why this method is directly implemented is that we want to make the
        /// RunBlock() method private and call it publicly.
        /// </summary>
        public void BlockRunner()
        {
            RunBlock();
        }
        #endregion

        #region RunBlock Function to capture measurement
        /// <summary>
        ///  we use this method to do a runblock
        /// </summary>
        /// 
        private void BlockCallback(short handle, short status, IntPtr pVoid)
        {
            // flag to say done reading data
            if (status != (short)StatusCodes.PICO_CANCELLED)
                _ready = true;
        }

        private void SetChannel()
        {

            // Set it to channel
            uint status;
            status = Imports.SetChannel(_handle, SelChannel, 1, SelCoup, SelVolt, 0);

        }

        private void RunBlock()
        {
            // determine maximum value for scaling
            Imports.MaximumValue(_handle, out _maxValue);


            // Setting channel. Is needed for everytime you want to do something. eg when you change the voltage range, resulotion.
            uint status;
            status = Imports.SetChannel(_handle, SelChannel, 1, SelCoup, SelVolt, 0);



            // SetsimplerTrigger function
            short enable = 0;
            uint delay = 0;
            short threshold = 25000;
            short auto = 0;
            status = Imports.SetSimpleTrigger(_handle, enable, SelChannel, threshold, Imports.ThresholdDirection.Rising, delay, auto);


            _ready = false;
            _callbackDelegate = BlockCallback;
            _channelCount = 1;

            bool retry;
            uint sampleCount = 100;
            PinnedArray<short>[] minPinned = new PinnedArray<short>[_channelCount];
            PinnedArray<short>[] maxPinned = new PinnedArray<short>[_channelCount];


            int timeIndisposed;

            short[] minBuffers = new short[sampleCount];
            short[] maxBuffers = new short[sampleCount];


            // Setting the buffer. Using set BufferS fo all channels. The HV91 uses setbuffer instead of setBuffers. the reason
            // is that we use only one channel for a certain purpose.
            status = Imports.SetDataBuffers(_handle, SelChannel, maxBuffers, minBuffers, (int)sampleCount, 0, Imports.RatioMode.None);



            /*  find the maximum number of samples, the time interval (in timeUnits),
             *		 the most suitable time units, and the maximum _oversample at the current _timebase*/
            int timeInterval;
            int maxSamples;
            double sampleRedo = (double)(SampleInterval * Math.Pow(10, -9));
            // Over own _timebase. This will slow the operation but should work
            _timebase = (uint)(125000000 * sampleRedo + 2);
            while (Imports.GetTimebase(_handle, _timebase, (int)sampleCount, out timeInterval, out maxSamples, 0) != 0)
            {
                // This will try to come to our chosen timebase as close as possible.
                _timebase++;

            }


            /* Start it collecting, then wait for completion*/
            _ready = false;
            _callbackDelegate = BlockCallback;


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
                    //txtStatus.AppendText(("Run Block Called\n") + Environment.NewLine);
                }
            }
            while (retry);

            //txtStatus.AppendText(("Waiting for Data\n") + Environment.NewLine);

            while (!_ready)
            {
                Thread.Sleep(500);
            }

            Imports.Stop(_handle);



            if (_ready)
            {
                short overflow;

                // Now get the values 
                status = Imports.GetValues(_handle, 0, ref sampleCount, 1, Imports.DownSamplingMode.None, 0, out overflow);
                minPinned[0] = new PinnedArray<short>(minBuffers);
                maxPinned[0] = new PinnedArray<short>(maxBuffers);
                samples = new double[sampleCount];
                sampleTime = new int[sampleCount];

                if (status == (short)StatusCodes.PICO_OK)
                {

                    // Scale the data with respect to the maximum value.
                    for (int x = 0; x < sampleCount; x++)
                    {
                        samples[x] = adc_to_mv(maxPinned[0].Target[x], SelRangeIndex);
                    }

                    // Make the time to ms
                    for (int i = 0; i < sampleCount; i++)
                    {
                        sampleTime[i] = (int)(i * SampleInterval)/1000000;
                    }


                    // Calling th drawing method
                    DrawingChart(samples, sampleTime);

                }
                else
                {
                    //txtStatus.AppendText(("No Data\n") + Environment.NewLine);

                }
            }
            else
            {
                //txtStatus.AppendText(("data collection aborted\n") + Environment.NewLine);
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

        #region Streaming method calls
        /// <summary>
        /// Here we first need to find the maximum value of the streamin based on current setting of resolution
        /// and voltage range. we look if the device is connected and available first and then do the findmax.
        /// we set the channel after chosen settings. It is essential that the channel is set before any operation.
        /// We sett the buffer. We only need to call the setBuffer and not setBuffers. the reason is that we use
        /// only one channel.
        /// We then set the streamin up using SetStreaming(). and getting the value from GetLatestValues().
        /// </summary>
        public void Streaming()
        {
            if (_handle != 0)
            {
                Plot plot = new Plot(_handle)
                {
                    SelChannelr = SelChannel,
                    SelCoupr = SelCoup,
                    selRange = SelVolt,
                    resolutionr = resolution,
                    SelChannelIndexr = SelChannelIndex,
                    SampleIntervalr = SampleInterval,
                    _channelSettingsr = _channelSettings,


                };
                //// Find Max to scaling
                plot.FindMax();

                //// Set the channel
                plot.SetChannel();



                plot.Show();


            }
        }
        #endregion

        #region Plot Function for Capturing
        /// <summary>
        ///  Calls Plot form and passes data to it to make a graph
        /// </summary>
        void DrawingChart(double[] data, int[] time)
        {
            double[] samples = data;
            int[] sampleTime = time;
            Plot plot = new Plot(_handle);
            plot.Show();
            int rangePlot = SelChannelIndex;
            if (samples != null)
            {
                plot.Samples = samples;
                plot.SampleTime = sampleTime;

                plot.Draw(rangePlot);
                plot.Show();

            }
        }
        #endregion


    }
}
