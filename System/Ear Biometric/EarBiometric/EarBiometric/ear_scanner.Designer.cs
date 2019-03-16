namespace EarBiometric
{
    partial class ear_scanner
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
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel9 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.panel8 = new System.Windows.Forms.Panel();
            this.tbRecognizeDelay = new System.Windows.Forms.TextBox();
            this.tbScanStatus = new System.Windows.Forms.TextBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.tbErrorsAttempt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel10 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.timerClearBufferedData = new System.Windows.Forms.Timer(this.components);
            this.ofdUploadEarImage = new System.Windows.Forms.OpenFileDialog();
            this.timerFormCloser = new System.Windows.Forms.Timer(this.components);
            this.panel11 = new System.Windows.Forms.Panel();
            this.pbManualLogin = new System.Windows.Forms.PictureBox();
            this.pbAverageRecognition = new System.Windows.Forms.PictureBox();
            this.pbUploadEarImage = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel9.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel10.SuspendLayout();
            this.panel7.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel11.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbManualLogin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAverageRecognition)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbUploadEarImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(160)))), ((int)(((byte)(133)))));
            this.panel1.Controls.Add(this.pictureBox2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(500, 43);
            this.panel1.TabIndex = 0;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 14.25F);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "Ear Scanner";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Location = new System.Drawing.Point(12, 49);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(476, 447);
            this.panel2.TabIndex = 1;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(160)))), ((int)(((byte)(133)))));
            this.panel4.Controls.Add(this.panel9);
            this.panel4.Controls.Add(this.tbScanStatus);
            this.panel4.Controls.Add(this.panel5);
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(476, 34);
            this.panel4.TabIndex = 2;
            // 
            // panel9
            // 
            this.panel9.BackColor = System.Drawing.Color.Gray;
            this.panel9.Controls.Add(this.label3);
            this.panel9.Controls.Add(this.panel8);
            this.panel9.Location = new System.Drawing.Point(131, 0);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(112, 34);
            this.panel9.TabIndex = 8;
            this.panel9.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Verdana", 9F);
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(7, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 14);
            this.label3.TabIndex = 8;
            this.label3.Text = "Delay:";
            // 
            // panel8
            // 
            this.panel8.BackColor = System.Drawing.Color.DimGray;
            this.panel8.Controls.Add(this.tbRecognizeDelay);
            this.panel8.Location = new System.Drawing.Point(60, 0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(52, 34);
            this.panel8.TabIndex = 7;
            // 
            // tbRecognizeDelay
            // 
            this.tbRecognizeDelay.BackColor = System.Drawing.Color.DimGray;
            this.tbRecognizeDelay.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbRecognizeDelay.Font = new System.Drawing.Font("Verdana", 9F);
            this.tbRecognizeDelay.ForeColor = System.Drawing.Color.White;
            this.tbRecognizeDelay.Location = new System.Drawing.Point(4, 10);
            this.tbRecognizeDelay.Name = "tbRecognizeDelay";
            this.tbRecognizeDelay.ReadOnly = true;
            this.tbRecognizeDelay.Size = new System.Drawing.Size(45, 15);
            this.tbRecognizeDelay.TabIndex = 1;
            this.tbRecognizeDelay.Text = "0";
            this.tbRecognizeDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbScanStatus
            // 
            this.tbScanStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(160)))), ((int)(((byte)(133)))));
            this.tbScanStatus.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbScanStatus.Font = new System.Drawing.Font("Verdana", 9F);
            this.tbScanStatus.ForeColor = System.Drawing.Color.White;
            this.tbScanStatus.Location = new System.Drawing.Point(289, 9);
            this.tbScanStatus.Name = "tbScanStatus";
            this.tbScanStatus.Size = new System.Drawing.Size(175, 15);
            this.tbScanStatus.TabIndex = 6;
            this.tbScanStatus.Text = "IDLE";
            this.tbScanStatus.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.Gray;
            this.panel5.Controls.Add(this.panel6);
            this.panel5.Controls.Add(this.label2);
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(125, 34);
            this.panel5.TabIndex = 4;
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.DimGray;
            this.panel6.Controls.Add(this.tbErrorsAttempt);
            this.panel6.Location = new System.Drawing.Point(73, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(52, 34);
            this.panel6.TabIndex = 5;
            // 
            // tbErrorsAttempt
            // 
            this.tbErrorsAttempt.BackColor = System.Drawing.Color.DimGray;
            this.tbErrorsAttempt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbErrorsAttempt.Font = new System.Drawing.Font("Verdana", 9F);
            this.tbErrorsAttempt.ForeColor = System.Drawing.Color.White;
            this.tbErrorsAttempt.Location = new System.Drawing.Point(3, 10);
            this.tbErrorsAttempt.Name = "tbErrorsAttempt";
            this.tbErrorsAttempt.ReadOnly = true;
            this.tbErrorsAttempt.Size = new System.Drawing.Size(45, 15);
            this.tbErrorsAttempt.TabIndex = 0;
            this.tbErrorsAttempt.Text = "0";
            this.tbErrorsAttempt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(7, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 14);
            this.label2.TabIndex = 3;
            this.label2.Text = "Attempt:";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(235)))), ((int)(((byte)(238)))));
            this.panel3.Controls.Add(this.panel11);
            this.panel3.Controls.Add(this.panel10);
            this.panel3.Controls.Add(this.panel7);
            this.panel3.Controls.Add(this.button2);
            this.panel3.Location = new System.Drawing.Point(12, 400);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(452, 41);
            this.panel3.TabIndex = 1;
            // 
            // panel10
            // 
            this.panel10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.panel10.Controls.Add(this.pbAverageRecognition);
            this.panel10.Location = new System.Drawing.Point(310, 3);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(41, 35);
            this.panel10.TabIndex = 5;
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.panel7.Controls.Add(this.pbUploadEarImage);
            this.panel7.Location = new System.Drawing.Point(405, 3);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(41, 35);
            this.panel7.TabIndex = 4;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.button2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Verdana", 9F);
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Location = new System.Drawing.Point(6, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(298, 35);
            this.button2.TabIndex = 3;
            this.button2.Text = "Scan";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Location = new System.Drawing.Point(12, 42);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(452, 352);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // timerClearBufferedData
            // 
            this.timerClearBufferedData.Interval = 500;
            this.timerClearBufferedData.Tick += new System.EventHandler(this.timerClearBufferedData_Tick);
            // 
            // ofdUploadEarImage
            // 
            this.ofdUploadEarImage.FileName = "openFileDialog1";
            // 
            // timerFormCloser
            // 
            this.timerFormCloser.Interval = 1000;
            this.timerFormCloser.Tick += new System.EventHandler(this.timerFormCloser_Tick);
            // 
            // panel11
            // 
            this.panel11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.panel11.Controls.Add(this.pbManualLogin);
            this.panel11.Location = new System.Drawing.Point(358, 3);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(41, 35);
            this.panel11.TabIndex = 6;
            // 
            // pbManualLogin
            // 
            this.pbManualLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbManualLogin.Image = global::EarBiometric.Properties.Resources.man_with_tie1;
            this.pbManualLogin.Location = new System.Drawing.Point(3, 3);
            this.pbManualLogin.Name = "pbManualLogin";
            this.pbManualLogin.Size = new System.Drawing.Size(35, 29);
            this.pbManualLogin.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbManualLogin.TabIndex = 0;
            this.pbManualLogin.TabStop = false;
            this.pbManualLogin.Click += new System.EventHandler(this.pbManualLogin_Click);
            // 
            // pbAverageRecognition
            // 
            this.pbAverageRecognition.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbAverageRecognition.Image = global::EarBiometric.Properties.Resources.timer;
            this.pbAverageRecognition.Location = new System.Drawing.Point(3, 3);
            this.pbAverageRecognition.Name = "pbAverageRecognition";
            this.pbAverageRecognition.Size = new System.Drawing.Size(35, 29);
            this.pbAverageRecognition.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbAverageRecognition.TabIndex = 0;
            this.pbAverageRecognition.TabStop = false;
            this.pbAverageRecognition.Click += new System.EventHandler(this.pbAverageRecognition_Click);
            // 
            // pbUploadEarImage
            // 
            this.pbUploadEarImage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbUploadEarImage.Image = global::EarBiometric.Properties.Resources.ear_white_64x64;
            this.pbUploadEarImage.Location = new System.Drawing.Point(3, 3);
            this.pbUploadEarImage.Name = "pbUploadEarImage";
            this.pbUploadEarImage.Size = new System.Drawing.Size(35, 29);
            this.pbUploadEarImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbUploadEarImage.TabIndex = 0;
            this.pbUploadEarImage.TabStop = false;
            this.pbUploadEarImage.Click += new System.EventHandler(this.pbUploadEarImage_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(6, 13);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(440, 333);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox2.Image = global::EarBiometric.Properties.Resources.cancel__1_;
            this.pictureBox2.Location = new System.Drawing.Point(463, 9);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(25, 25);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 2;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // ear_scanner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(235)))), ((int)(((byte)(238)))));
            this.ClientSize = new System.Drawing.Size(500, 508);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ear_scanner";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ear_scanner";
            this.Load += new System.EventHandler(this.ear_scanner_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel9.ResumeLayout(false);
            this.panel9.PerformLayout();
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel10.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.panel11.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbManualLogin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAverageRecognition)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbUploadEarImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.TextBox tbErrorsAttempt;
        private System.Windows.Forms.TextBox tbScanStatus;
        private System.Windows.Forms.Timer timerClearBufferedData;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.PictureBox pbUploadEarImage;
        private System.Windows.Forms.OpenFileDialog ofdUploadEarImage;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbRecognizeDelay;
        private System.Windows.Forms.Timer timerFormCloser;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.PictureBox pbAverageRecognition;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.PictureBox pbManualLogin;
    }
}