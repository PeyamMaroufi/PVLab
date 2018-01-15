using PicoStatus;
using PS5000AImports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PVLab
{
    public partial class PVLab : Form
    {
        int voltage, res, channel, coupling, streamType;
        double timebase;

        private short _handle;
        public const int BUFFER_SIZE = 1024;
        public const int MAX_CHANNELS = 4;
        public const int QUAD_SCOPE = 4;
        public const int DUAL_SCOPE = 2;


        uint _timebase = 5;
        short _oversample = 1;
        bool _scaleVoltages = true;

        ushort[] inputRanges = { 10, 20, 50, 100, 200, 500, 1000, 2000, 5000, 10000, 20000, 50000 };
        bool _ready = false;
        short _trig = 0;
        uint _trigAt = 0;
        int _sampleCount = 0;
        uint _startIndex = 0;
        bool _autoStop;
        //private ChannelSettings[] _channelSettings;
        private int _channelCount;
        private Imports.Range _firstRange;
        private Imports.Range _lastRange;
        private int _digitalPorts;
        private Imports.ps5000aBlockReady _callbackDelegate;
        private string StreamFile = "stream.txt";
        private string BlockFile = "block.txt";


        SetupStreaming streaming;
        public static string s;
        public PVLab()
        {
            InitializeComponent();

            // Initiate GUI
            InitiateGUI();
        }


        #region Initiating the combox box and other things

        /// <summary>
        /// Here we are initiating the combox box with the Enums available in the mapp
        /// </summary>
        private void InitiateGUI()
        {
            // Set up  combo boxes
            cbVoltage.DataSource = Enum.GetValues(typeof(Imports.Range));
            cbRes.DataSource = Enum.GetValues(typeof(Imports.DeviceResolution));
            cbChannels.DataSource = Enum.GetValues(typeof(Imports.Channel));
            cbTimebase.DataSource = Enum.GetValues(typeof(Imports.ReportedTimeUnits));
            cbCoupling.DataSource = Enum.GetValues(typeof(Imports.Coupling));
            cbStreaming.DataSource = Enum.GetValues(typeof(Imports.streaminType));
        }
        #endregion

        #region UI events
        private void btnOpen_Click(object sender, EventArgs e)
        {
            StringBuilder UnitInfo = new StringBuilder(80);
            short handle;

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
            Imports.DeviceResolution resolution = (Imports.DeviceResolution)(cbRes.SelectedIndex);

            if (_handle > 0)
            {
                Imports.CloseUnit(_handle);
                txtStatus.Text += "Closing Unit";
                _handle = 0;
                btnOpen.Text += "Open";
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
                        MessageBox.Show("Cannot open device error code: " + status.ToString(), "Error Opening Device", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                    }
                    else
                    {
                        // Do nothing - power supply connected
                    }

                    txtStatus.Text += "Handle            " + _handle.ToString() + "\r\n";

                    for (int i = 0; i < 9; i++)
                    {
                        short requiredSize;
                        Imports.GetUnitInfo(_handle, UnitInfo, 80, out requiredSize, (uint)i);
                        txtStatus.Text += (description[i] + UnitInfo + "\r\n");
                    }
                    btnOpen.Text = "Close";
                }
            }

        }

        #endregion


        #region Help Method

        private void BlockCallback(short handle, short status, IntPtr pVoid)
        {
            // flag to say done reading data
            if (status != (short)StatusCodes.PICO_CANCELLED)
                _ready = true;
        }

        private uint SetTrigger(Imports.TriggerChannelProperties[] channelProperties,
           short nChannelProperties,
           Imports.TriggerConditions[] triggerConditions,
           short nTriggerConditions,
           Imports.ThresholdDirection[] directions,
           uint delay,
           short auxOutputEnabled,
           int autoTriggerMs)
        {
            uint status;

            status = Imports.SetTriggerChannelProperties(_handle, channelProperties, nChannelProperties, auxOutputEnabled,
                                                   autoTriggerMs);
            if (status != StatusCodes.PICO_OK)
            {
                return status;
            }

            status = Imports.SetTriggerChannelConditions(_handle, triggerConditions, nTriggerConditions);

            if (status != StatusCodes.PICO_OK)
            {
                return status;
            }

            if (directions == null)
            {
                directions = new Imports.ThresholdDirection[] { Imports.ThresholdDirection.None,
                                Imports.ThresholdDirection.None, Imports.ThresholdDirection.None, Imports.ThresholdDirection.None,
                                Imports.ThresholdDirection.None, Imports.ThresholdDirection.None};
            }

            status = Imports.SetTriggerChannelDirections(_handle,
                                                               directions[(int)Imports.Channel.ChannelA],
                                                               directions[(int)Imports.Channel.ChannelB],
                                                               directions[(int)Imports.Channel.ChannelC],
                                                               directions[(int)Imports.Channel.ChannelD],
                                                               directions[(int)Imports.Channel.External],
                                                               directions[(int)Imports.Channel.Aux]);
            if (status != StatusCodes.PICO_OK)
            {
                return status;
            }

            status = Imports.SetTriggerDelay(_handle, delay);

            if (status != StatusCodes.PICO_OK)
            {
                return status;
            }

            return status;
        }

        #endregion

    }
}
