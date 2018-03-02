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


    public class SettingUpBlockRecordAndStream : Scaling
    {

        #region Private and public members
        public double[] sampleTime;
        
        public const int BUFFER_SIZE = 1024;
        public const int MAX_CHANNELS = 4;
        public const int QUAD_SCOPE = 4;
        public const int DUAL_SCOPE = 2;
        public short _overflow = 0;



        // Graph class
        Plot plot;
        StreamingClass StreamingSet;


        public uint _timebase;
        public short _oversample = 1;
        public bool _scaleVoltages = true;


        public bool _ready = false;
        public short _trig = 0;
        public uint _trigAt = 0;
        public int _sampleCount;
        public uint _startIndex = 0;
        public bool _autoStop;
        public ChannelSettings[] _channelSettings;
        public int _channelCount;
        public int _digitalPorts;
        public Imports.ps5000aBlockReady _callbackDelegate;
        public double[] samples;
        public int[] time;
        public static short _maxValue;
        public int[] minValue;
        public int[] maxValue;
        public static string s;


        public PinnedArray<short>[] appBuffersPinned;
        public int bufferSize = 1024 * 100;
        public uint streamingSamples = 60000;
        public short[][] channelBuffers;
        public short[][] buffers;
        private short handle;
        public   ChannelSettings[] _channelSettings1;


        public static int t = 1;
        public static int d;
        #endregion

        #region Construction
        public SettingUpBlockRecordAndStream()
        {



        }
        #endregion

        #region Open, close device, setchannel, getmaxValue methods
        /// <summary>
        /// Opens, closes and sets the maximum value of the device. Needs to be called 
        /// initially. User needs to set it manually if changes are desired
        /// </summary>
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
            if (StaticVariable._handle > 0)
            {
                Imports.CloseUnit(StaticVariable._handle);
                StaticVariable._handle = 0;

            }
            else
            {
                uint status = Imports.OpenUnit(out handle, null, StaticVariable.resolution);

                if (handle > 0)
                {
                    StaticVariable._handle = handle;

                    if (status == StatusCodes.PICO_POWER_SUPPLY_NOT_CONNECTED)
                    {
                        status = Imports.ChangePowerSource(StaticVariable._handle, status);
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
                        Imports.GetUnitInfo(StaticVariable._handle, UnitInfo, 80, out requiredSize, (uint)i);
                        s += (description[i] + UnitInfo + "\n") + Environment.NewLine;

                        // We make a new channelsetting in order to make the rest to work
                        // channelsetting is a structure defined earlier.
                        _channelSettings = new ChannelSettings[1];
                        _channelSettings[0].enabled = true;
                        _channelSettings[0].range = StaticVariable.SelVolt;
                        StaticVariable._channelSettings = _channelSettings;


                    }

                    d = 1;

                }
            }
        }

        /// <summary>
        ///  Closing the unit and inactivate the handel
        /// </summary>
        public  static void CloseUnit()
        {
            Imports.CloseUnit(StaticVariable._handle);
            StaticVariable._handle = 0;
        }


        public static void SetChannel()
        {
            uint status;
            status = Imports.SetChannel(StaticVariable._handle, StaticVariable.SelChannel, 1, StaticVariable.SelCoup, StaticVariable.SelVolt, 0);

        }

        public static void MaxValue()
        {
            uint status;
            status = Imports.MaximumValue(StaticVariable._handle, out _maxValue);
            StaticVariable._maxValue = _maxValue;

        }

        #endregion

        #region Capturing Method
        /// <summary>
        /// Calling capture from capturing class
        /// </summary>
        public void CapturingMethod()
        {
            if (StaticVariable._handle != 0)
            {
                Capturing capturing = new Capturing();
                capturing.RunBlock();
            }

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
        public void StreamingMethod()
        {
            if (StaticVariable._handle != 0)
            {
                Plot plot = new Plot();
                plot.Show();
            }
        }
        #endregion




    }
}
