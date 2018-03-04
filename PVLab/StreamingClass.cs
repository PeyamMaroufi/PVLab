// Copyright (c) 2018 Payam 
// Made by Payam M. 
// Purpose: Measuring the voltage of high voltage devices scaled down to almost 30 V
// Workes for continues streaming and triggers. Includes repeat trigger and single trigger. and capturing.
// Target framwork is .NET and WINFORM. 
// 

using PS5000AImports;
using PicoPinnedArray;
using PicoStatus;
using System.Threading;
using System;

namespace PVLab
{
    internal class StreamingClass
    {
        #region private members
        private readonly short _handle;
        int _channelCount = 1;
        private ChannelSettings[] _channelSettings;


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
        public static double[] sample;
        public static int[] sampleTime;
        #endregion

        public StreamingClass(short handle)
        {
            _handle = handle;
        }

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

        public void RunStreaminGraph()
        {

            int sampleCount = 1024 * 100; /*  *100 is to make sure buffer large enough */

            appBuffers = new short[_channelCount * 2][];
            buffers = new short[_channelCount * 2][];

            uint preTrigger = 0;
            int totalSamples = 0;
            uint triggeredAt = 0;
            uint sampleInterval = 1;
            uint status;

            System.Timers.Timer myTimer = new System.Timers.Timer();
            myTimer.Interval = 1000;

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
            sample = new double[appBuffersPinned[0].Target.Length];
            sampleTime = new int[appBuffersPinned[0].Target.Length];
            while (!_autoStop)
            {
                /* Poll until data is received. Until then, GetStreamingLatestValues wont call the callback */
                Thread.Sleep(0);
                _ready = false;
                status = Imports.GetStreamingLatestValues(_handle, StreamingCallback, IntPtr.Zero);
                if (_ready)
                {
                    myTimer.Start();
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
                        //for (uint ch = 0; ch < _channelCount * 2; ch += 2)
                        //{

                        sample[i] = adc_to_mv(appBuffersPinned[0].Target[i], inputRanges[SelChannelIndexr]);
                        sampleTime[i] = (int)(i * SampleIntervalr);
                        myTimer.Elapsed += MyTimer_Elapsed;
                        myTimer.AutoReset = true;
                        


                    }

                }

            }
            //myTimer.Stop();



            Imports.Stop(_handle);

        }

        private void MyTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Thread.Sleep(100);
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
                        Array.Copy(buffers[ 1], _startIndex, appBuffers[1], _startIndex, _sampleCount);//min

                    //}
                }
            }
        }


        public double adc_to_mv(int raw, int ch)
        {
            return (raw * inputRanges[ch]) / _maxValue; ;
        }
    }


}



