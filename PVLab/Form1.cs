using PS5000AImports;
using System;
using System.Windows.Forms;
using System.Collections.Generic;



namespace PVLab
{
    public partial class PVLab : Form
    {
        #region Variable
        private short _handle;
        ChannelSettings[] _channelSettings;
        SettingUpBlockRecordAndStream PicoSetup;
        DictionaryValues dictionaryValues;
        Dictionary<double, string> timeBaseDict;
        double SampllingIntervall;
        double timeinterval;
        double frequency;
        double value;

        #endregion

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

        }
        #endregion

        #region UI events
        /// <summary>
        /// Streaming button. Open button needs to be clicked before streaming.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStreaming_Click(object sender, EventArgs e)
        {
            if (PicoSetup != null && Condition())
            {
                PicoSetup.StreamingMethod();

            }
            else
            {
                MessageBox.Show("Click on Open first");
            }


        }

        /// <summary>
        /// When program opens the driver.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpen_Click(object sender, EventArgs e)
        {

            if (Condition() && PicoSetup == null)
            {
                SampllingIntervall = uint.Parse(txtSamplingInterval.Text);
                StaticVariable.SelChannel = (Imports.Channel)cbChannels.SelectedIndex;
                StaticVariable.SelCoup = (Imports.Coupling)cbCoupling.SelectedIndex;
                StaticVariable.SelVolt = (Imports.Range)cbVoltage.SelectedIndex;
                StaticVariable.SelChannelIndex = cbChannels.SelectedIndex;
                StaticVariable.SelRangeIndex = cbVoltage.SelectedIndex;
                StaticVariable.SampleInterval = SampllingIntervall;
                StaticVariable.NumberOfSamples = SampllingIntervall;
                StaticVariable.resolution = (Imports.DeviceResolution)cbRes.SelectedIndex;

                UpdateTextBoxProperties();
                PicoSetup = new SettingUpBlockRecordAndStream();

                PicoSetup.OpenUnit();

                if (SettingUpBlockRecordAndStream.t == 0)
                {
                    MessageBox.Show("Cannot open device error code:", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
                if (SettingUpBlockRecordAndStream.d == 1)
                    btnOpen.Text = "Close";

                txtStatus.AppendText(SettingUpBlockRecordAndStream.s);

            }
            else if (Condition() && PicoSetup != null)
            {
                SettingUpBlockRecordAndStream.CloseUnit();
                PicoSetup = null;
                btnOpen.Text = "Open Unit";
                txtStatus.AppendText("Unit closed" + Environment.NewLine);
            }
        }

        /// <summary>
        /// Updates the text status box
        /// </summary>
        private void UpdateTextBoxProperties()
        {
            if (Condition())
            {

                SampllingIntervall = uint.Parse(txtSamplingInterval.Text);
                txtProperty.AppendText("Chosen " + SampllingIntervall.ToString() + " ns" + Environment.NewLine);

                // Time base calculations
                StaticVariable.resolution = (Imports.DeviceResolution)cbRes.SelectedItem;

            }
        }

        /// <summary>
        /// Starts recording for some seconds.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (PicoSetup != null && Condition())
            {
                PicoSetup.CapturingMethod();
            }
            else
            {
                MessageBox.Show("Click on Open first");
            }

        }


        #endregion

        #region Condition 
        /// <summary>
        /// Conditions to run the program. 
        /// </summary>
        /// <returns></returns>
        private bool Condition()
        {
            bool okej = false;
            double n;
            var st = double.TryParse(txtSamplingInterval.Text, out n);
            if (!string.IsNullOrWhiteSpace(txtSamplingInterval.Text) && st != false)
            {
                okej = true;

            }
            else { MessageBox.Show("Error: Please make sure that sampling intervall is an integer or you have clicked on Open"); }
            return okej;
        }

        #endregion

        private void cbRes_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbRes.SelectedIndex)
            {
                case 0:
                    txtStatus.AppendText("You have chosen 8 Bit resolution. You can sample at most with an interval of 1ns" + Environment.NewLine);
                    break;
                case 1:
                    txtStatus.AppendText("You have chosen 12 Bit resolution. You can sample at most with an interval of 2ns" + Environment.NewLine);
                    break;
                case 2:
                    txtStatus.AppendText("You have chosen 14 Bit resolution. You can sample at most with an interval of 8ns" + Environment.NewLine);
                    break;
                case 3:
                    txtStatus.AppendText("You have chosen 15 Bit resolution. You can sample at most with an intervall of 8ns" + Environment.NewLine);
                    break;
                case 4:
                    txtStatus.AppendText("You have chosen 16 Bit resolution. You can sample at most with an intervall of 16ns" + Environment.NewLine);
                    break;
            }
        }
    }

}




