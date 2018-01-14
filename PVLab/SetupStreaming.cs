using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using PS5000AImports;
using PS5000APinnedArray;
using PicoStatus;

namespace PVLab
{
    struct ChannelSettings
    {
        public Imports.Range range;
        public bool enabled;
    }

    class SetupStreaming
    {
        #region Public and private members

        public string s;
        private bool powerSupplyConnected = true;
        private short _handle;
        int _channelCount;
        public ChannelSettings[] _channelSettings;


        short[][] appBuffers;
        short[][] buffers;

        private int bufferSize = 1024 * 200;

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


        #endregion

        #region Constructo
        /// <summary>
        /// Constructor
        /// </summary>
        public SetupStreaming()
        {

        }
        #endregion

        #region Public properties with default values
        // Set channel
        public Imports.Channel ChannelSelected { get; set; } = Imports.Channel.ChannelA;

        // Set time
        public Imports.ReportedTimeUnits reportedTimeUnits { get; set; } = Imports.ReportedTimeUnits.MicroSeconds;

        // Set last range
        public Imports.Range _lastRange { get; set; } = Imports.Range.Range_2V;

        // Set first range
        public Imports.Range _firstRange { get; set; } = Imports.Range.Range_20V;

        // Set Resulotion
        public Imports.DeviceResolution _resolution { get; set; } = Imports.DeviceResolution.PS5000A_DR_14BIT;

        // Set Coupling
        public Imports.Coupling _coupling { get; set; } = Imports.Coupling.PS5000A_DC;

        // Set Sampling Intervall
        public uint _sampleIntervall { get; set; } = 1000;

        // Set sampling counts
        public uint _sampleNumber { get; set; } = 60000;

        // Set voltage range
        public Imports.Range _voltageRange { get; set; } = Imports.Range.Range_5V;

        // Set up streaming type
        public uint typeOfStreamin { get; set; }

        // Set up 

        #endregion


        /****************************************************************************
        * Callback
        * Used by ps5000a data streaming collection calls, on receipt of data.
        * Used to set global flags etc checked by user routines
        ****************************************************************************/
        void StreamingCallback(short handle,
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
                if (_channelSettings[(int)(Imports.Channel.ChannelA)].enabled)
                {
                    Array.Copy(buffers[0], _startIndex, appBuffers[0], _startIndex, _sampleCount); //max
                    Array.Copy(buffers[0 + 1], _startIndex, appBuffers[0 + 1], _startIndex, _sampleCount);//min
                }
            }
        }

        /****************************************************************************
           * adc_to_mv
           *
           * Convert an 16-bit ADC count into millivolts
           ****************************************************************************/
        int adc_to_mv(int raw, int ch)
        {
            return (raw * inputRanges[ch]) / _maxValue;
        }


        /****************************************************************************
           * mv_to_adc
           *
           * Convert a millivolt value into a 16-bit ADC count
           *
           *  (useful for setting trigger thresholds)
           ****************************************************************************/
        short mv_to_adc(short mv, short ch)
        {
            return (short)((mv * _maxValue) / inputRanges[ch]);
        }

        /****************************************************************************
         * Stream Data Handler
         * - Used by the two stream data examples - untriggered and triggered
         * Inputs:
         * - unit - the unit to sample on
         * - preTrigger - the number of samples in the pre-trigger phase 
         *					(0 if no trigger has been set)
         ***************************************************************************/
        void StreamDataHandler(uint preTrigger)
        {
            int sampleCount = 1024 * 100; /*  *100 is to make sure buffer large enough */

            appBuffers = new short[_channelCount][];
            buffers = new short[_channelCount][];

            int totalSamples = 0;
            uint triggeredAt = 0;
            uint sampleInterval = (uint)_sampleIntervall;
            uint status;

            // Use Pinned Arrays for the application buffers
            PinnedArray<short>[] appBuffersPinned = new PinnedArray<short>[_channelCount * 2];

           // for (int ch = 0; ch < _channelCount * 2; ch += 1) // create data buffers
            //{
                buffers[0] = new short[sampleCount];
                buffers[ 1] = new short[sampleCount];

                appBuffers[0] = new short[sampleCount];
                appBuffers[ 1] = new short[sampleCount];

                appBuffersPinned[0] = new PinnedArray<short>(appBuffers[ch]);
                appBuffersPinned[1] = new PinnedArray<short>(appBuffers[ch + 1]);

                status = Imports.SetDataBuffers(_handle, (Imports.Channel)(0), buffers[0], buffers[1], sampleCount, 0, Imports.RatioMode.None);
            }


            _autoStop = false;

