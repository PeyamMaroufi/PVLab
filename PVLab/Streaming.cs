using PicoPinnedArray;
using PS5000AImports;
using System;
using OxyPlot.WindowsForms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OxyPlot;


namespace PVLab
{
    public class Streaming : SettingUpBlockRecordAndStream
    {
        #region Local variables
        Thread thread1;
        bool run = false;
        double iteration = 0;
        uint sampleInterval;
        double produkt;
        Thread thread;
        List<double> TimeToPlot;
        List<double> SampleToPlot;
        List<int> Indeces;
        public List<int> sampleCountList;
        List<double> updateTimeList;
        List<double> UpdatedSampleList;
        short[][] appBuffers;
        short[][] buffers;
        string ss;
        PlotView ploView;
        PlotModel plotModel;
        public short _handle;
        int _channelCount = 1;

        int CenterValue;
        private static System.Timers.Timer aTimer;


        #endregion

        #region Call Stream

        public Streaming()
        {
            sampleCountList = new List<int>();
            updateTimeList = new List<double>();
            UpdatedSampleList = new List<double>();
            TimeToPlot = new List<double>();
            SampleToPlot = new List<double>();
            Indeces = new List<int>();
            _handle = StaticVariable._handle;
            aTimer = new System.Timers.Timer();
            aTimer.Interval = 500;
            aTimer.AutoReset = true;
            aTimer.Elapsed += ATimer_Elapsed;


        }

        private void ATimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // Calling the plotting Method for unconstraint streaming
            if (StaticVariable.CheckRbs == 0)
            {
                StaticVariable.form.PlottingMethod(updateTimeList, UpdatedSampleList);
            }
            //else if (StaticVariable.CheckRbs == 1 && !StaticVariable.RunRepeat)
            //{
            //    // No direction specified
            //    RepeatTrigger(updateTimeList, UpdatedSampleList, StaticVariable.TriggerAt);
            //}
            //else if (StaticVariable.CheckRbs == 2 && StaticVariable.RunSingle)
            //{
            //    // Call singleTrigger without any direction specified.
            //    SingleTrigger(updateTimeList, UpdatedSampleList, StaticVariable.TriggerAt);
            //}
        }

