using LiveCharts;
using PicoPinnedArray;
using PicoStatus;
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
using System.Windows.Forms;
using System.Windows.Media;
using LiveCharts.Wpf;
using LiveCharts.WinForms;


namespace PVLab
{
    public partial class PVLab : Form
    {
        private short _handle;
        ChannelSettings[] _channelSettings;
        PicoSetup PicoSetup;



        #region Ctor
        public PVLab()
        {
            InitializeComponent();

            // Initiate GUI
            InitiateGUI();
        }
        #endregion

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
            cbCoupling.DataSource = Enum.GetValues(typeof(Imports.Coupling));
            cbStreaming.DataSource = Enum.GetValues(typeof(Imports.streaminType));
        }
        #endregion

        #region UI events
        private async void btnStreaming_Click(object sender, EventArgs e)
        {
            if (PicoSetup != null && Condition())
            {
                await Task.Run(() => PicoSetup.Streaming());
            }
            else
            {
                MessageBox.Show("Click on Open first");
            }


        }
        private void btnOpen_Click(object sender, EventArgs e)
        {

            if (Condition() && PicoSetup == null)
            {
                PicoSetup = new PicoSetup()
                {
                    SelChannel = (Imports.Channel)cbChannels.SelectedIndex,
                    SelCoup = (Imports.Coupling)cbCoupling.SelectedIndex,
                    SelVolt = (Imports.Range)cbVoltage.SelectedIndex,
                    SelChannelIndex = cbChannels.SelectedIndex,
                    SampleInterval = uint.Parse(txtSamplingInterval.Text),
                    resolution = (Imports.DeviceResolution)cbRes.SelectedIndex,
                };

                PicoSetup.OpenUnit();

                if (PicoSetup.t == 0)
                {
                    MessageBox.Show("Cannot open device error code:", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
                if (PicoSetup.d == 1)
                    btnOpen.Text = "Close";

                txtStatus.AppendText(PicoSetup.s);

            }
            else if(Condition() && PicoSetup != null)
            {
                PicoSetup.CloseUnit();
                PicoSetup = null;
                btnOpen.Text = "Open Unit";
                txtStatus.AppendText("Unit closed"+Environment.NewLine);
            }
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (PicoSetup != null && Condition())
            {
                PicoSetup.BlockRunner();
            }
            else
            {
                MessageBox.Show("Click on Open first");
            }

        }
        #endregion


        #region Help function
        private void txtSamplingInterval_TextChanged(object sender, EventArgs e)
        {

            if (Condition())
                txtRate.Text = (1.0 / ((int.Parse(txtSamplingInterval.Text)) * Math.Pow(10, -9))).ToString();

        }
        #endregion
        private bool Condition()
        {
            bool okej = false;
            int n = 0;
            var st = int.TryParse(txtSamplingInterval.Text, out n);
            if (!string.IsNullOrWhiteSpace(txtSamplingInterval.Text) && n != 0)
            {
                okej = true;

            }
            else { MessageBox.Show("Error: Please make sure that sampling intervall is an integer or you have clicked on Open"); }
            return okej;
        }
    }

}




