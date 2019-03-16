namespace EarBiometric
{
    partial class ear_capture
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
            this.tbEarName = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pbEarCaptured = new System.Windows.Forms.PictureBox();
            this.button2 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ofdUploadEarImage = new System.Windows.Forms.OpenFileDialog();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.pbCameraFrame = new System.Windows.Forms.PictureBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.panel10 = new System.Windows.Forms.Panel();
            this.pbUploadEarImage = new System.Windows.Forms.PictureBox();
            this.button4 = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel11 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.panel12 = new System.Windows.Forms.Panel();
            this.tbRecognizeDelay = new System.Windows.Forms.TextBox();
            this.tbScanStatus = new System.Windows.Forms.TextBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.tbErrorsAttempt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.timerClearBufferedData = new System.Windows.Forms.Timer(this.components);
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel9 = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.timerFormCloser = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbEarCaptured)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCameraFrame)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbUploadEarImage)).BeginInit();
            this.panel4.SuspendLayout();
            this.panel11.SuspendLayout();
            this.panel12.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel9.SuspendLayout();
            this.panel8.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbEarName
            // 
            this.tbEarName.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbEarName.Location = new System.Drawing.Point(6, 16);
            this.tbEarName.Name = "tbEarName";
            this.tbEarName.Size = new System.Drawing.Size(250, 26);
            this.tbEarName.TabIndex = 7;
            this.tbEarName.Text = "unnamed";
            this.tbEarName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pbEarCaptured);
            this.groupBox1.Location = new System.Drawing.Point(11, 42);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(262, 294);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // pbEarCaptured
            // 
            this.pbEarCaptured.Location = new System.Drawing.Point(6, 12);
            this.pbEarCaptured.Name = "pbEarCaptured";
            this.pbEarCaptured.Size = new System.Drawing.Size(250, 275);
            this.pbEarCaptured.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbEarCaptured.TabIndex = 1;
            this.pbEarCaptured.TabStop = false;
            this.pbEarCaptured.Click += new System.EventHandler(this.pbEarCaptured_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            this.button2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(79)))), ((int)(((byte)(86)))));
            this.button2.Location = new System.Drawing.Point(6, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(250, 35);
            this.button2.TabIndex = 3;
            this.button2.Text = "Add";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(160)))), ((int)(((byte)(133)))));
            this.panel1.Controls.Add(this.pictureBox2);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(792, 43);
            this.panel1.TabIndex = 1;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox2.Image = global::EarBiometric.Properties.Resources.cancel__1_;
            this.pictureBox2.Location = new System.Drawing.Point(753, 9);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(25, 25);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 3;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Verdana", 14.25F);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 23);
            this.label2.TabIndex = 0;
            this.label2.Text = "Ear Capture";
            // 
            // ofdUploadEarImage
            // 
            this.ofdUploadEarImage.FileName = "openFileDialog1";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.pbCameraFrame);
            this.groupBox3.Location = new System.Drawing.Point(12, 42);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(452, 352);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            // 
            // pbCameraFrame
            // 
            this.pbCameraFrame.Location = new System.Drawing.Point(6, 13);
            this.pbCameraFrame.Name = "pbCameraFrame";
            this.pbCameraFrame.Size = new System.Drawing.Size(440, 333);
            this.pbCameraFrame.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbCameraFrame.TabIndex = 0;
            this.pbCameraFrame.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tbEarName);
            this.groupBox2.Location = new System.Drawing.Point(11, 342);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(262, 52);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Ear Name";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.panel7);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.groupBox3);
            this.panel2.Location = new System.Drawing.Point(12, 49);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(476, 447);
            this.panel2.TabIndex = 2;
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(235)))), ((int)(((byte)(238)))));
            this.panel7.Controls.Add(this.panel10);
            this.panel7.Controls.Add(this.button4);
            this.panel7.Location = new System.Drawing.Point(12, 400);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(452, 41);
            this.panel7.TabIndex = 12;
            // 
            // panel10
            // 
            this.panel10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.panel10.Controls.Add(this.pbUploadEarImage);
            this.panel10.Location = new System.Drawing.Point(405, 3);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(41, 35);
            this.panel10.TabIndex = 5;
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
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.button4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.ForeColor = System.Drawing.Color.White;
            this.button4.Location = new System.Drawing.Point(6, 3);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(393, 35);
            this.button4.TabIndex = 1;
            this.button4.Text = "Scan";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(160)))), ((int)(((byte)(133)))));
            this.panel4.Controls.Add(this.panel11);
            this.panel4.Controls.Add(this.tbScanStatus);
            this.panel4.Controls.Add(this.panel5);
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(476, 34);
            this.panel4.TabIndex = 11;
            // 
            // panel11
            // 
            this.panel11.BackColor = System.Drawing.Color.Gray;
            this.panel11.Controls.Add(this.label4);
            this.panel11.Controls.Add(this.panel12);
            this.panel11.Location = new System.Drawing.Point(147, 0);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(112, 34);
            this.panel11.TabIndex = 9;
            this.panel11.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Verdana", 9F);
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(7, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 14);
            this.label4.TabIndex = 8;
            this.label4.Text = "Delay:";
            // 
            // panel12
            // 
            this.panel12.BackColor = System.Drawing.Color.DimGray;
            this.panel12.Controls.Add(this.tbRecognizeDelay);
            this.panel12.Location = new System.Drawing.Point(60, 0);
            this.panel12.Name = "panel12";
            this.panel12.Size = new System.Drawing.Size(52, 34);
            this.panel12.TabIndex = 7;
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
            this.panel5.Controls.Add(this.label1);
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(141, 34);
            this.panel5.TabIndex = 4;
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.DimGray;
            this.panel6.Controls.Add(this.tbErrorsAttempt);
            this.panel6.Location = new System.Drawing.Point(89, 0);
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(7, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 14);
            this.label1.TabIndex = 3;
            this.label1.Text = "Ear Frame:";
            // 
            // timerClearBufferedData
            // 
            this.timerClearBufferedData.Interval = 500;
            this.timerClearBufferedData.Tick += new System.EventHandler(this.timerClearBufferedData_Tick);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.Controls.Add(this.panel9);
            this.panel3.Controls.Add(this.panel8);
            this.panel3.Controls.Add(this.groupBox1);
            this.panel3.Controls.Add(this.groupBox2);
            this.panel3.Location = new System.Drawing.Point(494, 49);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(284, 447);
            this.panel3.TabIndex = 8;
            // 
            // panel9
            // 
            this.panel9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(235)))), ((int)(((byte)(238)))));
            this.panel9.Controls.Add(this.button2);
            this.panel9.Location = new System.Drawing.Point(11, 400);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(262, 41);
            this.panel9.TabIndex = 9;
            // 
            // panel8
            // 
            this.panel8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(160)))), ((int)(((byte)(133)))));
            this.panel8.Controls.Add(this.label3);
            this.panel8.Location = new System.Drawing.Point(0, 0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(284, 34);
            this.panel8.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(8, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 18);
            this.label3.TabIndex = 0;
            this.label3.Text = "Ear Captured";
            // 
            // timerFormCloser
            // 
            this.timerFormCloser.Interval = 1000;
            this.timerFormCloser.Tick += new System.EventHandler(this.timerFormCloser_Tick);
            // 
            // ear_capture
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(235)))), ((int)(((byte)(238)))));
            this.ClientSize = new System.Drawing.Size(792, 508);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ear_capture";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ear_capture";
            this.Load += new System.EventHandler(this.ear_capture_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbEarCaptured)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbCameraFrame)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel10.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbUploadEarImage)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel11.ResumeLayout(false);
            this.panel11.PerformLayout();
            this.panel12.ResumeLayout(false);
            this.panel12.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox pbEarCaptured;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbEarName;
        private System.Windows.Forms.OpenFileDialog ofdUploadEarImage;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.PictureBox pbCameraFrame;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TextBox tbScanStatus;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.TextBox tbErrorsAttempt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Timer timerClearBufferedData;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.PictureBox pbUploadEarImage;
        private System.Windows.Forms.Timer timerFormCloser;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel12;
        private System.Windows.Forms.TextBox tbRecognizeDelay;
    }
}