namespace PVLab
{
    partial class Plot
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
            this.plotView1 = new OxyPlot.WindowsForms.PlotView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnStream = new System.Windows.Forms.Button();
            this.rbETS = new System.Windows.Forms.RadioButton();
            this.rbSingle = new System.Windows.Forms.RadioButton();
            this.rbAuto = new System.Windows.Forms.RadioButton();
            this.rbRepeat = new System.Windows.Forms.RadioButton();
            this.rbNone = new System.Windows.Forms.RadioButton();
            this.cbDirection = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // plotView1
            // 
            this.plotView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.plotView1.Location = new System.Drawing.Point(12, 110);
            this.plotView1.Name = "plotView1";
            this.plotView1.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.plotView1.Size = new System.Drawing.Size(1025, 584);
            this.plotView1.TabIndex = 22;
            this.plotView1.Text = "plotView1";
            this.plotView1.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.plotView1.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.plotView1.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cbDirection);
            this.groupBox1.Controls.Add(this.btnStream);
            this.groupBox1.Controls.Add(this.rbETS);
            this.groupBox1.Controls.Add(this.rbSingle);
            this.groupBox1.Controls.Add(this.rbAuto);
            this.groupBox1.Controls.Add(this.rbRepeat);
            this.groupBox1.Controls.Add(this.rbNone);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1024, 91);
            this.groupBox1.TabIndex = 23;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Control";
            // 
            // btnStream
            // 
            this.btnStream.Location = new System.Drawing.Point(562, 33);
            this.btnStream.Name = "btnStream";
            this.btnStream.Size = new System.Drawing.Size(121, 21);
            this.btnStream.TabIndex = 27;
            this.btnStream.Text = "Start";
            this.btnStream.UseVisualStyleBackColor = true;
            // 
            // rbETS
            // 
            this.rbETS.AutoSize = true;
            this.rbETS.Location = new System.Drawing.Point(414, 35);
            this.rbETS.Name = "rbETS";
            this.rbETS.Size = new System.Drawing.Size(46, 17);
            this.rbETS.TabIndex = 26;
            this.rbETS.TabStop = true;
            this.rbETS.Text = "ETS";
            this.rbETS.UseVisualStyleBackColor = true;
            // 
            // rbSingle
            // 
            this.rbSingle.AutoSize = true;
            this.rbSingle.Location = new System.Drawing.Point(484, 35);
            this.rbSingle.Name = "rbSingle";
            this.rbSingle.Size = new System.Drawing.Size(54, 17);
            this.rbSingle.TabIndex = 25;
            this.rbSingle.TabStop = true;
            this.rbSingle.Text = "Single";
            this.rbSingle.UseVisualStyleBackColor = true;
            // 
            // rbAuto
            // 
            this.rbAuto.AutoSize = true;
            this.rbAuto.Location = new System.Drawing.Point(343, 35);
            this.rbAuto.Name = "rbAuto";
            this.rbAuto.Size = new System.Drawing.Size(47, 17);
            this.rbAuto.TabIndex = 24;
            this.rbAuto.TabStop = true;
            this.rbAuto.Text = "Auto";
            this.rbAuto.UseVisualStyleBackColor = true;
            // 
            // rbRepeat
            // 
            this.rbRepeat.AutoSize = true;
            this.rbRepeat.Location = new System.Drawing.Point(184, 35);
            this.rbRepeat.Name = "rbRepeat";
            this.rbRepeat.Size = new System.Drawing.Size(60, 17);
            this.rbRepeat.TabIndex = 23;
            this.rbRepeat.TabStop = true;
            this.rbRepeat.Text = "Repeat";
            this.rbRepeat.UseVisualStyleBackColor = true;
            // 
            // rbNone
            // 
            this.rbNone.AutoSize = true;
            this.rbNone.Location = new System.Drawing.Point(268, 35);
            this.rbNone.Name = "rbNone";
            this.rbNone.Size = new System.Drawing.Size(51, 17);
            this.rbNone.TabIndex = 22;
            this.rbNone.TabStop = true;
            this.rbNone.Text = "None";
            this.rbNone.UseVisualStyleBackColor = true;
            // 
            // cbDirection
            // 
            this.cbDirection.FormattingEnabled = true;
            this.cbDirection.Location = new System.Drawing.Point(799, 33);
            this.cbDirection.Name = "cbDirection";
            this.cbDirection.Size = new System.Drawing.Size(121, 21);
            this.cbDirection.TabIndex = 28;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(707, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 29;
            this.label1.Text = "Ratio Mode: ";
            // 
            // Plot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1049, 706);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.plotView1);
            this.Name = "Plot";
            this.Text = "Plot";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private OxyPlot.WindowsForms.PlotView plotView1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnStream;
        private System.Windows.Forms.RadioButton rbETS;
        private System.Windows.Forms.RadioButton rbSingle;
        private System.Windows.Forms.RadioButton rbAuto;
        private System.Windows.Forms.RadioButton rbRepeat;
        private System.Windows.Forms.RadioButton rbNone;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbDirection;
    }
}