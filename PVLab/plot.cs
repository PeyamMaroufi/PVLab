
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using PicoPinnedArray;
using PicoStatus;
using PS5000AImports;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace PVLab
{
    public partial class Plot : Form
    {


        #region private members
        System.Timers.Timer myTimer;

        private readonly short _handle;
        int _channelCount = 1;
        private ChannelSettings[] _channelSettings;

        private Imports.ps5000aBlockReady _callbackDelegate;


        Thread thread;
        uint _timebase;
        public static string ss;
        public double[] Samples { get; set; }
        public int[] SampleTime { get; set; }

        public int[] samplesTrigger;
        public int[] sampleTimeTrigger;


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
        public double[] sampleTimeCont;

        int numofEl = 0;
        int z = 1;

        // Click Helpers
        bool run = false;

        // PLOTT VARIABLER
        public LineSeries series1;
        public PlotModel myModel;
        public LinearAxis linearAxis1;
        public LinearAxis linearAxis2;
        //System.Windows.Forms.DataVisualization.Charting.Series series;

        public bool sendit = false;

        // Make a list to remember sampleCounts
        public List<int> sampleCountList;
        List<double> updateTimeList;
        List<double> UpdatedSampleList;
        #endregion

        #region properties
        /// <summary>
        /// Setting variable passed from other classes
        /// </summary>
        public Imports.Channel SelChannelr { get; set; }
        public Imports.Coupling SelCoupr { get; set; }
        public int SelRangeIndex { get; set; }
        public Imports.DeviceResolution resolutionr { get; set; }
        public Imports.Range selRange { get; set; }
        public int SelChannelIndexr { get; set; }
        public uint SampleIntervalr { get; set; }
        public ChannelSettings[] _channelSettingsr { get; set; }

        #endregion

        #region Construction
        /// <summary>
        /// Construction.
        /// </summary>
        /// <param name="handle"></param>
        public Plot(short handle)
        {
            InitializeComponent();
            _handle = handle;

            myTimer = new System.Timers.Timer
            {
                Interval = 500
            };

            SetupEverythingUI();

        }

        public void SetupEverythingUI()
        {


            ss = "Plot form is created . Construction is called" + Environment.NewLine;
            InsertText(ss);
            cbDirection.DataSource = Enum.GetValues(typeof(Imports.ThresholdDirection));
            sampleCountList = new List<int>();
            updateTimeList = new List<double>();
            UpdatedSampleList = new List<double>();
        }

        private void InsertText(string ss)
        {
            if (txtStatus.InvokeRequired)
            {
                txtStatus.Invoke((MethodInvoker)delegate ()
                {
                    txtStatus.AppendText(ss);
                });
            }
        }

        #endregion

        #region Methods required for streaming

        /* For streaming following are requiree:
         * 1. finding max for current resolution.
         * 2. setting channel for current contiguration.
         * 3. buffering set up
         * 4. running streamin
         * 5. getting values
         */

        /// <summary>
        /// Find max for current resolution
        /// </summary>
        public void FindMax()
        {
            uint status;
            status = Imports.MaximumValue(_handle, out _maxValue); // Set max. ADC Counts
            ss = ("Max value for current resolution founded " + _maxValue.ToString()) + Environment.NewLine;
            InsertText(ss);
        }

        /// <summary>
        /// setting channel for current configuration
        /// </summary>
        public void SetChannel()
        {
            uint status;
            status = Imports.SetChannel(_handle, SelChannelr, 1, SelCoupr, selRange, 0);
            ss = ("Set channel is done") + SelChannelr.ToString() + " Coupling  " + SelCoupr.ToString() + " Range " + selRange.ToString() + Environment.NewLine;
            InsertText(ss);
        }



        /// <summary>
        /// Required for current streaming especially for getting values and buffering
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="noOfSamples"></param>
        /// <param name="startIndex"></param>
        /// <param name="overflow"></param>
        /// <param name="triggerAt"></param>
        /// <param name="triggered"></param>
        /// <param name="autoStop"></param>
        /// <param name="pVoid"></param>
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

                    Array.Copy(buffers[0], _startIndex, appBuffers[0], _startIndex, _sampleCount); //max
                    Array.Copy(buffers[1], _startIndex, appBuffers[1], _startIndex, _sampleCount); // min

                }
            }
        }


        /// <summary>
        /// Running streaming.
        /// </summary>
        public void RunStreaming()
        {
            ss = ("Run streaming is initiated") + Environment.NewLine;
            if (txtStatus.InvokeRequired)
            {
                txtStatus.Invoke((MethodInvoker)delegate ()
                 {
                     txtStatus.AppendText(ss);

                 });
            }

            int sampleCount = 1024 * 100; /*  *100 is to make sure buffer large enough */

            appBuffers = new short[_channelCount * 2][];
            buffers = new short[_channelCount * 2][];

            uint preTrigger = 0;
            int totalSamples = 0;
            uint triggeredAt = 0;
            uint sampleInterval = SampleIntervalr;
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
            status = Imports.RunStreaming(_handle, ref sampleInterval, Imports.ReportedTimeUnits.NanoSeconds, preTrigger, 1000000 - preTrigger, 1, 1, Imports.RatioMode.None, (uint)sampleCount);
            SampleCont = new double[appBuffersPinned[0].Target.Length];
            sampleTimeCont = new double[appBuffersPinned[0].Target.Length];

            while (!_autoStop)
            {
                /* Poll until data is received. Until then, GetStreamingLatestValues wont call the callback */
                Thread.Sleep(0);
                _ready = false;
                status = Imports.GetStreamingLatestValues(_handle, StreamingCallback, IntPtr.Zero);


                //myTimer.Enabled = true;

                if (_ready && _sampleCount > 0) /* can be ready and have no data, if autoStop has fired */
                {
                    //myTimer.Enabled = true;

                    if (_trig > 0)
                    {
                        triggeredAt = (uint)totalSamples + _trigAt;
                    }

                    totalSamples += _sampleCount;
                    sampleCountList.Add(_sampleCount);
                    if (_trig > 0)
                    {

                    }

                    for (uint i = _startIndex; i < (_startIndex + _sampleCount); i++)
                    {
                        SampleCont[i] = adc_to_mv(appBuffersPinned[0].Target[i], inputRanges[SelRangeIndex]);
                        double power = SampleIntervalr * Math.Pow(10, -9);
                        sampleTimeCont[i] = (double)(i * power);

                    }


                }
            }
        }



        /// <summary>
        /// Scaling from sample values to mv. can require modification
        /// </summary>
        /// <param name="raw"></param>
        /// <param name="ch"></param>
        /// <returns></returns>
        public double adc_to_mv(int raw, int ch)
        {
            return (raw * inputRanges[ch]) / _maxValue; ;
        }
        #endregion

        #region Plotting

        /* Using two plots, one for recording where only plot is showing.
        * Second plot is streaming plot. It is a Oxyplot for Winform which requires 
        * less modification than MS chart control
        * In Streaming plot we first create a Model if there isn't already one. In case user 
        * click on recirst and then then streaming there  is already one. checking for null
        * plot is also required for updating.
        */

        /// <summary>
        /// Draw method for record plot
        /// </summary>
        /// <param name="range"></param>
        public void Draw(int range)
        {
            myModel = new PlotModel { Title = "Voltage level" };
            linearAxis1 = new LinearAxis { Position = AxisPosition.Bottom, Title = "Time in nanosec" };
            linearAxis2 = new LinearAxis { Position = AxisPosition.Left, Title = "Voltage" };
            myModel.Axes.Add(linearAxis1);
            myModel.Axes.Add(linearAxis2);

            series1 = new LineSeries
            {
                MarkerType = MarkerType.Circle,
                StrokeThickness = 1,
                MarkerSize = 1,
                Smooth = true,
                Title = "Voltage level"


            };


            ss = "Recording plot is called." + Environment.NewLine;
            txtStatus.AppendText(ss);
            //myTimer.Enabled = true;
            plotView1.Dock = DockStyle.Fill;

            for (int i = 0; i < Samples.Length; i++)
            {
                series1.Points.Add(new OxyPlot.DataPoint(SampleTime[i], Samples[i]));
            }


            myModel.Series.Add(series1);
            plotView1.Model = myModel;
            ss = "Recording plot is  succesfully created." + Environment.NewLine;
            txtStatus.AppendText(ss);
        }


        /// <summary>
        /// Streaming plot
        /// </summary>
        private void InitiateStreamPlot()
        {
            myModel = new PlotModel { Title = "Voltage level" };
            ss = "New streaming plot is starting" + Environment.NewLine;

            series1 = new LineSeries
            {
                MarkerType = MarkerType.Circle,
                StrokeThickness = 1,
                MarkerSize = 1,
                Smooth = true,
                Title = "Voltage level",
                CanTrackerInterpolatePoints = false,

            };

            linearAxis1 = new LinearAxis { Position = AxisPosition.Bottom, Title = "Time in nanosec" };
            linearAxis2 = new LinearAxis { Position = AxisPosition.Left, Title = "Voltage" };

            myModel.Axes.Add(linearAxis1);
            myModel.Axes.Add(linearAxis2);

            for (int i = 0; i < SampleCont.Length; i++)
            {
                series1.Points.Add(new OxyPlot.DataPoint(sampleTimeCont[i], SampleCont[i]));
            }

            myModel.Series.Add(series1);
            plotView1.Model = myModel;
            sendit = true;
        }

        /// <summary>
        /// Updating the chart.Invoke is required for not blocking UI thread
        /// since we use a timer we do calculations and reading in a other thread
        /// than UI thread.and for getting back those values and show them on UI
        /// thread invoking is required.
        /// </summary>
        public void UpdatePlot()
        {

            if (plotView1.InvokeRequired)
            {
                plotView1.Invoke((MethodInvoker)UpdatePlot);
                //lbPoints.Invoke((MethodInvoker)UpdatePlot);

            }
            else
            {
                //while (series1.Points.Count > 0)
                //{

                //}
                if (sampleCountList.Count > 1)
                {
                    if (updateTimeList.Count > 0)
                    {
                        updateTimeList.Clear();
                    }

                    for (int i = sampleCountList[sampleCountList.Count - 2]; i < sampleCountList[sampleCountList.Count - 1]; i++)
                    {
                        updateTimeList.Add(sampleTimeCont[i] - sampleTimeCont[sampleCountList[sampleCountList.Count - 2]]);
                        UpdatedSampleList.Add(SampleCont[i]);

                    }

                }
                if (series1.Points.Count > 0)
                    series1.Points.Clear();

                if (myModel.Series.Count > 0)
                    myModel.Series.Clear();


                string num = series1.Points.Count.ToString();
                lbPoints.Text = series1.Points.Count.ToString();
                //myModel.Series.Add(series1);

                for (int i = 0; i < SampleCont.Length; i++)
                {
                    series1.Points.Add(new OxyPlot.DataPoint(sampleTimeCont[i], SampleCont[i]));
                }

                myModel.Series.Add(series1);

                //plotView1.Refresh();
                plotView1.InvalidatePlot(true);



            }
        }
        #endregion
        #region Timer method
        /// <summary>
        /// checking for myModel.to choose between updating and creating.
        /// reason for this check is first time the mymodel is running it needs
        /// to be created.second time and after we use the same model.
        /// </summary>
        /// <param name = "sender" ></ param >
        /// < param name= "e" ></ param >
        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            ss = "Timer elapsed" + Environment.NewLine;

            InsertText(ss);

            if (!sendit)
            {
                InitiateStreamPlot();
                //SampleCont = null;
                //sampleTimeCont = null;
            }
            else
            {

                UpdatePlot();
            }


        }


        #endregion

        #region UI event
        /// <summary>
        /// Click event. moving calculation and reading samples to an other thread
        /// to avoid freezing and getting more performance.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStream_Click(object sender, EventArgs e)
        {
            ss = "Start Streaming button is clicked" + Environment.NewLine;
            InsertText(ss);

            if (!run)
            {
                ss = "Thread was null and therefor gets created" + Environment.NewLine;
                InsertText(ss);
                thread = new Thread(new ThreadStart(RunStreaming));
                thread.Start();
                btnStream.Text = "Stop Streaming";
                myTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
                myTimer.Enabled = true;
                ss = "Timer Started" + Environment.NewLine;
                InsertText(ss);
                run = true;
            }
            else
            {
                ss = "Aborting thread. A temporary solution" + Environment.NewLine;
                InsertText(ss);
                ss = "Timer Stopped" + Environment.NewLine;
                Imports.CloseUnit(_handle);
                btnStream.Text = "Start steaming";
                thread = null;
                if (myTimer.Enabled == true)
                    myTimer.Enabled = false;
                InsertText(ss);
                run = false;

            }

        }


        /// <summary>
        /// Before trigger you need to close the thread.
        /// To avoid complexity , ETS is intentionally not included.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbRepeat_CheckedChanged(object sender, EventArgs e)
        {
            if (thread != null)
                MessageBox.Show("Close streaming before choosing trigger!");
        }

        private void rbNone_CheckedChanged(object sender, EventArgs e)
        {
            if (thread != null)
                MessageBox.Show("Close streaming before choosing trigger!");
        }

        private void rbAuto_CheckedChanged(object sender, EventArgs e)
        {
            if (thread != null)
                MessageBox.Show("Close streaming before choosing trigger!");
        }

        private void rbSingle_CheckedChanged(object sender, EventArgs e)
        {
            if (thread != null)
                MessageBox.Show("Close streaming before choosing trigger!");
        }
        #endregion


        #region Triggers
        /// <summary>
        /// Repeat algorithm
        /// </summary>
        private void repeatTrigger()
        {

        }

        #endregion
    }


}

