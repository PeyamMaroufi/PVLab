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
        string s;
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
            // Set up voltage combo box
            foreach (var range in Enum.GetValues(typeof(Imports.Range)))
            {
                cbVoltage.Items.Add(range);
            }

            // Set up res combo box
            foreach (var res in Enum.GetValues(typeof(Imports.DeviceResolution)))
            {
                cbRes.Items.Add(res);
            }

            // Set up channel combo box
            foreach (var channel in Enum.GetValues(typeof(Imports.Channel)))
            {
                cbChannels.Items.Add(channel);
            }

            // Set up time combo box
            foreach (var time in Enum.GetValues(typeof(Imports.Channel)))
            {
                cbTimebase.Items.Add(time);
            }

            // Set  up coupling combo box
            foreach (var coupling in Enum.GetValues(typeof(Imports.Coupling)))
            {
                cbCoupling.Items.Add(coupling);
            }

        }

        #endregion

        private void btnStart_Click(object sender, EventArgs e)
        {
            // Making a new instance of SetupStreaming which runs the drivers and methods
            SetupStreaming streaming = new SetupStreaming()
            {
                _resolution = (Imports.DeviceResolution)cbRes.SelectedIndex,
                _coupling = (Imports.Coupling)cbCoupling.SelectedIndex,
                _voltageRange = (Imports.Range)cbVoltage.SelectedIndex,
                reportedTimeUnits = (Imports.ReportedTimeUnits)cbTimebase.SelectedIndex,
                _sampleIntervall = uint.Parse(txtSamplingInterval.Text),
            };

            // Litsens to the choices of user
            uint streaminIndex = (uint)cbStreaming.SelectedIndex;
            switch (streaminIndex)
            {
                case 0:
                    streaming.CollectStreamingImmediate();
                    break;
                case 1:
                    streaming.CollectStreamingTriggered();
                    break;             
            }

            // Updating status textbox
            s = streaming.s;
            txtStatus.Text += s + Environment.NewLine;
        }
    }
}
