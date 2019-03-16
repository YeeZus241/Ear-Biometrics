using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;
using Emgu.CV;
using Emgu.CV.Structure;
using System.IO;
namespace EarBiometric
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        // initialize data for draggable form
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        //initialize classes
        global_arguments ga = new global_arguments();
        sql_connection sc = new sql_connection();
        ear_process ep = new ear_process();
        public administrator_login _form_login;
        FormReactor _reactor = new FormReactor();
        public string session_admin_id = "5";
        private void tester()
        {

        }
        // build notification for user auto logged out when it is overtime
        private void loadAdmindata()
        {
            try
            {
                sc.open();
                sc.command = sc.SQLCommand("SELECT first_name, last_name FROM administrators WHERE admin_id = '" + session_admin_id + "'");
                sc.reader = sc.command.ExecuteReader();
                while (sc.reader.Read())
                {
                    lblAdminName.Text = Convert.ToString(sc.reader["first_name"] + " " + sc.reader["last_name"]);
                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sc.close();
            }
        }
        private void initializeButtonStyles()
        {
            button5.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);
            button6.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);
            button5.FlatAppearance.BorderSize = 0;
            button6.FlatAppearance.BorderSize = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tester();
            loadAdmindata();
            label2.Text = DateTime.Now.ToString("MMMM d yyyy");
            initializeButtonStyles();
            this.MouseClick += landLeftClickEvent;
           
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            manage_users mu = new manage_users();
            mu.home = this;
            this.Hide();
            mu.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ear_scanner es = new ear_scanner();
            es._home = this;
            this.Hide();
            es.Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblGlobalTime.Text = DateTime.Now.ToString("h:mm tt");
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            FormDeparments fd = new FormDeparments();
            fd.home = this;
            this.Hide();
            fd.Show();
        }
        private void openFormSettings()
        {
            FormSettings fs = new FormSettings();
            fs._home = this;
            fs.initializeAdminInformation(session_admin_id);
            this.Hide();
            fs.Show();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            openFormSettings();
        }

        private void hoverInChangeButtonStyle(Button b)
        {
         //   b.BackColor = Color.FromArgb(46, 204, 113);
        }
        private void hoverOutChangeButtonStyle(Button b)
        {
         //   b.BackColor = Color.FromArgb(88, 88, 88);
        }
        private void button2_MouseHover(object sender, EventArgs e)
        {
            hoverInChangeButtonStyle(button2);
        }

        private void button1_MouseHover(object sender, EventArgs e)
        {
            hoverInChangeButtonStyle(button1);
        }

        private void button3_MouseHover(object sender, EventArgs e)
        {
            hoverInChangeButtonStyle(button3);
        }

        private void button4_MouseHover(object sender, EventArgs e)
        {
            hoverInChangeButtonStyle(button4);
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            hoverOutChangeButtonStyle(button2);
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            hoverOutChangeButtonStyle(button1);
        }

        private void button3_MouseLeave(object sender, EventArgs e)
        {
            hoverOutChangeButtonStyle(button3);
        }

        private void button4_MouseLeave(object sender, EventArgs e)
        {
            hoverOutChangeButtonStyle(button4);
        }
        private bool _p_settings_altername = true;
       
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (_p_settings_altername) {
                panelSettings.Visible = true;
                _p_settings_altername = false;
            }
            else
            {
                panelSettings.Visible = false;
                _p_settings_altername = true;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            _form_login.Show();
            panelSettings.Visible = false;
            this.Close();
        }
        private void hidePanelSettings()
        {
            panelSettings.Visible = false;
            _p_settings_altername = true;
        }
        private void button5_Click(object sender, EventArgs e)
        {
            openFormSettings();
        }
        private void landLeftClickEvent(object sender, MouseEventArgs e)
        {
            hidePanelSettings();
        }

        private void pictureBox2_MouseHover(object sender, EventArgs e)
        {
        }

        private void button7_Click(object sender, EventArgs e)
        {
            FormEarProcess fep = new FormEarProcess();
            this.Hide();
            fep._home = this;
            fep.Show();
        }
    }
}