        public void StartTimer(bool dec)
        {
            if (dec)
            {
                aTimer.Enabled = false;
            }
            else
            {
                aTimer.Enabled = false;
            }
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
            while (StaticVariable.ISaidRun)
            {
                uint status;
                uint preTrigger = 0;
                int totalSamples = 0;
                uint triggeredAt = 0;


                if (appBuffers != null)
                {
                    appBuffers = null;
                    buffers = null;
                }
                //if (aTimer.Enabled)
                //{
                //    aTimer.Enabled = false;
                //}

                MaxValue();
                SetChannel();
                int sampleCount = 1024 * 100;  /*  *100 is to make sure buffer large enough */

                appBuffers = new short[_channelCount * 2][];
                buffers = new short[_channelCount * 2][];


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

                // Time need to be in nanosec
                sampleInterval = (uint)StaticVariable.SampleInterval;

                // Run streaming with those values
                status = Imports.RunStreaming(_handle, ref sampleInterval, Imports.ReportedTimeUnits.NanoSeconds, preTrigger, 1000000 - preTrigger, 1, 1, Imports.RatioMode.None, (uint)sampleCount);

                //aTimer.Enabled = true;


                while (!_autoStop && StaticVariable.RunStuffOrNot)
                {
                    /* Poll until data is received. Until then, GetStreamingLatestValues wont call the callback */
                    Thread.Sleep(0);
                    _ready = false;
                    status = Imports.GetStreamingLatestValues(_handle, StreamingCallback, IntPtr.Zero);

                    if (_ready && _sampleCount > 0) /* can be ready and have no data, if autoStop has fired */
                    {
                        iteration += 1;
                        //ss = "New Iteration " + iteration + Environment.NewLine;
                        //InsertText(ss);

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

                        for (uint i = _startIndex; i < (_startIndex + _sampleCount); i++)
                        {

                            // Insert samples in a list
                            UpdatedSampleList.Add(adc_to_mv(appBuffersPinned[0].Target[i], StaticVariable.SelRangeIndex, _maxValue));


                            // Insert time in a list
                            //updateTimeList.Add(i * sampleInterval);

                        }
                        for (uint i = 0; i < _sampleCount; i++)
                        {


                            updateTimeList.Add(i * sampleInterval / StaticVariable.DivValue);
                        }

                        var index = updateTimeList.FindIndex(item => item > StaticVariable.DivValueLimit);
                        if (index != -1)
                        {
                            int resul = UpdatedSampleList.Count - index - 1;
                            updateTimeList.RemoveRange(index, updateTimeList.Count - index - 1);
                            UpdatedSampleList.RemoveRange(index, UpdatedSampleList.Count - index - 1);
                        }
                        if (UpdatedSampleList.Count < 10)
                            continue;

                        var d = updateTimeList.Last();

                        if (StaticVariable.CheckRbs == 0)
                        {
                            StaticVariable.form.PlottingMethod(updateTimeList, UpdatedSampleList);
                        }

                        else if (StaticVariable.CheckRbs == 1 && !StaticVariable.RunRepeat)
                        {
                            // No direction specified
                            RepeatTrigger(updateTimeList, UpdatedSampleList, StaticVariable.TriggerAt);
                        }
                        else if (StaticVariable.CheckRbs == 2 && StaticVariable.RunSingle)
                        {
                            // Call singleTrigger without any direction specified.
                            SingleTrigger(updateTimeList, UpdatedSampleList, StaticVariable.TriggerAt);


                        }

                    }
                }

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
                StaticVariable.form.InsertText(ss);
                Indeces.Clear();
            }

            ss = "Search for indeces that corrospond to the triggered value that are between  " + (StaticVariable.Error + f) + " and " + (f - StaticVariable.Error) + Environment.NewLine;
            StaticVariable.form.InsertText(ss);

            // Finding the indeces of the element that trigger is sett to
            for (int i = 0; i < sample.Count; i++)
            {
                if (sample[i] == f || (sample[i] <= f + StaticVariable.Error && sample[i] >= f - StaticVariable.Error))
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
            StaticVariable.form.InsertText(ss);

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
                    if (!StaticVariable.RunRepeat)
                    {
                        SampleToPlot.Clear();
                        TimeToPlot.Clear();

                        // Checks different situation of trigging
                        bool isThereValueAround = (Indeces[i] > 5 && Indeces[i] < time.Count - 5) ? true : false;
                        bool isNoValueBefore = ((Indeces[i] < 5 && Indeces[i] < time.Count - 5) && StaticVariable.form.CheckBoxValidation()) ? true : false;
                        bool isNoValueAfter = ((Indeces[i] > 5 && Indeces[i] > time.Count - 5) && StaticVariable.form.CheckBoxValidation()) ? true : false;

                        if (isThereValueAround)
                        {
                            // Refer to the time intervall
                            double repeatIndex = StaticVariable.SampleInterval;

                            // In normal condition when the value has 20 elements before
                            // and after.
                            ss = "There are more than 5 values after and before the triggered value in this iteration " + Environment.NewLine;
                            StaticVariable.form.InsertText(ss);
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
                            double repeatIndex = StaticVariable.SampleInterval;

                            // When the value has less elements before it
                            ss = "There are less than 5 values  before the triggered value in this iteration " + Environment.NewLine;
                            StaticVariable.form.InsertText(ss);
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
                            double repeatIndex = StaticVariable.SampleInterval;

                            // then value doesnt have enough elements after
                            ss = "There are less than 5 values after the triggered value in this iteration " + Environment.NewLine;
                            StaticVariable.form.InsertText(ss);
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
                        StaticVariable.form.InsertText(ss);

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


                        switch (StaticVariable.form.GetIndexComBo())
                        {
                            // No direction
                            case -1:
                                ss = "No item is selected in the combo box " + Environment.NewLine;
                                StaticVariable.form.InsertText(ss);

                                // Show
                                StaticVariable.form.PlottingMethod(TimeToPlot, SampleToPlot, time1, sample1);
                                break;

                            // Rising
                            case 0:
                                if (ExistValueAround && (SampleToPlot[CenterValue + 1] > sample1))
                                {

                                    ss = "Rising mode is selected in the combo box" + Environment.NewLine;
                                    // Gör rising
                                    StaticVariable.form.PlottingMethod(TimeToPlot, SampleToPlot, time1, sample1);
                                }
                                break;

                            // Falling
                            case 1:

                                if (ExistValueAround && (SampleToPlot[CenterValue + 1] < sample1))
                                {
                                    ss = "Falling mode is selected in the combo box " + Environment.NewLine;
                                    // Gör falling
                                    StaticVariable.form.PlottingMethod(TimeToPlot, SampleToPlot, time1, sample1);
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
            if (StaticVariable.RunSingle)
            {
                // Finding the indeces of the element that trigger is sett to
                for (int i = 0; i < sample.Count; i++)
                {
                    if (sample[i] == f || (sample[i] <= f + StaticVariable.Error && sample[i] >= f - StaticVariable.Error))
                    {
                        Indeces.Add(i);
                    }
                }
                if (Indeces.Count > 1)
                {

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

                        // Checks different situation of trigging
                        bool isThereValueAround = (Indeces[0] > 5 && Indeces[0] < time.Count - 5) ? true : false;
                        bool isNoValueBefore = ((Indeces[0] < 5 && Indeces[0] < time.Count - 5) && StaticVariable.form.CheckBoxValidation()) ? true : false;
                        bool isNoValueAfter = ((Indeces[0] > 5 && Indeces[0] > time.Count - 5) && StaticVariable.form.CheckBoxValidation()) ? true : false;

                        if (isThereValueAround)
                        {
                            // Refer to the time intervall
                            double repeatIndex = StaticVariable.SampleInterval;

                            // In normal condition when the value has 20 elements before
                            // and after.
                            ss = "There are more than 5 values after and before the triggered value in this iteration " + Environment.NewLine;
                            StaticVariable.form.InsertText(ss);
                            for (int d = Indeces[0] - 5; d < Indeces[0] + 5; d++)
                            {
                                SampleToPlot.Add(sample[d]);
                            }

                            // Set negative side of time and zero time point
                            for (int d = Indeces[0]; d >= Indeces[0] - 5; d--)
                            {
                                if (d == Indeces[0])
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
                            for (int d = Indeces[0] + 1; d <= Indeces[0] + 5; d++)
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
                            double repeatIndex = StaticVariable.SampleInterval;

                            // When the value has less elements before it
                            ss = "There are less than 5 values  before the triggered value in this iteration " + Environment.NewLine;
                            StaticVariable.form.InsertText(ss);
                            for (int d = Indeces[0]; d < Indeces[0] + 5; d++)
                            {
                                SampleToPlot.Add(sample[d]);
                            }

                            // Set negative side of time and zero time point
                            for (int d = Indeces[0]; d >= Indeces[0] - 5; d--)
                            {
                                if (d == Indeces[0])
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
                            double repeatIndex = StaticVariable.SampleInterval;

                            // then value doesnt have enough elements after
                            ss = "There are less than 5 values after the triggered value in this iteration " + Environment.NewLine;
                            StaticVariable.form.InsertText(ss);
                            for (int d = Indeces[0] - 5; d < time.Count; d++)
                            {
                                SampleToPlot.Add(sample[d]);
                            }

                            // Set positive side of time and zero time point
                            for (int d = Indeces[0]; d <= Indeces[0] + 5; d++)
                            {
                                if (d == Indeces[0])
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


                        time1 = time[Indeces[0]];
                        sample1 = sample[Indeces[0]];


                        CenterValue = SampleToPlot.FindIndex(item => item == sample1);
                        bool ExistValueAround = ((SampleToPlot.Count > 0) && (CenterValue < SampleToPlot.Count)) ? true : false;


                        switch (StaticVariable.form.GetIndexComBo())
                        {
                            // No direction
                            case -1:
                                ss = "No item is selected in the combo box " + Environment.NewLine;
                                StaticVariable.form.InsertText(ss);

                                // Show
                                StaticVariable.form.PlottingMethod(TimeToPlot, SampleToPlot, time1, sample1);
                                break;

                            // Rising
                            case 0:
                                if (ExistValueAround && (SampleToPlot[CenterValue + 1] > sample1))
                                {

                                    ss = "Rising mode is selected in the combo box" + Environment.NewLine;
                                    // Gör rising
                                    StaticVariable.form.PlottingMethod(TimeToPlot, SampleToPlot, time1, sample1);
                                }
                                break;

                            // Falling
                            case 1:

                                if (ExistValueAround && (SampleToPlot[CenterValue + 1] < sample1))
                                {
                                    ss = "Falling mode is selected in the combo box " + Environment.NewLine;
                                    // Gör falling
                                    StaticVariable.form.PlottingMethod(TimeToPlot, SampleToPlot, time1, sample1);
                                }

                                break;
                        }


                        // Send data to be plotted
                        StaticVariable.form.PlottingMethod(TimeToPlot, SampleToPlot, time1, sample1);

                        // sleep
                        //Thread.Sleep(500);

                    }
                    StaticVariable.RunSingle = false;
                }
                else
                {

                }


            }

        }

        #endregion

    }
}
