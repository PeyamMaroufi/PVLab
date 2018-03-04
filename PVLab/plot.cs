// Copyright (c) 2018 Payam 
// Made by Payam M. 
// Purpose: Measuring the voltage of high voltage devices scaled down to almost 30 V
// Workes for continues streaming and triggers. Includes repeat trigger and single trigger. and capturing.
// Target framwork is .NET and WINFORM. 
// 

using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using PicoPinnedArray;
using PicoStatus;
using PS5000AImports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Timers;

namespace PVLab
{
    public partial class Plot : Form
    {

        #region Local variables

        // PLOTT VARIABLER
        LineSeries series1;
        LineSeries series2;
        PlotModel myModel;
        LinearAxis linearAxis1;
        LinearAxis linearAxis2;
        bool sendit = false;
        List<double> TimeToPlot;
        List<double> SampleToPlot;
        List<int> Indeces;
        double timeAtIndex;
        double sampleAtIndex;
        double divValue;
        double maxValueOfDiv;

        // STREAMING METHOD VARIABLES
        Streaming streaming;
        bool run = false;
        double NumberOfSamples;
        double SampleInterval;

        // UI VARIABLES
        DictionaryValues dictionaryValues;
        Dictionary<double, string> timeBaseDict;
        public static string ss;
        Thread thread;
        int valueIndex;
        double timeInDiv;
        bool openClose = false;
        #endregion

        #region Construction and UI initialization
        /// <summary>
        /// Construction.
        /// </summary>
        /// <param name="handle"></param>
        public Plot()
        {
            InitializeComponent();

            SetupEverythingUI();

            // To stop streaming without droppping the device
            run = false;

            // Calculate the value of fre
            Frequency();

        }

        /// <summary>
        /// Set up the UI and creates a new instance of streaming class, lists.
        /// </summary>
        public void SetupEverythingUI()
        {

            ss = "Plot form is created . Construction is called" + Environment.NewLine;
            InsertText(ss);

            // fill the combo box
            cbDirection.Items.Add("Rising");
            cbDirection.Items.Add("Falling");

            // list to store the sampled data
            TimeToPlot = new List<double>();
            SampleToPlot = new List<double>();

            //track number of indeces in repeat
            Indeces = new List<int>();

            // check the default radio button
            rbNone.Checked = true;

            // Streaming class
            streaming = new Streaming();

            // Div Values
            dictionaryValues = new DictionaryValues();
            timeBaseDict = dictionaryValues.dictionary;
            CbDivs.DataSource = new BindingSource(timeBaseDict, null);
            CbDivs.DisplayMember = "Value";
            CbDivs.ValueMember = "Key";
            CbDivs.SelectedIndex = 4;

            // Selected value
            NumberOfSampleUpAndDown.Value = 1000;

            // Number Of Samples
            NumberOfSamples = decimal.ToDouble(NumberOfSampleUpAndDown.Value);


        }

        #endregion

        #region Frequency Calculations
        /// <summary>
        /// is needed for updating the current sampling interval and timebase.
        /// </summary>
        public void Frequency()
        {
            // to emitate pico 6
            int NumberOfDivs = 10;
            // Find the index of selected value in combo box
            valueIndex = CbDivs.SelectedIndex;
            // find the corresponding value in seconds
            double valueInSec = dictionaryValues.dictionaryInSec[valueIndex];

            // To emitate Pico sex and choose sample for 10 time divisions
            SampleInterval = (NumberOfDivs * valueInSec) / NumberOfSamples;

            // Find frequency for given div and intervall
            double freq = 1 / SampleInterval;

            // set the sampleinterval to the static variable. sampling is in nanosec
            StaticVariable.SampleInterval = SampleInterval * Math.Pow(10, 9);

            // set number of samples defined by user
            StaticVariable.NumberOfSamples = NumberOfSamples;

            // set name of selected division to use in chart. is string
            int index = CbDivs.SelectedIndex;
            StaticVariable.SelectedDiv = dictionaryValues.dictionary.ElementAt(index).Value;
            StaticVariable.DivValueLimit = dictionaryValues.dictionaryValueDiv.ElementAt(index);

            // show values in textbox
            UpdateSignalProperties(NumberOfSamples, freq, SampleInterval);

            // Set the value of Div
            StaticVariable.DivValue = dictionaryValues.DivValueLimit.ElementAt(valueIndex);

        }



