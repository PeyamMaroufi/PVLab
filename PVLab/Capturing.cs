// Copyright (c) 2018 Payam 
// Made by Payam M. 
// Purpose: Measuring the voltage of high voltage devices scaled down to almost 30 V
// Workes for continues streaming and triggers. Includes repeat trigger and single trigger. and capturing.
// Target framwork is .NET and WINFORM. 
// 

using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.WindowsForms;
using PicoPinnedArray;
using PicoStatus;
using PS5000AImports;
using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PVLab
{
    class Capturing : SettingUpBlockRecordAndStream
    {
        public Capturing()
        {

        }

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


        public void RunBlock()
        {
            uint status;
            // determine maximum value for scaling
            status = Imports.MaximumValue(StaticVariable._handle, out _maxValue);


            // Setting channel. Is needed for everytime you want to do something. eg when you change the voltage range, resulotion.

            status = Imports.SetChannel(StaticVariable._handle, StaticVariable.SelChannel, 1, StaticVariable.SelCoup, StaticVariable.SelVolt, 0);



            // SetsimplerTrigger function
            short enable = 0;
            uint delay = 0;
            short threshold = 25000;
            short auto = 0;
            status = Imports.SetSimpleTrigger(StaticVariable._handle, enable, StaticVariable.SelChannel, threshold, Imports.ThresholdDirection.Rising, delay, auto);


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
            status = Imports.SetDataBuffers(StaticVariable._handle, StaticVariable.SelChannel, maxBuffers, minBuffers, (int)sampleCount, 0, Imports.RatioMode.None);



            /*  find the maximum number of samples, the time interval (in timeUnits),
             *		 the most suitable time units, and the maximum _oversample at the current _timebase*/
            int timeInterval;
            int maxSamples;

            TimeBaseCalc.TimeBase(StaticVariable.SampleInterval);

            while (Imports.GetTimebase(StaticVariable._handle, _timebase, (int)sampleCount, out timeInterval, out maxSamples, 0) != 0)
            {
                // This will try to come to our chosen timebase as close as possible.
                _timebase++;

            }

            StaticVariable.TimeBase = _timebase;

            /* Start it collecting, then wait for completion*/
            _ready = false;
            _callbackDelegate = BlockCallback;


            do
            {
                retry = false;
                status = Imports.RunBlock(StaticVariable._handle, 0, (int)sampleCount, _timebase, out timeIndisposed, 0, _callbackDelegate, IntPtr.Zero);

                if (status == (short)StatusCodes.PICO_POWER_SUPPLY_CONNECTED || status == (short)StatusCodes.PICO_POWER_SUPPLY_NOT_CONNECTED || status == (short)StatusCodes.PICO_POWER_SUPPLY_UNDERVOLTAGE)
                {
                    status = Imports.ChangePowerSource(StaticVariable._handle, status);
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

            Imports.Stop(StaticVariable._handle);



            if (_ready)
            {
                short overflow;

                // Now get the values 
                status = Imports.GetValues(StaticVariable._handle, 0, ref sampleCount, 1, Imports.DownSamplingMode.None, 0, out overflow);
                minPinned[0] = new PinnedArray<short>(minBuffers);
                maxPinned[0] = new PinnedArray<short>(maxBuffers);
                samples = new double[sampleCount];
                sampleTime = new double[sampleCount];

                if (status == (short)StatusCodes.PICO_OK)
                {

                    // Scale the data with respect to the maximum value.
                    for (int x = 0; x < sampleCount; x++)
                    {
                        samples[x] = Scaling.adc_to_mv(maxPinned[0].Target[x], StaticVariable.SelRangeIndex, _maxValue);
                    }

                    // Make the time to ms
                    for (int i = 0; i < sampleCount; i++)
                    {
                        sampleTime[i] = (double)(i * StaticVariable.SampleInterval) / 1000000;
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

            Imports.Stop(StaticVariable._handle);

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

        #region Plot Function for Capturing
        /// <summary>
        ///  Calls Plot form and passes data to it to make a graph
        /// </summary>
        void DrawingChart(double[] data, double[] time)
        {

            Form form = new Form();
            PlotView oxyPlot = new PlotView();
            PlotModel CaptureModel = new PlotModel { Title = "Voltage level" };
            LinearAxis linearAxis1 = new LinearAxis { Position = AxisPosition.Bottom, Title = "Time in nanosec" };
            LinearAxis linearAxis2 = new LinearAxis { Position = AxisPosition.Left, Title = "Voltage" };
            CaptureModel.Axes.Add(linearAxis1);
            CaptureModel.Axes.Add(linearAxis2);

            LineSeries serieCapture = new LineSeries
            {
                MarkerType = MarkerType.Circle,
                StrokeThickness = 1,
                MarkerSize = 1,
                Smooth = true,
                Title = "Voltage level"


            };

            oxyPlot.Dock = DockStyle.Fill;

            for (int i = 0; i < data.Length; i++)
            {
                serieCapture.Points.Add(new DataPoint(time[i], data[i]));
            }


            CaptureModel.Series.Add(serieCapture);
            oxyPlot.Model = CaptureModel;
            form.Controls.Add(oxyPlot);
            form.Show();

            //int rangePlot = StaticVariable.SelChannelIndex;
            //if (samples != null)
            //{
            //    plot.Samples = samples;
            //    plot.SampleTime = sampleTime;

            //    plot.Draw(rangePlot);
            //    plot.Show();


        }


        #endregion
    }
}
