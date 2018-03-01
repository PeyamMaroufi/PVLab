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
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnStreaming = new System.Windows.Forms.Button();
            this.txtSamplingInterval = new System.Windows.Forms.TextBox();
            this.cbRes = new System.Windows.Forms.ComboBox();
            this.cbCoupling = new System.Windows.Forms.ComboBox();
            this.cbVoltage = new System.Windows.Forms.ComboBox();
            this.cbChannels = new System.Windows.Forms.ComboBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.txtProperty = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btnOpen);
            this.panel1.Controls.Add(this.btnStreaming);
            this.panel1.Controls.Add(this.txtSamplingInterval);
            this.panel1.Controls.Add(this.cbRes);
            this.panel1.Controls.Add(this.cbCoupling);
            this.panel1.Controls.Add(this.cbVoltage);
            this.panel1.Controls.Add(this.cbChannels);
            this.panel1.Controls.Add(this.btnStart);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(12, 13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1018, 113);
            this.panel1.TabIndex = 0;
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
            this.txtSamplingInterval.Location = new System.Drawing.Point(175, 45);
            this.txtSamplingInterval.Name = "txtSamplingInterval";
            this.txtSamplingInterval.Size = new System.Drawing.Size(95, 20);
            this.txtSamplingInterval.TabIndex = 25;
            // 
            // cbRes
            // 
            this.cbRes.FormattingEnabled = true;
            this.cbRes.Location = new System.Drawing.Point(871, 63);
            this.cbRes.Name = "cbRes";
            this.cbRes.Size = new System.Drawing.Size(95, 21);
            this.cbRes.TabIndex = 24;
            this.cbRes.SelectedIndexChanged += new System.EventHandler(this.cbRes_SelectedIndexChanged);
            // 
            // cbCoupling
            // 
            this.cbCoupling.FormattingEnabled = true;
            this.cbCoupling.Location = new System.Drawing.Point(871, 30);
            this.cbCoupling.Name = "cbCoupling";
            this.cbCoupling.Size = new System.Drawing.Size(95, 21);
            this.cbCoupling.TabIndex = 23;
            // 
            // cbVoltage
            // 
            this.cbVoltage.FormattingEnabled = true;
            this.cbVoltage.Location = new System.Drawing.Point(673, 64);
            this.cbVoltage.Name = "cbVoltage";
            this.cbVoltage.Size = new System.Drawing.Size(95, 21);
            this.cbVoltage.TabIndex = 22;
            // 
            // cbChannels
            // 
            this.cbChannels.FormattingEnabled = true;
            this.cbChannels.Location = new System.Drawing.Point(675, 30);
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
            this.btnStart.Text = "Record";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(794, 33);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(51, 13);
            this.label8.TabIndex = 8;
            this.label8.Text = "Coupling:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(601, 68);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(46, 13);
            this.label7.TabIndex = 7;
            this.label7.Text = "Voltage:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(601, 34);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Channels:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(794, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Resolution:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(41, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Capturing Interval Interval";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(9, 401);
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
            this.txtStatus.Location = new System.Drawing.Point(12, 324);
            this.txtStatus.Multiline = true;
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.ReadOnly = true;
            this.txtStatus.Size = new System.Drawing.Size(1018, 259);
            this.txtStatus.TabIndex = 3;
            // 
            // txtProperty
            // 
            this.txtProperty.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProperty.Location = new System.Drawing.Point(12, 155);
            this.txtProperty.Multiline = true;
            this.txtProperty.Name = "txtProperty";
            this.txtProperty.ReadOnly = true;
            this.txtProperty.Size = new System.Drawing.Size(1018, 163);
            this.txtProperty.TabIndex = 4;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 139);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(106, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Sampling Properties: ";
            // 
            // PVLab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1042, 595);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtProperty);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.panel1);
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
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.ComboBox cbRes;
        private System.Windows.Forms.ComboBox cbCoupling;
        private System.Windows.Forms.ComboBox cbVoltage;
        private System.Windows.Forms.ComboBox cbChannels;
        private System.Windows.Forms.TextBox txtSamplingInterval;
        private System.Windows.Forms.Button btnStreaming;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.TextBox txtProperty;
        private System.Windows.Forms.Label label6;
    }
}