        #endregion

        #region Plotting
        /// <summary>
        /// gets value from streaming and triggers and creates the 
        /// plot. this is called only once in a streaming and triggering.
        /// </summary>
        private void InitiateStreamPlot(List<double> time, List<double> sample, params object[] restOfPointsToPlote)
        {

            // Create a new instance of plotView
            myModel = new PlotModel { Title = "Voltage level" };
            ss = "New streaming plot is starting" + Environment.NewLine;

            // series of points
            series1 = new LineSeries
            {
                MarkerType = MarkerType.Circle,
                StrokeThickness = 1,
                MarkerSize = 1,
                Smooth = true,
                Title = "Voltage level",
                CanTrackerInterpolatePoints = false,

            };

            // series of point used in single and repeat
            series2 = new LineSeries
            {
                Color = OxyColors.Red,
                MarkerFill = OxyColors.Blue,
                MarkerStroke = OxyColors.Red,
                MarkerType = MarkerType.Circle,
                StrokeThickness = 0,
                MarkerSize = 4,
            };

            // Track the key corresponding the selected value in combo box


            // Define the maximum number of divisions. set to 10
            maxValueOfDiv = 10 * dictionaryValues.dictionary.ElementAt(valueIndex).Key;

            // Define axis with limits
            linearAxis1 = new LinearAxis { Position = AxisPosition.Bottom };
            linearAxis2 = new LinearAxis { Position = AxisPosition.Left, Title = "Voltage", Minimum = -1 * StaticVariable.voltages[StaticVariable.SelRangeIndex], Maximum = StaticVariable.voltages[StaticVariable.SelRangeIndex] };

            // Add it to the model
            myModel.Axes.Add(linearAxis1);
            myModel.Axes.Add(linearAxis2);


            for (int i = 0; i < time.Count - 1; i++)
            {
                //timeInDiv = time[i] / divValue;
                series1.Points.Add(new OxyPlot.DataPoint(time[i], sample[i]));
            }

            // Merge the serie to the model
            myModel.Series.Add(series1);

            // Show the triggered value on the plot
            if (restOfPointsToPlote.Length > 0)
            {
                // Finding the index at which trigger occures
                timeAtIndex = (double)restOfPointsToPlote[0];

                // find the index of the voltage at which trigger occures
                sampleAtIndex = (double)restOfPointsToPlote[1];

                // plot that value
                series2.Points.Add(new OxyPlot.DataPoint(timeAtIndex, sampleAtIndex));

                // add serie to the model
                myModel.Series.Add(series2);
            }

            // Merge the model to the plotView
            plotView1.Model = myModel;
            // in order to avoid creating plotModel again

            sendit = true;
        }

        /// <summary>
        /// Updating the chart.Invoke is required for not blocking UI thread.
        /// Reciving the optional data when trigger is on.
        /// </summary>
        public void UpdatePlot(List<double> time, List<double> sample, params object[] restOfPointsToUdate)
        {
            // do invoking recursively
            if (plotView1.InvokeRequired)
            {
                plotView1.Invoke((MethodInvoker)delegate ()
                {
                    UpdatePlot(time, sample, restOfPointsToUdate);

                });

            }
            else
            {
                // Clearing earlier points in series2
                if (series2.Points.Count > 0)
                    series2.Points.Clear();

                // Clearing earlier points in series2
                if (series1.Points.Count > 0)
                    series1.Points.Clear();

                // Clear the series
                if (myModel.Series.Count > 0)
                    myModel.Series.Clear();


                //string num = series1.Points.Count.ToString();



                // Adding new values
                for (int i = 0; i < time.Count - 1; i++)
                {
                    //timeInDiv = time[i] / divValue;
                    series1.Points.Add(new OxyPlot.DataPoint(time[i], sample[i]));
                }

                // Add to the plot
                myModel.Series.Add(series1);

                if (restOfPointsToUdate.Length > 0)
                {
                    timeAtIndex = (double)restOfPointsToUdate[0];
                    sampleAtIndex = (double)restOfPointsToUdate[1];
                    series2.Points.Add(new OxyPlot.DataPoint(timeAtIndex, sampleAtIndex));
                    myModel.Series.Add(series2);
                }

                // Update
                //myModel.InvalidatePlot(true);
                plotView1.InvalidatePlot(true);
                //myModel.PlotView.InvalidatePlot(true);
            }
        }