            status = Imports.RunStreaming(_handle, ref sampleInterval, reportedTimeUnits, preTrigger, 1000000 - preTrigger, 1, 1, Imports.RatioMode.None, (uint)sampleCount);

            ViewInformation(totalSamples, triggeredAt, sampleInterval);


            while (!_autoStop)
            {
                /* Poll until data is received. Until then, GetStreamingLatestValues wont call the callback */
                Thread.Sleep(0);
                _ready = false;
                status = Imports.GetStreamingLatestValues(_handle, StreamingCallback, IntPtr.Zero);

                //Console.Write((status > 0 && status != 39 /*PICO_BUSY*/) ? "Status =  {0}\n" : "", status);

                if (_ready && _sampleCount > 0) /* can be ready and have no data, if autoStop has fired */
                {
                    if (_trig > 0)
                    {
                        triggeredAt = (uint)totalSamples + _trigAt;
                    }

                    totalSamples += _sampleCount;


                }
            }



            Imports.Stop(_handle);

        }

        private string ViewInformation(int? totalSamples, uint? triggeredAt, uint? sampleInterval)
        {

            var s = String.Format("Total sample : {0} \n Triggered at {1} \n Sampling intervall \n", totalSamples, triggeredAt, sampleInterval);
            return s;
        }


        /****************************************************************************
        * Initialise unit' structure with Variant specific defaults
        ****************************************************************************/
        void GetDeviceInfo()
        {
            string[] description = {
                                       "Driver Version",
                                       "USB Version",
                                       "Hardware Version",
                                       "Variant Info",
                                       "Serial",
                                       "Calibration Date",
                                       "Kernel Version",
                                       "Digital Hardware",
                                       "Analogue Hardware",
                                       "Firmware 1",
                                       "Firmware 2"
                                    };

            // passing the device Info


            System.Text.StringBuilder line = new System.Text.StringBuilder(80);

            if (_handle >= 0)
            {
                for (int i = 0; i < description.Length; i++)
                {
                    short requiredSize;
                    Imports.GetUnitInfo(_handle, line, 80, out requiredSize, (uint)i);
                    // Device info


                    if (_powerSupplyConnected)
                    {
                        if (i == 3)
                        {
                            // The third one is "Variant Info"
                            _channelCount = int.Parse(line[1].ToString());

                        }
                    }
                    else
                    {
                        _channelCount = 2;
                    }
                    ViewDeviceInformation(description[i], line);
                    _channelSettings = new ChannelSettings[_channelCount];
                    _noEnabledChannels = _channelCount;

                }


                // Kan sättas för hand
                for (uint ch = 0; ch < _channelCount; ch++)
                {
                    Imports.SetChannel(_handle, ChannelSelected, 1, _coupling, _voltageRange, 0);
                    _channelSettings[ch].enabled = true;
                    _channelSettings[ch].range = _voltageRange;
                }
            }
        }

        /// <summary>
        /// View information of the device
        /// </summary>
        /// <param name="v"></param>
        /// <param name="line"></param>
        public void ViewDeviceInformation(string v, StringBuilder line)
        {
            s = String.Format("{0}, {1}", v, line);

        }

        /****************************************************************************
        * Display Device Settings
        ****************************************************************************/
        void DisplaySettings()
        {

            int voltage;
            uint istatus = StatusCodes.PICO_OK;

            if (!(_channelSettings[0].enabled))
            {
                s = "channel A, Voltage range off\n ";
            }
            else
            {
                voltage = inputRanges[(int)_channelSettings[0].range];
                if (voltage < 1000)
                {
                    // Report
                    s = voltage.ToString() + "{0}mV\n";
                }
                else
                {
                    // Report
                    s = (voltage / 1000).ToString() + "{0}V\n";
                }
            }

            // To avoid conflics
            Imports.DeviceResolution resolution = _resolution;
            istatus = Imports.GetDeviceResolution(_handle, out resolution);

            // Report
            s = "The resolution is now " + _resolution;


            if (_powerSupplyConnected)
            {
                // Report
                s = "Power Supply connected";
            }
            else
            {
                // Report
                s = "USB connected";

            }
        }


        /****************************************************************************
        * CollectStreamingImmediate
        *  this function demonstrates how to collect a stream of data
        *  from the unit (start collecting immediately)
        ***************************************************************************/
        public void CollectStreamingImmediate()
        {

            Imports.SetSimpleTrigger(_handle, 0, Imports.Channel.ChannelA, 0, Imports.ThresholdDirection.None, 0, 0);

            StreamDataHandler(0);
        }

        /****************************************************************************
         * CollectStreamingTriggered
         *  this function demonstrates how to collect a stream of data
         *  from the unit (start collecting on trigger)
         ***************************************************************************/
        public void CollectStreamingTriggered()
        {
            short triggerVoltage = mv_to_adc(1000, (short)_channelSettings[(int)Imports.Channel.ChannelA].range); // ChannelInfo stores ADC counts            


            Imports.SetSimpleTrigger(_handle, 1, ChannelSelected, triggerVoltage, Imports.ThresholdDirection.Rising, 0, 0);

            StreamDataHandler(100000); // Collect 100000 pre-trigger samples
        }

        /****************************************************************************
        * Select resolution of device
        ****************************************************************************/
        //void SetResolution()
        //{
        //    bool ivalid;
        //    int maxSelection = 2;
        //    uint istatus;

        //    if (_noEnabledChannels <= 2)
        //    {
        //        Console.WriteLine("3 : 15 bits"); //can only use up to 2 channels with 15 bit mode     
        //        if (_noEnabledChannels < 2)
        //        {
        //            Console.WriteLine("4 : 16 bits"); //can only use 1 channel with 16 bit mode
        //            maxSelection = 4;
        //        }
        //        else
        //        {
        //            maxSelection = 3;
        //        }
        //    }

        //    //Console.WriteLine();

        //    do
        //    {
        //        try
        //        {
        //            //Console.WriteLine("Resolution: ");
        //            _resolution = (Imports.DeviceResolution)(uint.Parse(Console.ReadLine()));
        //            ivalid = true;
        //        }
        //        catch (FormatException e)
        //        {
        //            ivalid = false;
        //            //Console.WriteLine("Error: " + e.Message);
        //        }

        //        if (_resolution > (Imports.DeviceResolution)maxSelection)
        //        {
        //            //Console.WriteLine("Please select a number stated above");
        //            ivalid = false;
        //        }

        //    } while (!ivalid);

        //    if ((istatus = Imports.SetDeviceResolution(_handle, _resolution)) != 0)
        //    {
        //        //Console.WriteLine("Resolution not set Error code: {0)", istatus);
        //    }

        //}


        /*************************************************************************************
        * Run
        *  main menu
        *  
        **************************************************************************************/
        public void Run()
        {
            // Display the device info
            GetDeviceInfo();

            // resulotion
            DisplaySettings();



            Imports.MaximumValue(_handle, out _maxValue); // Set max. ADC Counts


            // set voltage and it's members
            SetVoltage();

            // Start Streaming
            StartStreaming();
        }



        /// <summary>
        /// Start sampling
        /// </summary>
        /// <returns></returns>
        [STAThread]
        public void First()
        {
            short handle;
            bool powerSupplyConnected = true;
            uint status = Imports.OpenUnit(out handle, null, Imports.DeviceResolution.PS5000A_DR_8BIT);

            if (status != StatusCodes.PICO_OK && handle != 0)
            {
                status = Imports.ChangePowerSource(handle, status);
                powerSupplyConnected = false;
            }
            else
            {
                s = "Device opened succefully";
                _handle = handle;
                powerSupplyConnected = true;
                Run();
            }
        }

        /// <summary>
        /// Start Streaming
        /// </summary>
        private void StartStreaming()
        {
            uint sampleInterval = (uint)_sampleIntervall;
            uint samplenumber = (uint)_sampleNumber;
            uint status;
            status = Imports.RunStreaming(_handle, ref sampleInterval, reportedTimeUnits, 0, samplenumber, 1, 1, Imports.RatioMode.None, (uint)bufferSize);
            StreamDataHandler(0);
        }

        /// <summary>
        /// SetVoltage
        /// </summary>
        private void SetVoltage()
        {

            int noAllowedEnabledChannels;
            uint status;

            switch (_resolution)
            {
                case Imports.DeviceResolution.PS5000A_DR_15BIT:

                    noAllowedEnabledChannels = 2;
                    break;

                case Imports.DeviceResolution.PS5000A_DR_16BIT:

                    noAllowedEnabledChannels = 1;
                    break;

                default:

                    noAllowedEnabledChannels = _channelCount;
                    break;
            }


            _noEnabledChannels = 0;

            uint range = (uint)_voltageRange;
            status = Imports.SetChannel(_handle, ChannelSelected, 1, 0, (Imports.Range)range, 0);
            _channelSettings[0].enabled = true;
            _channelSettings[0].range = (Imports.Range)range;
        }


        /// <summary>
        /// Close Device
        /// </summary>
        public void CloseDevice()
        {
            Imports.CloseUnit(_handle);
            _handle = 0;
        }
    }
}