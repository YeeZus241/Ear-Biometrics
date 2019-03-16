namespace EarBiometric
{
    partial class form_user_identified
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
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panelPIStatus = new System.Windows.Forms.Panel();
            this.pbMedal = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pbProfilePhoto = new System.Windows.Forms.PictureBox();
            this.tbFullName = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.tbRemarksComment = new System.Windows.Forms.TextBox();
            this.labelSignStatus = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.labelCurrentTime = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.timerCloseFUI = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.panel2.SuspendLayout();
            this.panelPIStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbMedal)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbProfilePhoto)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel5.SuspendLayout();
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
            this.panel1.Size = new System.Drawing.Size(700, 43);
            this.panel1.TabIndex = 0;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox2.Image = global::EarBiometric.Properties.Resources.cancel__1_;
            this.pictureBox2.Location = new System.Drawing.Point(663, 9);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(25, 25);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 3;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 14.25F);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(173, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "User Information";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.panelPIStatus);
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Location = new System.Drawing.Point(12, 50);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(269, 326);
            this.panel2.TabIndex = 2;
            // 
            // panelPIStatus
            // 
            this.panelPIStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(89)))), ((int)(((byte)(152)))));
            this.panelPIStatus.Controls.Add(this.pbMedal);
            this.panelPIStatus.Controls.Add(this.label2);
            this.panelPIStatus.Location = new System.Drawing.Point(0, 279);
            this.panelPIStatus.Name = "panelPIStatus";
            this.panelPIStatus.Size = new System.Drawing.Size(269, 35);
            this.panelPIStatus.TabIndex = 1;
            // 
            // pbMedal
            // 
            this.pbMedal.Image = global::EarBiometric.Properties.Resources.medal__3_;
            this.pbMedal.Location = new System.Drawing.Point(22, 3);
            this.pbMedal.Name = "pbMedal";
            this.pbMedal.Size = new System.Drawing.Size(38, 29);
            this.pbMedal.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbMedal.TabIndex = 1;
            this.pbMedal.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(75, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(149, 18);
            this.label2.TabIndex = 2;
            this.label2.Text = "Person Identified";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pbProfilePhoto);
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 9F);
            this.groupBox1.Location = new System.Drawing.Point(4, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(262, 270);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // pbProfilePhoto
            // 
            this.pbProfilePhoto.Image = global::EarBiometric.Properties.Resources.face_man;
            this.pbProfilePhoto.Location = new System.Drawing.Point(6, 14);
            this.pbProfilePhoto.Name = "pbProfilePhoto";
            this.pbProfilePhoto.Size = new System.Drawing.Size(250, 250);
            this.pbProfilePhoto.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbProfilePhoto.TabIndex = 0;
            this.pbProfilePhoto.TabStop = false;
            // 
            // tbFullName
            // 
            this.tbFullName.BackColor = System.Drawing.Color.White;
            this.tbFullName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbFullName.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbFullName.Location = new System.Drawing.Point(6, 21);
            this.tbFullName.Multiline = true;
            this.tbFullName.Name = "tbFullName";
            this.tbFullName.Size = new System.Drawing.Size(356, 27);
            this.tbFullName.TabIndex = 0;
            this.tbFullName.Text = "$USER-NAME";
            this.tbFullName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tbFullName);
            this.groupBox3.Font = new System.Drawing.Font("Verdana", 9F);
            this.groupBox3.Location = new System.Drawing.Point(13, 32);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(368, 54);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.tbRemarksComment);
            this.groupBox4.Controls.Add(this.labelSignStatus);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Font = new System.Drawing.Font("Verdana", 9F);
            this.groupBox4.Location = new System.Drawing.Point(13, 117);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(368, 151);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            // 
            // tbRemarksComment
            // 
            this.tbRemarksComment.Font = new System.Drawing.Font("Verdana", 11.25F);
            this.tbRemarksComment.Location = new System.Drawing.Point(19, 52);
            this.tbRemarksComment.Multiline = true;
            this.tbRemarksComment.Name = "tbRemarksComment";
            this.tbRemarksComment.ReadOnly = true;
            this.tbRemarksComment.Size = new System.Drawing.Size(330, 84);
            this.tbRemarksComment.TabIndex = 10;
            // 
            // labelSignStatus
            // 
            this.labelSignStatus.AutoSize = true;
            this.labelSignStatus.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSignStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(79)))), ((int)(((byte)(86)))));
            this.labelSignStatus.Location = new System.Drawing.Point(97, 18);
            this.labelSignStatus.Name = "labelSignStatus";
            this.labelSignStatus.Size = new System.Drawing.Size(22, 18);
            this.labelSignStatus.TabIndex = 9;
            this.labelSignStatus.Text = "--";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(156, 122);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(0, 14);
            this.label8.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(79)))), ((int)(((byte)(86)))));
            this.label3.Location = new System.Drawing.Point(27, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 18);
            this.label3.TabIndex = 0;
            this.label3.Text = "Status:";
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.White;
            this.panel4.Controls.Add(this.panel6);
            this.panel4.Controls.Add(this.groupBox4);
            this.panel4.Controls.Add(this.panel5);
            this.panel4.Controls.Add(this.button1);
            this.panel4.Controls.Add(this.groupBox3);
            this.panel4.Location = new System.Drawing.Point(294, 49);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(394, 327);
            this.panel4.TabIndex = 3;
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(160)))), ((int)(((byte)(133)))));
            this.panel6.Controls.Add(this.labelCurrentTime);
            this.panel6.Controls.Add(this.label7);
            this.panel6.Location = new System.Drawing.Point(13, 92);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(368, 29);
            this.panel6.TabIndex = 6;
            // 
            // labelCurrentTime
            // 
            this.labelCurrentTime.AutoSize = true;
            this.labelCurrentTime.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCurrentTime.ForeColor = System.Drawing.Color.White;
            this.labelCurrentTime.Location = new System.Drawing.Point(96, 6);
            this.labelCurrentTime.Name = "labelCurrentTime";
            this.labelCurrentTime.Size = new System.Drawing.Size(155, 18);
            this.labelCurrentTime.TabIndex = 1;
            this.labelCurrentTime.Text = "$load_current_time";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(16, 6);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(74, 18);
            this.label7.TabIndex = 0;
            this.label7.Text = "Remarks";
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(160)))), ((int)(((byte)(133)))));
            this.panel5.Controls.Add(this.label6);
            this.panel5.Location = new System.Drawing.Point(13, 7);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(368, 29);
            this.panel5.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(16, 4);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(52, 18);
            this.label6.TabIndex = 0;
            this.label6.Text = "Name";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(79)))), ((int)(((byte)(86)))));
            this.button1.Location = new System.Drawing.Point(32, 283);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(330, 29);
            this.button1.TabIndex = 4;
            this.button1.Text = "Continue";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // timerCloseFUI
            // 
            this.timerCloseFUI.Interval = 1000;
            this.timerCloseFUI.Tick += new System.EventHandler(this.timerCloseFUI_Tick);
            // 
            // form_user_identified
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(235)))), ((int)(((byte)(238)))));
            this.ClientSize = new System.Drawing.Size(700, 388);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "form_user_identified";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "form_user_identified";
            this.Load += new System.EventHandler(this.form_user_identified_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panelPIStatus.ResumeLayout(false);
            this.panelPIStatus.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbMedal)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbProfilePhoto)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox tbFullName;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox pbProfilePhoto;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panelPIStatus;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label labelCurrentTime;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Timer timerCloseFUI;
        private System.Windows.Forms.Label labelSignStatus;
        private System.Windows.Forms.PictureBox pbMedal;
        private System.Windows.Forms.TextBox tbRemarksComment;
    }
}