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
            this.label10 = new System.Windows.Forms.Label();
            this.txtRate = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnStreaming = new System.Windows.Forms.Button();
            this.txtSamplingInterval = new System.Windows.Forms.TextBox();
            this.cbRes = new System.Windows.Forms.ComboBox();
            this.cbCoupling = new System.Windows.Forms.ComboBox();
            this.cbVoltage = new System.Windows.Forms.ComboBox();
            this.cbStreaming = new System.Windows.Forms.ComboBox();
            this.cbChannels = new System.Windows.Forms.ComboBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.rbETS = new System.Windows.Forms.RadioButton();
            this.rbSingle = new System.Windows.Forms.RadioButton();
            this.rbAuto = new System.Windows.Forms.RadioButton();
            this.rbRepeat = new System.Windows.Forms.RadioButton();
            this.rbNone = new System.Windows.Forms.RadioButton();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.txtRate);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.btnOpen);
            this.panel1.Controls.Add(this.btnStreaming);
            this.panel1.Controls.Add(this.txtSamplingInterval);
            this.panel1.Controls.Add(this.cbRes);
            this.panel1.Controls.Add(this.cbCoupling);
            this.panel1.Controls.Add(this.cbVoltage);
            this.panel1.Controls.Add(this.cbStreaming);
            this.panel1.Controls.Add(this.cbChannels);
            this.panel1.Controls.Add(this.btnStart);
            this.panel1.Controls.Add(this.rbETS);
            this.panel1.Controls.Add(this.rbSingle);
            this.panel1.Controls.Add(this.rbAuto);
            this.panel1.Controls.Add(this.rbRepeat);
            this.panel1.Controls.Add(this.rbNone);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(12, 13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1018, 113);
            this.panel1.TabIndex = 0;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(182, 68);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(20, 13);
            this.label10.TabIndex = 31;
            this.label10.Text = "Hz";
            // 
            // txtRate
            // 
            this.txtRate.Location = new System.Drawing.Point(100, 63);
            this.txtRate.Name = "txtRate";
            this.txtRate.Size = new System.Drawing.Size(76, 20);
            this.txtRate.TabIndex = 30;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(182, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 29;
            this.label2.Text = "Nano sec";
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(401, 15);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(75, 23);
            this.btnOpen.TabIndex = 27;
            this.btnOpen.Text = "Open Unit";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnStreaming
            // 
            this.btnStreaming.Location = new System.Drawing.Point(401, 73);
            this.btnStreaming.Name = "btnStreaming";
            this.btnStreaming.Size = new System.Drawing.Size(75, 23);
            this.btnStreaming.TabIndex = 26;
            this.btnStreaming.Text = "Streaming";
            this.btnStreaming.UseVisualStyleBackColor = true;
            this.btnStreaming.Click += new System.EventHandler(this.btnStreaming_Click);
            // 
            // txtSamplingInterval
            // 
            this.txtSamplingInterval.Location = new System.Drawing.Point(100, 31);
            this.txtSamplingInterval.Name = "txtSamplingInterval";
            this.txtSamplingInterval.Size = new System.Drawing.Size(76, 20);
            this.txtSamplingInterval.TabIndex = 25;
            this.txtSamplingInterval.TextChanged += new System.EventHandler(this.txtSamplingInterval_TextChanged);
            // 
            // cbRes
            // 
            this.cbRes.FormattingEnabled = true;
            this.cbRes.Location = new System.Drawing.Point(908, 73);
            this.cbRes.Name = "cbRes";
            this.cbRes.Size = new System.Drawing.Size(95, 21);
            this.cbRes.TabIndex = 24;
            // 
            // cbCoupling
            // 
            this.cbCoupling.FormattingEnabled = true;
            this.cbCoupling.Location = new System.Drawing.Point(908, 30);
            this.cbCoupling.Name = "cbCoupling";
            this.cbCoupling.Size = new System.Drawing.Size(95, 21);
            this.cbCoupling.TabIndex = 23;
            // 
            // cbVoltage
            // 
            this.cbVoltage.FormattingEnabled = true;
            this.cbVoltage.Location = new System.Drawing.Point(744, 70);
            this.cbVoltage.Name = "cbVoltage";
            this.cbVoltage.Size = new System.Drawing.Size(95, 21);
            this.cbVoltage.TabIndex = 22;
            // 
            // cbStreaming
            // 
            this.cbStreaming.FormattingEnabled = true;
            this.cbStreaming.Location = new System.Drawing.Point(744, 30);
            this.cbStreaming.Name = "cbStreaming";
            this.cbStreaming.Size = new System.Drawing.Size(95, 21);
            this.cbStreaming.TabIndex = 21;
            // 
            // cbChannels
            // 
            this.cbChannels.FormattingEnabled = true;
            this.cbChannels.Location = new System.Drawing.Point(574, 50);
            this.cbChannels.Name = "cbChannels";
            this.cbChannels.Size = new System.Drawing.Size(95, 21);
            this.cbChannels.TabIndex = 20;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(401, 44);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 16;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // rbETS
            // 
            this.rbETS.AutoSize = true;
            this.rbETS.Location = new System.Drawing.Point(247, 71);
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
            this.rbSingle.Location = new System.Drawing.Point(324, 61);
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
            this.rbAuto.Location = new System.Drawing.Point(324, 36);
            this.rbAuto.Name = "rbAuto";
            this.rbAuto.Size = new System.Drawing.Size(47, 17);
            this.rbAuto.TabIndex = 13;
            this.rbAuto.TabStop = true;
            this.rbAuto.Text = "Auto";
            this.rbAuto.UseVisualStyleBackColor = true;
            // 
            // rbRepeat
            // 
            this.rbRepeat.AutoSize = true;
            this.rbRepeat.Location = new System.Drawing.Point(247, 44);
            this.rbRepeat.Name = "rbRepeat";
            this.rbRepeat.Size = new System.Drawing.Size(60, 17);
            this.rbRepeat.TabIndex = 11;
            this.rbRepeat.TabStop = true;
            this.rbRepeat.Text = "Repeat";
            this.rbRepeat.UseVisualStyleBackColor = true;
            // 
            // rbNone
            // 
            this.rbNone.AutoSize = true;
            this.rbNone.Location = new System.Drawing.Point(247, 21);
            this.rbNone.Name = "rbNone";
            this.rbNone.Size = new System.Drawing.Size(51, 17);
            this.rbNone.TabIndex = 10;
            this.rbNone.TabStop = true;
            this.rbNone.Text = "None";
            this.rbNone.UseVisualStyleBackColor = true;
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
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(681, 73);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(46, 13);
            this.label7.TabIndex = 7;
            this.label7.Text = "Voltage:";
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
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(505, 53);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Channels:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 66);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Sampling Rate:";
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Sampling Int:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(10, 131);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(40, 13);
            this.label9.TabIndex = 2;
            this.label9.Text = "Status:";
            // 
            // txtStatus
            // 
            this.txtStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtStatus.Location = new System.Drawing.Point(12, 147);
            this.txtStatus.Multiline = true;
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.Size = new System.Drawing.Size(1018, 544);
            this.txtStatus.TabIndex = 3;
            // 
            // PVLab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1042, 703);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.label9);
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rbETS;
        private System.Windows.Forms.RadioButton rbSingle;
        private System.Windows.Forms.RadioButton rbAuto;
        private System.Windows.Forms.RadioButton rbRepeat;
        private System.Windows.Forms.RadioButton rbNone;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.ComboBox cbRes;
        private System.Windows.Forms.ComboBox cbCoupling;
        private System.Windows.Forms.ComboBox cbVoltage;
        private System.Windows.Forms.ComboBox cbStreaming;
        private System.Windows.Forms.ComboBox cbChannels;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtSamplingInterval;
        private System.Windows.Forms.Button btnStreaming;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtRate;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtStatus;
    }
}