        /// <summary>
        /// Help function for plotting. restOfPoints is an optional parameter
        /// that can be used when triggered to avoid overloading.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="sample"></param>
        /// <param name="restOfPoints"></param>
        public void PlottingMethod(List<double> time, List<double> sample, params object[] restOfPoints)
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

        #region UI Events
        /// <summary>
        /// run is a boolean to extinguish between stop and start streaming.
        /// A thread is created to not block the UI. if the radiobutton is checked
        /// and the conversion of the text box content is succeful then it runs the 
        /// streaming. if not a messege box is showed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnStream_Click(object sender, EventArgs e)
        {
            ss = "Start Streaming button is clicked" + Environment.NewLine;
            InsertText(ss);
            if (!run)
            {
                // Update with new sampling interval
                Frequency();
                ss = "Thread was null and therefor gets created" + Environment.NewLine;
                InsertText(ss);

                // nasted ifs
                if (rbRepeat.Checked)
                {
                    if (ValidateTheTextBox())
                    {

                        // to enter while loop in streaming class
                        StaticVariable.RunStuffOrNot = true;

                        // Do stuff in secondary thread
                        thread = new Thread(new ThreadStart(streaming.RunStreaming));
                        ss = "Thread started for Repeat Trigger" + Environment.NewLine;
                        InsertText(ss);

                        // Trigger repeat
                        StaticVariable.RunRepeat = false;
                        StaticVariable.ISaidRun = true;
                        // start the thread
                        thread.Start();
                    }
                    else
                    {
                        MessageBox.Show("You have to insert a proper value");
                    }
                }
                else if (rbNone.Checked)
                {
                    StaticVariable.RunStuffOrNot = true;
                    thread = new Thread(new ThreadStart(streaming.RunStreaming));
                    ss = "Thread started for None Trigger" + Environment.NewLine;
                    InsertText(ss);
                    thread.Start();
                    StaticVariable.ISaidRun = true;

                }
                else if (rbSingle.Checked)
                {
                    if (ValidateTheTextBox())
                    {
                        StaticVariable.RunStuffOrNot = true;
                        thread = new Thread(new ThreadStart(streaming.RunStreaming));
                        StaticVariable.RunSingle = true;
                        ss = "Thread started for Single Trigger" + Environment.NewLine;
                        InsertText(ss);
                        StaticVariable.ISaidRun = true;
                        thread.Start();
                    }
                    else
                    {
                        MessageBox.Show("You have to insert a proper value");
                    }
                }
                else
                {
                    StaticVariable.RunStuffOrNot = true;
                    thread = new Thread(new ThreadStart(streaming.RunStreaming));
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
                StaticVariable.RunStuffOrNot = false;
                ss = "Close and Stop done" + Environment.NewLine;
                btnStream.Text = "Start steaming";
                thread = null;
                InsertText(ss);
                run = false;
                StaticVariable.RunSingle = true;
                StaticVariable.RunRepeat = true;
                StaticVariable.ISaidRun = false;

            }
        }

        private void rbNone_CheckedChanged(object sender, EventArgs e)
        {
            txtError.ReadOnly = txtTriggerAT.ReadOnly = true;
            StaticVariable.CheckRbs = 0;
        }

        private void rbRepeat_CheckedChanged(object sender, EventArgs e)
        {
            txtError.ReadOnly = txtTriggerAT.ReadOnly = false;
            StaticVariable.CheckRbs = 1;
        }

        private void rbSingle_CheckedChanged(object sender, EventArgs e)
        {
            txtError.ReadOnly = txtTriggerAT.ReadOnly = false;
            StaticVariable.CheckRbs = 2;
        }

        private void NumberOfSampleUpAndDown_ValueChanged(object sender, EventArgs e)
        {
            // update sampling intervall and number of points
            NumberOfSamples = decimal.ToDouble(NumberOfSampleUpAndDown.Value);
            Frequency();
        }

        private void cbTimeBase_SelectedIndexChanged(object sender, EventArgs e)
        {
            // update sampling intervall and number of points.
            Frequency();
        }

        private void btnDrop_Click(object sender, EventArgs e)
        {

            // call CloseUnit method in second base class
            Imports.CloseUnit(StaticVariable._handle);
            this.Close();

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
            int TriggerAt;
            var control2 = int.TryParse(txtTriggerAT.Text, out TriggerAt);
            StaticVariable.TriggerAt = TriggerAt;

            int Error;
            var control3 = int.TryParse(txtError.Text, out Error);
            StaticVariable.Error = Error;

            if (string.IsNullOrWhiteSpace(txtTriggerAT.Text) || control2 == false || control3 == false)
            {
                StaticVariable.TriggerAt = 0;
                StaticVariable.Error = 0;
                //txtStatus.AppendText("No trigger set or not possible to parse to double. Sets the trigger at 0" + Environment.NewLine);
                valid = false;
            }

            return valid;
        }

        /// <summary>
        /// Invoking the textbox containing the status
        /// </summary>
        /// <param name="ss"></param>
        public void InsertText(string ss)
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

            if (cbDirection.InvokeRequired)
            {
                StaticVariable.RaisingFallingIndex = (int)cbDirection.Invoke(new Func<int>(() => cbDirection.SelectedIndex));
            }
            else
            {
                StaticVariable.RaisingFallingIndex = cbDirection.SelectedIndex;
            }
            return StaticVariable.RaisingFallingIndex;

        }

