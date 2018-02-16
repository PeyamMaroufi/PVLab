﻿
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using PicoPinnedArray;
using PicoStatus;
using PS5000AImports;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;


namespace PVLab
{
    public partial class Plot : Form
    {
        #region private members
        System.Timers.Timer myTimer;

        private short _handle;
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
        // Threats
        Thread thread1;
        // Click Helpers
        bool run = false;
        ushort RunStuffOrNot;
        double iteration = 0;

        // PLOTT VARIABLER
        public LineSeries series1;
        public LineSeries series2;

        public PlotModel myModel;
        public LinearAxis linearAxis1;
        public LinearAxis linearAxis2;
        public bool sendit = false;

        // Trigger at 
        int triggerAtNumber;
        List<double> TimeToPlot;
        List<double> SampleToPlot;
        List<int> Indeces;
        int Error;
        bool runRepeat;
        double timeAtIndex;
        double sampleAtIndex;
        bool runSingle;
        int indexOfComboBox;
        int CenterValue;

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
            SetupEverythingUI();

            // To stop streaming without droppping the device
            run = false;

        }

        public void SetupEverythingUI()
        {

            ss = "Plot form is created . Construction is called" + Environment.NewLine;
            InsertText(ss);

            cbDirection.Items.Add("Rising");
            cbDirection.Items.Add("Falling");

            sampleCountList = new List<int>();
            updateTimeList = new List<double>();
            UpdatedSampleList = new List<double>();
            TimeToPlot = new List<double>();
            SampleToPlot = new List<double>();
            Indeces = new List<int>();
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


            while (!_autoStop && RunStuffOrNot != 0)
            {
                /* Poll until data is received. Until then, GetStreamingLatestValues wont call the callback */
                Thread.Sleep(0);
                _ready = false;
                status = Imports.GetStreamingLatestValues(_handle, StreamingCallback, IntPtr.Zero);

                if (_ready && _sampleCount > 0) /* can be ready and have no data, if autoStop has fired */
                {
                    iteration += 1;
                    ss = "New Iteration " + iteration + Environment.NewLine;
                    InsertText(ss);

                    // Since streaming can be very large amount of data, we choose to remove the old value
                    // if we decide to not do that the memmory taken by streaming will increase very fast
                    if (updateTimeList.Count > 0)
                    {
                        updateTimeList.Clear();
                        UpdatedSampleList.Clear();
                    }

                    if (_trig > 0)
                    {
                        triggeredAt = (uint)totalSamples + _trigAt;
                    }

                    // Each iteration the number of _samplecount will vary. And the values always start at zero. 
                    // for instance First iteration start at 0 to t = 100 and second iteration it starts at the same
                    // point it finished.
                    totalSamples += _sampleCount;
                    sampleCountList.Add(totalSamples);

                    if (_trig > 0)
                    {

                    }

                    InsertText("Number of samples " + _sampleCount.ToString() + Environment.NewLine + "Start index is " + _startIndex.ToString() + Environment.NewLine);

                    for (uint i = _startIndex; i < (_startIndex + _sampleCount); i++)
                    {
                        // Insert samples in a list
                        UpdatedSampleList.Add(adc_to_mv(appBuffersPinned[0].Target[i], inputRanges[SelRangeIndex]));
                        double power = SampleIntervalr * Math.Pow(10, -9);

                        // Insert time in a list
                        updateTimeList.Add((double)(i * power));

                    }

                    if (rbNone.Checked)
                    {
                        // Calling the plotting Method for unconstraint streaming
                        PlottingMethod(updateTimeList, UpdatedSampleList);
                    }
                    else if (rbRepeat.Checked && !runRepeat)
                    {
                        // No direction specified
                        RepeatTrigger(updateTimeList, UpdatedSampleList, triggerAtNumber);
                    }
                    else if (rbSingle.Checked && !runSingle)
                    {
                        // Call singleTrigger without any direction specified.
                        SingleTrigger(updateTimeList, UpdatedSampleList, triggerAtNumber);
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
        /* Using two plots, one for recording where only plot is streaming.
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
        /// gets value from streaming and triggers and creates the 
        /// plot. this is called only once in a streaming and triggering.
        /// </summary>
        private void InitiateStreamPlot(List<double> time, List<double> sample, params object[] restOfPointsToPlote)
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

            series2 = new LineSeries
            {
                Color = OxyColors.Red,
                MarkerFill = OxyColors.Blue,
                MarkerStroke = OxyColors.Red,
                MarkerType = MarkerType.Circle,
                StrokeThickness = 0,
                MarkerSize = 4,
            };

            linearAxis1 = new LinearAxis { Position = AxisPosition.Bottom, Title = "Time in nanosec" };
            linearAxis2 = new LinearAxis { Position = AxisPosition.Left, Title = "Voltage" };

            myModel.Axes.Add(linearAxis1);
            myModel.Axes.Add(linearAxis2);

            // Show the triggered value on the plot
            if (restOfPointsToPlote.Length > 0)
            {
                timeAtIndex = (double)restOfPointsToPlote[0];
                sampleAtIndex = (double)restOfPointsToPlote[1];
                series2.Points.Add(new OxyPlot.DataPoint(timeAtIndex, sampleAtIndex));
                myModel.Series.Add(series2);
            }

            for (int i = 0; i < time.Count - 1; i++)
            {
                series1.Points.Add(new OxyPlot.DataPoint(time[i], sample[i]));
            }

            myModel.Series.Add(series1);
            plotView1.Model = myModel;
            sendit = true;
        }

        /// <summary>
        /// Updating the chart.Invoke is required for not blocking UI thread.
        /// Reciving the optional data when trigger is on.
        /// </summary>
        public void UpdatePlot(List<double> time, List<double> sample, params object[] restOfPointsToUdate)
        {

            if (plotView1.InvokeRequired)
            {
                plotView1.Invoke((MethodInvoker)delegate
                {
                    UpdatePlot(time, sample, restOfPointsToUdate);

                });

            }
            else
            {
                if (series2.Points.Count > 0)
                    series2.Points.Clear();

                if (series1.Points.Count > 0)
                    series1.Points.Clear();

                // Clear the series
                if (myModel.Series.Count > 0)
                    myModel.Series.Clear();


                string num = series1.Points.Count.ToString();
                lbPoints.Text = series1.Points.Count.ToString();
                //myModel.Series.Add(series1);

                if (restOfPointsToUdate.Length > 0)
                {
                    timeAtIndex = (double)restOfPointsToUdate[0];
                    sampleAtIndex = (double)restOfPointsToUdate[1];
                    series2.Points.Add(new OxyPlot.DataPoint(timeAtIndex, sampleAtIndex));
                    myModel.Series.Add(series2);
                }


                // Adding new values
                for (int i = 0; i < time.Count - 1; i++)
                {
                    series1.Points.Add(new OxyPlot.DataPoint(time[i], sample[i]));
                }

                // Add to the plot
                myModel.Series.Add(series1);

                // Update
                plotView1.InvalidatePlot(true);

            }
        }

        /// <summary>
        /// Help function for plotting. restOfPoints is an optional parameter
        /// that can be used when triggered to avoid overloading.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="sample"></param>
        /// <param name="restOfPoints"></param>
        private void PlottingMethod(List<double> time, List<double> sample, params object[] restOfPoints)
        {
            if (!sendit)
            {
                // Initiate the plot first time
                InitiateStreamPlot(time, sample, restOfPoints);
            }
            else
            {
                ss = "Plot will get updated" + Environment.NewLine;
                InsertText(ss);
                // If the plot has been created then do this
                UpdatePlot(time, sample, restOfPoints);
            }
        }

        #endregion

        #region Triggers
        /// <summary>
        /// Repeat trigger: User defines a value that repeats in the streaming. This method finds that 
        /// value and show it on the plot.
        /// 
        /// How it is done: 
        /// Finds all the points point around the triggered point.
        /// A tolerance is set to 5 but can be more and less if wished.  Collecting the indeces
        /// of element that are equal to searched value. If those values are at the beginning or middle
        /// or end of a serie, different operations will take place.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="sample"></param>
        /// <param name="f"></param>
        private void RepeatTrigger(List<double> time, List<double> sample, double f)
        {

            // since the streaming occurs in a loop we need to be sure that the previouse values are gone.
            // The reason for that is the fact that the values in the streaming change so using the same position
            // in last loop will give wrong values. For instance if the index of an element of interest is 3
            // and the value is 2000, and if we choose not to clear that index value in next iteration, the length of 
            // indeces will increase and be longer.
            if (Indeces.Count > 0)
            {
                ss = "Previouse indeces will get ereased now" + Environment.NewLine;
                InsertText(ss);
                Indeces.Clear();
            }

            ss = "Search for indeces that corrospond to the triggered value that are between  " + (Error + f) + " and " + (f - Error) + Environment.NewLine;
            InsertText(ss);

            // Finding the indeces of the element that trigger is sett to
            for (int i = 0; i < sample.Count; i++)
            {
                if (sample[i] == f || (sample[i] <= f + Error && sample[i] >= f - Error))
                {
                    Indeces.Add(i);
                }
            }



            // count the number of element in in updatedSampleList
            var countValue = time.Count;
            var countIndec = Indeces.Count;
            double time1;
            double sample1;
            ss = "There are " + countIndec + " values corrosponding to the triggered value" + Environment.NewLine;
            InsertText(ss);

            // if the number of elements of updatedSampleList is less than 41. show all of 
            // the list
            if (countValue < 41)
            {
                // Plot all value
            }
            else
            {
                // We need to plot the values for each iteration. Let's assume that we find a value that is equal to the trigged value
                // we need to buil a curve that goes through that value. we choose to go 5 position forward and 5 backwards. The reason is
                // only to see the curves at that time. Since we do this in a loop then the triggered could be at the end of the list where 
                // it is impossible to go forward.
                for (int i = 0; i < Indeces.Count; i++)
                {
                    if (!runRepeat)
                    {
                        SampleToPlot.Clear();
                        TimeToPlot.Clear();

                        // Checks different situation of trigging
                        bool isThereValueAround = (Indeces[i] > 5 && Indeces[i] < time.Count - 5) ? true : false;
                        bool isNoValueBefore = ((Indeces[i] < 5 && Indeces[i] < time.Count - 5) && CheckBoxValidation()) ? true : false;
                        bool isNoValueAfter = ((Indeces[i] > 5 && Indeces[i] > time.Count - 5) && CheckBoxValidation()) ? true : false;

                        if (isThereValueAround)
                        {
                            // Refer to the time intervall
                            double repeatIndex = SampleIntervalr;

                            // In normal condition when the value has 20 elements before
                            // and after.
                            ss = "There are more than 5 values after and before the triggered value in this iteration " + Environment.NewLine;
                            InsertText(ss);
                            for (int d = Indeces[i] - 5; d < Indeces[i] + 5; d++)
                            {
                                SampleToPlot.Add(sample[d]);
                            }

                            // Set negative side of time and zero time point
                            for (int d = Indeces[i]; d >= Indeces[i] - 5; d--)
                            {
                                if (d == Indeces[i])
                                {
                                    TimeToPlot.Add(0);
                                }
                                else
                                {
                                    TimeToPlot.Add(-repeatIndex);
                                    repeatIndex--;
                                }
                            }

                            // Set positive side of time and zero time point
                            for (int d = Indeces[i] + 1; d <= Indeces[i] + 5; d++)
                            {
                                TimeToPlot.Add(repeatIndex);
                                repeatIndex++;
                            }

                            // Sort the elements from most negative to most positive.
                            TimeToPlot.Sort();

                        }
                        else if (isNoValueBefore)
                        {
                            // Refer to sampling interval
                            double repeatIndex = SampleIntervalr;

                            // When the value has less elements before it
                            ss = "There are less than 5 values  before the triggered value in this iteration " + Environment.NewLine;
                            InsertText(ss);
                            for (int d = Indeces[i]; d < Indeces[i] + 5; d++)
                            {
                                SampleToPlot.Add(sample[d]);
                            }

                            // Set negative side of time and zero time point
                            for (int d = Indeces[i]; d >= Indeces[i] - 5; d--)
                            {
                                if (d == Indeces[i])
                                {
                                    TimeToPlot.Add(0);
                                }
                                else
                                {
                                    TimeToPlot.Add(repeatIndex);
                                    repeatIndex--;
                                }
                            }
                        }
                        else if (isNoValueAfter)
                        {
                            // Refer to the time intervall
                            double repeatIndex = SampleIntervalr;

                            // then value doesnt have enough elements after
                            ss = "There are less than 5 values after the triggered value in this iteration " + Environment.NewLine;
                            InsertText(ss);
                            for (int d = Indeces[i] - 5; d < time.Count; d++)
                            {
                                SampleToPlot.Add(sample[d]);
                            }

                            // Set positive side of time and zero time point
                            for (int d = Indeces[i]; d <= Indeces[i] + 5; d++)
                            {
                                if (d == Indeces[i])
                                {
                                    TimeToPlot.Add(0);
                                }
                                else
                                {
                                    TimeToPlot.Add(repeatIndex);
                                    repeatIndex++;
                                }

                            }
                        }

                        // The value that the user wants to trigger at is as following
                        time1 = time[Indeces[i]];
                        sample1 = sample[Indeces[i]];
                        ss = "The triggered value for this iteration is  " + sample1 + Environment.NewLine;
                        InsertText(ss);

                        // When selecting the trigger state(rising and falling)
                        // we look at the value after. if that value is higher then
                        // the voltage is increasing. If the value is less then the 
                        // voltage is decreasing. For this to happen we need to be 
                        // sure that the combox box is active and user has choosen
                        // an item from the list. We pass the value to plottingmetod.
                        // If instead no item in the combo box is selected then we do 
                        // repeat trigger in both direction.

                        CenterValue = SampleToPlot.FindIndex(item => item == sample1);
                        bool ExistValueAround = ((SampleToPlot.Count > 0) && (CenterValue < SampleToPlot.Count)) ? true : false;


                        switch (GetIndexComBo())
                        {
                            // No direction
                            case -1:
                                ss = "No item is selected in the combo box " + Environment.NewLine;
                                InsertText(ss);

                                // Show
                                PlottingMethod(TimeToPlot, SampleToPlot, time1, sample1);
                                break;

                            // Rising
                            case 0:
                                if (ExistValueAround && (SampleToPlot[CenterValue + 1] > sample1))
                                {

                                    ss = "Rising mode is selected in the combo box" + Environment.NewLine;
                                    // Gör rising
                                    PlottingMethod(TimeToPlot, SampleToPlot, time1, sample1);
                                }
                                break;

                            // Falling
                            case 1:

                                if (ExistValueAround && (SampleToPlot[CenterValue + 1] < sample1))
                                {
                                    ss = "Falling mode is selected in the combo box " + Environment.NewLine;
                                    // Gör falling
                                    PlottingMethod(TimeToPlot, SampleToPlot, time1, sample1);
                                }

                                break;
                        }


                    }

                }
                // sleep
                Thread.Sleep(500);
            }

        }


        /// <summary>
        /// Single Trigger:
        /// Finds a close value to that triggered value and show it plot
        /// 
        /// How it is done:
        /// Searching for the position of the element in the streaming values.
        /// Searching for the value itself and pass it to plottingMethod.
        /// </summary>
        private void SingleTrigger(List<double> time, List<double> sample, double f, params object[] restOfArguments)
        {
            if (runSingle)
            {
                // Finding the indeces of the element that trigger is sett to
                for (int i = 0; i < sample.Count; i++)
                {
                    if (sample[i] == f || (sample[i] <= f + Error && sample[i] >= f - Error))
                    {
                        Indeces.Add(i);
                    }
                }

                // count the number of element in in updatedSampleList
                var countValue = time.Count;
                double time1;
                double sample1;

                if (countValue < 41)
                {
                    // Plot all value
                }
                else
                {
                    SampleToPlot.Clear();
                    TimeToPlot.Clear();

                    if (Indeces[0] > 5 && Indeces[0] < time.Count - 5)
                    {
                        // In normal condition when the value has 20 elements before
                        // and after
                        for (int d = Indeces[0] - 5; d < Indeces[0] + 5; d++)
                        {
                            SampleToPlot.Add(sample[d]);
                            TimeToPlot.Add(time[d]);
                        }

                    }

                    else if (Indeces[0] < 5 && Indeces[0] < time.Count - 5)
                    {
                        // When the value has less elements before it
                        for (int d = Indeces[0]; d < Indeces[0] + 5; d++)
                        {
                            SampleToPlot.Add(sample[d]);
                            TimeToPlot.Add(time[d]);
                        }
                    }
                    else if (Indeces[0] > 5 && Indeces[0] > time.Count - 5)
                    {
                        // then value doesnt have enough elements after
                        for (int d = Indeces[0] - 5; d < time.Count; d++)
                        {
                            SampleToPlot.Add(sample[d]);
                            TimeToPlot.Add(time[d]);
                        }
                    }



                    time1 = time[Indeces[0]];
                    sample1 = sample[Indeces[0]];

                    CenterValue = SampleToPlot.FindIndex(item => item == sample1);

                    switch (GetIndexComBo())
                    {
                        // No direction
                        case -1:
                            PlottingMethod(TimeToPlot, SampleToPlot, time1, sample1);
                            break;

                        // Rising
                        case 0:
                            if (SampleToPlot[CenterValue + 1] > sample1)
                            {
                                // Gör rising
                                PlottingMethod(TimeToPlot, SampleToPlot, time1, sample1);
                            }
                            else
                            {

                            }
                            break;

                        // Falling
                        case 1:

                            if (SampleToPlot[CenterValue + 1] < sample1)
                            {
                                // Gör falling
                                PlottingMethod(TimeToPlot, SampleToPlot, time1, sample1);
                            }
                            else
                            {

                            }

                            break;
                    }

                    // Send data to be plotted
                    PlottingMethod(TimeToPlot, SampleToPlot, time1, sample1);

                    // sleep
                    //Thread.Sleep(500);

                }
                runSingle = false;



            }

        }
        #endregion

        #region UI Events
        /// <summary>
        /// run is a boolean to extinguish between stop and start streaming.
        /// A thread is created to not block the UI. if the radiobutton is checked
        /// and the conversion of the text box content is succeful then it runs the 
        /// streaming. if not a messege box is showed.
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
                if (rbRepeat.Checked)
                {
                    if (ValidateTheTextBox())
                    {

                        RunStuffOrNot = 1;
                        thread = new Thread(new ThreadStart(RunStreaming));
                        ss = "Thread started for Repeat Trigger" + Environment.NewLine;
                        InsertText(ss);
                        runRepeat = false;
                        thread.Start();
                    }
                    else
                    {
                        MessageBox.Show("You have to insert a proper value");
                    }
                }
                else if (rbNone.Checked)
                {
                    RunStuffOrNot = 1;
                    thread = new Thread(new ThreadStart(RunStreaming));
                    ss = "Thread started for None Trigger" + Environment.NewLine;
                    InsertText(ss);
                    thread.Start();

                }
                else if (rbSingle.Checked)
                {
                    if (ValidateTheTextBox())
                    {
                        RunStuffOrNot = 1;
                        thread = new Thread(new ThreadStart(RunStreaming));
                        runSingle = true;
                        ss = "Thread started for Single Trigger" + Environment.NewLine;
                        InsertText(ss);
                        thread.Start();
                    }
                    else
                    {
                        MessageBox.Show("You have to insert a proper value");
                    }
                }
                else
                {
                    RunStuffOrNot = 1;
                    thread = new Thread(new ThreadStart(RunStreaming));
                    ss = "Thread started for None Trigger" + Environment.NewLine;
                    InsertText(ss);
                    thread.Start();
                }


                btnStream.Text = "Stop Streaming";
                ss = "Thread started" + Environment.NewLine;
                InsertText(ss);
                run = true;
            }
            else
            {
                ss = "Aborting thread. A temporary solution" + Environment.NewLine;
                InsertText(ss);
                RunStuffOrNot = 0;
                ss = "Close and Stop done" + Environment.NewLine;
                btnStream.Text = "Start steaming";
                thread = null;
                InsertText(ss);
                run = false;
                runSingle = true;
                runRepeat = true;

            }
        }


        private void rbNone_CheckedChanged(object sender, EventArgs e)
        {
            txtError.ReadOnly = txtTriggerAT.ReadOnly = true;
        }

        private void rbRepeat_CheckedChanged(object sender, EventArgs e)
        {
            txtError.ReadOnly = txtTriggerAT.ReadOnly = false;
        }

        private void rbSingle_CheckedChanged(object sender, EventArgs e)
        {
            txtError.ReadOnly = txtTriggerAT.ReadOnly = false;
        }

        #endregion

        #region Validating Indata and Update Unrelated UI

        /// <summary>
        /// Checks if the conversion to double is succefull.
        /// The value of Error is set. If the sampling frequency 
        /// is not small enough it would be unefficient to find value
        /// in the textbox. If the Textbox is empty the trigger is set
        /// at zero.
        /// </summary>
        /// <returns></returns>
        private bool ValidateTheTextBox()
        {
            bool valid = true;
            var control2 = int.TryParse(txtTriggerAT.Text, out triggerAtNumber);
            var control3 = int.TryParse(txtError.Text, out Error);
            if (string.IsNullOrWhiteSpace(txtTriggerAT.Text) || control2 == false || control3 == false)
            {
                triggerAtNumber = 0;
                Error = 0;
                //txtStatus.AppendText("No trigger set or not possible to parse to double. Sets the trigger at 0" + Environment.NewLine);
                valid = false;
            }

            return valid;
        }

        /// <summary>
        /// Invoking the textbox containing the status
        /// </summary>
        /// <param name="ss"></param>
        private void InsertText(string ss)
        {
            if (txtStatus.InvokeRequired)
            {
                txtStatus.Invoke((MethodInvoker)delegate ()
                {
                    InsertText(ss);
                });
            }
            else
            {
                txtStatus.AppendText(ss);
            }
        }

        /// <summary>
        /// Getting the index of selected item in the combo box for directions
        /// </summary>
        /// <returns></returns>
        public int GetIndexComBo()
        {
            int index = -1;
            if (cbDirection.InvokeRequired)
            {
                index = (int)cbDirection.Invoke(new Func<int>(() => cbDirection.SelectedIndex));
            }
            else
            {
                index = cbDirection.SelectedIndex;
            }
            return index;
        }

        /// <summary>
        /// Checks if the checkbox is activated
        /// </summary>
        /// <returns></returns>
        public bool CheckBoxValidation()
        {
            bool index = false;
            if (chbDetaljs.InvokeRequired)
            {
                index = (bool)chbDetaljs.Invoke(new Func<bool>(() => chbDetaljs.Checked));
            }
            else
            {
                index = chbDetaljs.Checked;
            }
            return index;
        }

        #endregion

        private void btnDrop_Click(object sender, EventArgs e)
        {
            Imports.CloseUnit(_handle);
            this.Close();
        }
    }
}

