using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using PicoPinnedArray;
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
using System.Timers;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace PVLab
{
    public partial class Plot : Form
    {
        System.Timers.Timer myTimer;

        #region private members
        private readonly short _handle;
        int _channelCount = 1;
        private ChannelSettings[] _channelSettings;
        PlotModel myModel;
        LinearAxis linearAxis1;
        LinearAxis linearAxis2;

        public int[] Samples { get; set; }
        public int[] SampleTime { get; set; }

        public int SamplesCont { get; set; }
        public int SampleTimeCont { get; set; }

        int MinimumY;
        int MaximumY;
        public bool sss;

        short[][] appBuffers;
        short[][] buffers;

        ushort[] inputRanges = { 10, 20, 50, 100, 200, 500, 1000, 2000, 5000, 10000, 20000, 50000 };

        bool _autoStop;
        bool _powerSupplyConnected;
        bool _ready = false;

        short _maxValue;
        short _trig = 0;

        int _noEnabledChannels;
        int _sampleCount;

        uint _trigAt = 0;
        uint _startIndex;
        public double[] SampleCont;
        public int[] sampleTimeCont;
        LineSeries series1;
        #endregion



        public Imports.Channel SelChannelr { get; set; }
        public Imports.Coupling SelCoupr { get; set; }
        public Imports.DeviceResolution resolutionr { get; set; }
        public Imports.Range selRange { get; set; }
        public int SelChannelIndexr { get; set; }
        public uint SampleIntervalr { get; set; }
        public ChannelSettings[] _channelSettingsr { get; set; }


        public void FindMax()
        {
            uint status;
            status = Imports.MaximumValue(_handle, out _maxValue); // Set max. ADC Counts

        }

        public void SetChannel()
        {
            uint status;
            status = Imports.SetChannel(_handle, SelChannelr, 1, SelCoupr, selRange, 0);
        }




        public void StreamingCallback(short handle,
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
            _trigAt = triggerAt;

            if (_sampleCount != 0)
            {
                for (uint ch = 0; ch < _channelCount * 2; ch += 2)
                {
                    //if (_channelSettings[(int)(Imports.Channel.ChannelA + (ch / 2))].enabled)
                    //{

                    Array.Copy(buffers[0], _startIndex, appBuffers[0], _startIndex, _sampleCount); //max
                    Array.Copy(buffers[1], _startIndex, appBuffers[1], _startIndex, _sampleCount);//min

                    //}
                }
            }
        }


        public double adc_to_mv(int raw, int ch)
        {
            return (raw * inputRanges[ch]) / _maxValue; ;
        }


        /* ------------------------------------------ PLOTS ----------------------------------*/
        /* ------------------------------------------ PLOTS ----------------------------------*/
        /* ------------------------------------------ PLOTS ----------------------------------*/
        /* ------------------------------------------ PLOTS ----------------------------------*/





        public void Draw(int range)
        {
            myTimer.Enabled = false;
            btnStream.Visible = false;
            myModel = new PlotModel { Title = "Example 1" };
            linearAxis1 = new LinearAxis { Position = AxisPosition.Bottom };
            linearAxis2 = new LinearAxis { Position = AxisPosition.Left };
            myModel.Axes.Add(linearAxis1);
            myModel.Axes.Add(linearAxis2);
            LineSeries series1 = new LineSeries
            {
                MarkerType = MarkerType.Circle,
                StrokeThickness = 1,
                MarkerSize = 1,
                Title = "Voltage level"
            };


            for (int i = 0; i < Samples.Length; i++)
            {
                series1.Points.Add(new OxyPlot.DataPoint(SampleTime[i], Samples[i]));
            }

            myModel.Series.Add(series1);
            plotView1.Model = myModel;

        }
        


        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {

            if (myModel == null)
            {
                btnStream.Enabled = true;
                myModel = new PlotModel { Title = "Example 1" };
                linearAxis1 = new LinearAxis { Position = AxisPosition.Bottom };
                linearAxis2 = new LinearAxis { Position = AxisPosition.Left };
                myModel.Axes.Add(linearAxis1);
                myModel.Axes.Add(linearAxis2);
                series1 = new LineSeries
                {
                    MarkerType = MarkerType.Circle,
                    StrokeThickness = 1,
                    MarkerSize = 1,
                    Title = "Voltage level"
                };

                for (int i = 0; i < SampleCont.Length; i++)
                {
                    series1.Points.Add(new OxyPlot.DataPoint(sampleTimeCont[i], SampleCont[i]));
                }

                myModel.Series.Add(series1);
                plotView1.Model = myModel;
            }
            else
            {
                new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    series1.Points.Clear();
                   
                }).Start();

                if (plotView1.InvokeRequired)
                {
                    plotView1.Invoke((MethodInvoker)delegate ()
                    {
                        for (int i = 0; i < SampleCont.Length; i++)
                        {
                            series1.Points.Add(new OxyPlot.DataPoint(sampleTimeCont[i], SampleCont[i]));
                        }
                        plotView1.InvalidatePlot(true);
                    });
                        
                }
                else
                {

                }

            }
            Thread.Sleep(200);

            myTimer.Enabled = true;

        }

        public Plot(short handle)
        {
            InitializeComponent();
            _handle = handle;
            myTimer = new System.Timers.Timer
            {
                Interval = 1000
            };
            cbDirection.DataSource = Enum.GetValues(typeof(Imports.RatioMode));
        }

        private void btnStream_Click(object sender, EventArgs e)
        {
            int sampleCount = 1024 * 100; /*  *100 is to make sure buffer large enough */

            appBuffers = new short[_channelCount * 2][];
            buffers = new short[_channelCount * 2][];

            uint preTrigger = 0;
            int totalSamples = 0;
            uint triggeredAt = 0;
            uint sampleInterval = 1;
            uint status;




            // Use Pinned Arrays for the application buffers
            PinnedArray<short>[] appBuffersPinned = new PinnedArray<short>[_channelCount * 2];

            for (int ch = 0; ch < _channelCount * 2; ch += 2) // create data buffers
            {
                buffers[ch] = new short[sampleCount];
                buffers[ch + 1] = new short[sampleCount];

                appBuffers[ch] = new short[sampleCount];
                appBuffers[ch + 1] = new short[sampleCount];

                appBuffersPinned[ch] = new PinnedArray<short>(appBuffers[ch]);
                appBuffersPinned[ch + 1] = new PinnedArray<short>(appBuffers[ch + 1]);

                status = Imports.SetDataBuffers(_handle, (Imports.Channel)(ch / 2), buffers[ch], buffers[ch + 1], sampleCount, 0, Imports.RatioMode.None);
            }
            _autoStop = false;
            status = Imports.RunStreaming(_handle, ref sampleInterval, Imports.ReportedTimeUnits.MicroSeconds, preTrigger, 1000000 - preTrigger, 1, 1, Imports.RatioMode.None, (uint)sampleCount);
            SampleCont = new double[appBuffersPinned[0].Target.Length];
            sampleTimeCont = new int[appBuffersPinned[0].Target.Length];


            while (!_autoStop)
            {
                /* Poll until data is received. Until then, GetStreamingLatestValues wont call the callback */
                Thread.Sleep(0);
                _ready = false;
                status = Imports.GetStreamingLatestValues(_handle, StreamingCallback, IntPtr.Zero);

                if (_ready)
                {
                    myTimer.Enabled = true;
                    myTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent);
                }
                if (_ready && _sampleCount > 0) /* can be ready and have no data, if autoStop has fired */
                {


                    if (_trig > 0)
                    {
                        triggeredAt = (uint)totalSamples + _trigAt;
                    }

                    totalSamples += _sampleCount;

                    if (_trig > 0)
                    {

                    }

                    for (uint i = _startIndex; i < (_startIndex + _sampleCount); i++)
                    {
                        SampleCont[i] = adc_to_mv(appBuffersPinned[0].Target[i], inputRanges[SelChannelIndexr]);
                        sampleTimeCont[i] = (int)(i * SampleIntervalr);

                    }

                }

            }
            //myTimer.Stop();



            //Imports.Stop(_handle);
        }

      
    }
}