        /// <summary>
        /// Checks if the checkbox is activated
        /// </summary>
        /// <returns></returns>
        public bool CheckBoxValidation()
        {
            //bool index = false;
            if (chbDetaljs.InvokeRequired)
            {
                StaticVariable.CheckBoxStatus = (bool)chbDetaljs.Invoke(new Func<bool>(() => chbDetaljs.Checked));
            }
            else
            {
                StaticVariable.CheckBoxStatus = chbDetaljs.Checked;
            }
            return StaticVariable.CheckBoxStatus;
        }

        /// <summary>
        /// Updates signal properties
        /// </summary>
        private void UpdateSignalProperties(double points, double freq, double intervall)
        {
            if (txtSignal.InvokeRequired)
                txtSignal.Invoke((MethodInvoker)delegate ()
                {
                    UpdateSignalProperties(points, freq, intervall);
                });
            else
            {
                txtSignal.AppendText(" Number of points: " + points.ToString() + Environment.NewLine);
                txtSignal.AppendText(" Frequency: " + freq.ToString() + Environment.NewLine);
                txtSignal.AppendText(" Interval: " + intervall.ToString() + Environment.NewLine);


            }
        }

        /// <summary>
        /// Updates the number of samples done in streaming.
        /// </summary>
        /// <param name="points"></param>
        public void NumberOFPoints(double points)
        {
            if (lblPoints.InvokeRequired)
                lblPoints.Invoke((MethodInvoker)delegate ()
                {
                    NumberOFPoints(points);
                });
            else
            {
                lblPoints.Text = " Number of points: " + points.ToString();



            }
        }

        #endregion

        #region Form load event handler
        private void Plot_Load(object sender, EventArgs e)
        {
            // make this form static to access the UI event handlers
            StaticVariable.form = this;
        }
        #endregion

        private void cbDirection_SelectedIndexChanged(object sender, EventArgs e)
        {
            StaticVariable.RaisingFallingIndex = cbDirection.SelectedIndex;
        }
    }
}

