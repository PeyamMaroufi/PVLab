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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtError = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtTriggerAT = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbDirection = new System.Windows.Forms.ComboBox();
            this.lbPoints = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnStream = new System.Windows.Forms.Button();
            this.rbSingle = new System.Windows.Forms.RadioButton();
            this.rbRepeat = new System.Windows.Forms.RadioButton();
            this.rbNone = new System.Windows.Forms.RadioButton();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.plotView1 = new OxyPlot.WindowsForms.PlotView();
            this.chbDetaljs = new System.Windows.Forms.CheckBox();
            this.btnDrop = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnDrop);
            this.groupBox1.Controls.Add(this.chbDetaljs);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtError);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtTriggerAT);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cbDirection);
            this.groupBox1.Controls.Add(this.lbPoints);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btnStream);
            this.groupBox1.Controls.Add(this.rbSingle);
            this.groupBox1.Controls.Add(this.rbRepeat);
            this.groupBox1.Controls.Add(this.rbNone);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1024, 91);
            this.groupBox1.TabIndex = 23;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Control";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(618, 32);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 37;
            this.label4.Text = "Error: ";
            // 
            // txtError
            // 
            this.txtError.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtError.Location = new System.Drawing.Point(676, 28);
            this.txtError.Name = "txtError";
            this.txtError.Size = new System.Drawing.Size(100, 20);
            this.txtError.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(618, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 35;
            this.label3.Text = "Triger at: ";
            // 
            // txtTriggerAT
            // 
            this.txtTriggerAT.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtTriggerAT.Location = new System.Drawing.Point(676, 55);
            this.txtTriggerAT.Name = "txtTriggerAT";
            this.txtTriggerAT.Size = new System.Drawing.Size(100, 20);
            this.txtTriggerAT.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(821, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 33;
            this.label1.Text = "Direction: ";
            // 
            // cbDirection
            // 
            this.cbDirection.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbDirection.FormattingEnabled = true;
            this.cbDirection.ItemHeight = 13;
            this.cbDirection.Location = new System.Drawing.Point(894, 28);
            this.cbDirection.Name = "cbDirection";
            this.cbDirection.Size = new System.Drawing.Size(98, 21);
            this.cbDirection.TabIndex = 8;
            // 
            // lbPoints
            // 
            this.lbPoints.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbPoints.AutoSize = true;
            this.lbPoints.Location = new System.Drawing.Point(81, 32);
            this.lbPoints.Name = "lbPoints";
            this.lbPoints.Size = new System.Drawing.Size(0, 13);
            this.lbPoints.TabIndex = 31;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 30;
            this.label2.Text = "Points:";
            // 
            // btnStream
            // 
            this.btnStream.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnStream.Location = new System.Drawing.Point(115, 28);
            this.btnStream.Name = "btnStream";
            this.btnStream.Size = new System.Drawing.Size(121, 23);
            this.btnStream.TabIndex = 1;
            this.btnStream.Text = "Start";
            this.btnStream.UseVisualStyleBackColor = true;
            this.btnStream.Click += new System.EventHandler(this.btnStream_Click);
            // 
            // rbSingle
            // 
            this.rbSingle.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.rbSingle.AutoSize = true;
            this.rbSingle.Location = new System.Drawing.Point(440, 30);
            this.rbSingle.Name = "rbSingle";
            this.rbSingle.Size = new System.Drawing.Size(54, 17);
            this.rbSingle.TabIndex = 4;
            this.rbSingle.TabStop = true;
            this.rbSingle.Text = "Single";
            this.rbSingle.UseVisualStyleBackColor = true;
            this.rbSingle.CheckedChanged += new System.EventHandler(this.rbSingle_CheckedChanged);
            // 
            // rbRepeat
            // 
            this.rbRepeat.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.rbRepeat.AutoSize = true;
            this.rbRepeat.Location = new System.Drawing.Point(283, 30);
            this.rbRepeat.Name = "rbRepeat";
            this.rbRepeat.Size = new System.Drawing.Size(60, 17);
            this.rbRepeat.TabIndex = 2;
            this.rbRepeat.TabStop = true;
            this.rbRepeat.Text = "Repeat";
            this.rbRepeat.UseVisualStyleBackColor = true;
            this.rbRepeat.CheckedChanged += new System.EventHandler(this.rbRepeat_CheckedChanged);
            // 
            // rbNone
            // 
            this.rbNone.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.rbNone.AutoSize = true;
            this.rbNone.Location = new System.Drawing.Point(367, 30);
            this.rbNone.Name = "rbNone";
            this.rbNone.Size = new System.Drawing.Size(51, 17);
            this.rbNone.TabIndex = 3;
            this.rbNone.TabStop = true;
            this.rbNone.Text = "None";
            this.rbNone.UseVisualStyleBackColor = true;
            this.rbNone.CheckedChanged += new System.EventHandler(this.rbNone_CheckedChanged);
            // 
            // txtStatus
            // 
            this.txtStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtStatus.Location = new System.Drawing.Point(795, 110);
            this.txtStatus.Multiline = true;
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.ReadOnly = true;
            this.txtStatus.Size = new System.Drawing.Size(242, 584);
            this.txtStatus.TabIndex = 9;
            // 
            // plotView1
            // 
            this.plotView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.plotView1.Location = new System.Drawing.Point(13, 110);
            this.plotView1.Name = "plotView1";
            this.plotView1.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.plotView1.Size = new System.Drawing.Size(776, 584);
            this.plotView1.TabIndex = 26;
            this.plotView1.Text = "plotView1";
            this.plotView1.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.plotView1.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.plotView1.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // chbDetaljs
            // 
            this.chbDetaljs.AutoSize = true;
            this.chbDetaljs.Location = new System.Drawing.Point(283, 57);
            this.chbDetaljs.Name = "chbDetaljs";
            this.chbDetaljs.Size = new System.Drawing.Size(165, 17);
            this.chbDetaljs.TabIndex = 5;
            this.chbDetaljs.Text = "Show detals in Repeat trigger";
            this.chbDetaljs.UseVisualStyleBackColor = true;
            // 
            // btnDrop
            // 
            this.btnDrop.Location = new System.Drawing.Point(115, 54);
            this.btnDrop.Name = "btnDrop";
            this.btnDrop.Size = new System.Drawing.Size(121, 23);
            this.btnDrop.TabIndex = 38;
            this.btnDrop.Text = "Drop Device";
            this.btnDrop.UseVisualStyleBackColor = true;
            this.btnDrop.Click += new System.EventHandler(this.btnDrop_Click);
            // 
            // Plot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1049, 706);
            this.Controls.Add(this.plotView1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtStatus);
            this.Name = "Plot";
            this.Text = "Plot";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnStream;
        private System.Windows.Forms.RadioButton rbSingle;
        private System.Windows.Forms.RadioButton rbRepeat;
        private System.Windows.Forms.RadioButton rbNone;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbPoints;
        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.ComboBox cbDirection;
        private System.Windows.Forms.Label label1;
        private OxyPlot.WindowsForms.PlotView plotView1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtTriggerAT;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtError;
        private System.Windows.Forms.CheckBox chbDetaljs;
        private System.Windows.Forms.Button btnDrop;
    }
}