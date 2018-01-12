namespace PVLab
{
    partial class PVLab
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.rbNone = new System.Windows.Forms.RadioButton();
            this.rbRepeat = new System.Windows.Forms.RadioButton();
            this.rbRapid = new System.Windows.Forms.RadioButton();
            this.rbETS = new System.Windows.Forms.RadioButton();
            this.rbSingle = new System.Windows.Forms.RadioButton();
            this.rbAuto = new System.Windows.Forms.RadioButton();
            this.btnStart = new System.Windows.Forms.Button();
            this.cbSamplingsInt = new System.Windows.Forms.ComboBox();
            this.cbTimebase = new System.Windows.Forms.ComboBox();
            this.cbSamplingRate = new System.Windows.Forms.ComboBox();
            this.cbChannels = new System.Windows.Forms.ComboBox();
            this.cbStreaming = new System.Windows.Forms.ComboBox();
            this.cbVoltage = new System.Windows.Forms.ComboBox();
            this.cbCoupling = new System.Windows.Forms.ComboBox();
            this.cbRes = new System.Windows.Forms.ComboBox();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.cbRes);
            this.panel1.Controls.Add(this.cbCoupling);
            this.panel1.Controls.Add(this.cbVoltage);
            this.panel1.Controls.Add(this.cbStreaming);
            this.panel1.Controls.Add(this.cbChannels);
            this.panel1.Controls.Add(this.cbSamplingRate);
            this.panel1.Controls.Add(this.cbTimebase);
            this.panel1.Controls.Add(this.cbSamplingsInt);
            this.panel1.Controls.Add(this.btnStart);
            this.panel1.Controls.Add(this.rbETS);
            this.panel1.Controls.Add(this.rbSingle);
            this.panel1.Controls.Add(this.rbAuto);
            this.panel1.Controls.Add(this.rbRapid);
            this.panel1.Controls.Add(this.rbRepeat);
            this.panel1.Controls.Add(this.rbNone);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(12, 13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1018, 113);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Sampling Int:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Timebase:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(848, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Resolution:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(495, 33);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Sampling Rate:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(495, 73);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Channels:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(681, 33);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(57, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Streaming:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(681, 73);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(46, 13);
            this.label7.TabIndex = 7;
            this.label7.Text = "Voltage:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(848, 33);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(51, 13);
            this.label8.TabIndex = 8;
            this.label8.Text = "Coupling:";
            // 
            // rbNone
            // 
            this.rbNone.AutoSize = true;
            this.rbNone.Location = new System.Drawing.Point(247, 29);
            this.rbNone.Name = "rbNone";
            this.rbNone.Size = new System.Drawing.Size(51, 17);
            this.rbNone.TabIndex = 10;
            this.rbNone.TabStop = true;
            this.rbNone.Text = "None";
            this.rbNone.UseVisualStyleBackColor = true;
            // 
            // rbRepeat
            // 
            this.rbRepeat.AutoSize = true;
            this.rbRepeat.Location = new System.Drawing.Point(247, 52);
            this.rbRepeat.Name = "rbRepeat";
            this.rbRepeat.Size = new System.Drawing.Size(60, 17);
            this.rbRepeat.TabIndex = 11;
            this.rbRepeat.TabStop = true;
            this.rbRepeat.Text = "Repeat";
            this.rbRepeat.UseVisualStyleBackColor = true;
            // 
            // rbRapid
            // 
            this.rbRapid.AutoSize = true;
            this.rbRapid.Location = new System.Drawing.Point(247, 75);
            this.rbRapid.Name = "rbRapid";
            this.rbRapid.Size = new System.Drawing.Size(53, 17);
            this.rbRapid.TabIndex = 12;
            this.rbRapid.TabStop = true;
            this.rbRapid.Text = "Rapid";
            this.rbRapid.UseVisualStyleBackColor = true;
            // 
            // rbETS
            // 
            this.rbETS.AutoSize = true;
            this.rbETS.Location = new System.Drawing.Point(325, 75);
            this.rbETS.Name = "rbETS";
            this.rbETS.Size = new System.Drawing.Size(46, 17);
            this.rbETS.TabIndex = 15;
            this.rbETS.TabStop = true;
            this.rbETS.Text = "ETS";
            this.rbETS.UseVisualStyleBackColor = true;
            // 
            // rbSingle
            // 
            this.rbSingle.AutoSize = true;
            this.rbSingle.Location = new System.Drawing.Point(324, 52);
            this.rbSingle.Name = "rbSingle";
            this.rbSingle.Size = new System.Drawing.Size(54, 17);
            this.rbSingle.TabIndex = 14;
            this.rbSingle.TabStop = true;
            this.rbSingle.Text = "Single";
            this.rbSingle.UseVisualStyleBackColor = true;
            // 
            // rbAuto
            // 
            this.rbAuto.AutoSize = true;
            this.rbAuto.Location = new System.Drawing.Point(324, 29);
            this.rbAuto.Name = "rbAuto";
            this.rbAuto.Size = new System.Drawing.Size(47, 17);
            this.rbAuto.TabIndex = 13;
            this.rbAuto.TabStop = true;
            this.rbAuto.Text = "Auto";
            this.rbAuto.UseVisualStyleBackColor = true;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(401, 49);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 16;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            // 
            // cbSamplingsInt
            // 
            this.cbSamplingsInt.FormattingEnabled = true;
            this.cbSamplingsInt.Location = new System.Drawing.Point(87, 30);
            this.cbSamplingsInt.Name = "cbSamplingsInt";
            this.cbSamplingsInt.Size = new System.Drawing.Size(121, 21);
            this.cbSamplingsInt.TabIndex = 17;
            // 
            // cbTimebase
            // 
            this.cbTimebase.FormattingEnabled = true;
            this.cbTimebase.Location = new System.Drawing.Point(87, 68);
            this.cbTimebase.Name = "cbTimebase";
            this.cbTimebase.Size = new System.Drawing.Size(121, 21);
            this.cbTimebase.TabIndex = 18;
            // 
            // cbSamplingRate
            // 
            this.cbSamplingRate.FormattingEnabled = true;
            this.cbSamplingRate.Location = new System.Drawing.Point(580, 30);
            this.cbSamplingRate.Name = "cbSamplingRate";
            this.cbSamplingRate.Size = new System.Drawing.Size(95, 21);
            this.cbSamplingRate.TabIndex = 19;
            // 
            // cbChannels
            // 
            this.cbChannels.FormattingEnabled = true;
            this.cbChannels.Location = new System.Drawing.Point(580, 70);
            this.cbChannels.Name = "cbChannels";
            this.cbChannels.Size = new System.Drawing.Size(95, 21);
            this.cbChannels.TabIndex = 20;
            // 
            // cbStreaming
            // 
            this.cbStreaming.FormattingEnabled = true;
            this.cbStreaming.Location = new System.Drawing.Point(744, 30);
            this.cbStreaming.Name = "cbStreaming";
            this.cbStreaming.Size = new System.Drawing.Size(95, 21);
            this.cbStreaming.TabIndex = 21;
            // 
            // cbVoltage
            // 
            this.cbVoltage.FormattingEnabled = true;
            this.cbVoltage.Location = new System.Drawing.Point(744, 70);
            this.cbVoltage.Name = "cbVoltage";
            this.cbVoltage.Size = new System.Drawing.Size(95, 21);
            this.cbVoltage.TabIndex = 22;
            // 
            // cbCoupling
            // 
            this.cbCoupling.FormattingEnabled = true;
            this.cbCoupling.Location = new System.Drawing.Point(908, 30);
            this.cbCoupling.Name = "cbCoupling";
            this.cbCoupling.Size = new System.Drawing.Size(95, 21);
            this.cbCoupling.TabIndex = 23;
            // 
            // cbRes
            // 
            this.cbRes.FormattingEnabled = true;
            this.cbRes.Location = new System.Drawing.Point(908, 73);
            this.cbRes.Name = "cbRes";
            this.cbRes.Size = new System.Drawing.Size(95, 21);
            this.cbRes.TabIndex = 24;
            // 
            // txtStatus
            // 
            this.txtStatus.Location = new System.Drawing.Point(850, 145);
            this.txtStatus.Multiline = true;
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.Size = new System.Drawing.Size(180, 546);
            this.txtStatus.TabIndex = 1;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(847, 129);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(40, 13);
            this.label9.TabIndex = 2;
            this.label9.Text = "Status:";
            // 
            // PVLab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1042, 703);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.panel1);
            this.MaximumSize = new System.Drawing.Size(1058, 742);
            this.MinimumSize = new System.Drawing.Size(1058, 742);
            this.Name = "PVLab";
            this.Text = "PVLab";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rbETS;
        private System.Windows.Forms.RadioButton rbSingle;
        private System.Windows.Forms.RadioButton rbAuto;
        private System.Windows.Forms.RadioButton rbRapid;
        private System.Windows.Forms.RadioButton rbRepeat;
        private System.Windows.Forms.RadioButton rbNone;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.ComboBox cbTimebase;
        private System.Windows.Forms.ComboBox cbSamplingsInt;
        private System.Windows.Forms.ComboBox cbRes;
        private System.Windows.Forms.ComboBox cbCoupling;
        private System.Windows.Forms.ComboBox cbVoltage;
        private System.Windows.Forms.ComboBox cbStreaming;
        private System.Windows.Forms.ComboBox cbChannels;
        private System.Windows.Forms.ComboBox cbSamplingRate;
        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.Label label9;
    }
}

