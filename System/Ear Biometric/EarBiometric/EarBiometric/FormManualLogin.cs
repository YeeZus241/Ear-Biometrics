using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EarBiometric
{
    public partial class FormManualLogin : Form
    {
        public FormManualLogin()
        {
            InitializeComponent();
        }
        // initializa data for draggable form
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        global_arguments ga = new global_arguments();
        sql_connection sc = new sql_connection();
        public ear_scanner es;
        FormReactor _reactor = new FormReactor();
        form_user_identified _fui = new form_user_identified();
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            ga.invokeShowForm(es);
            this.Close();
        }

        private void untouchableKeyElements()
        {
            tbUserID.Enabled = false;
            tbUserPassword.Enabled = false;
            btnLogin.Enabled = false;
            btnLogin.Text = "Logging in...";
        }
        private void touchableKeyElements()
        {
            tbUserID.Enabled = true;
            tbUserPassword.Enabled = true;
            btnLogin.Enabled = true;
            btnLogin.Text = "Login";
        }

        private void login()
        {
            string user_id = tbUserID.Text;
            string password = ga.toMD5(tbUserPassword.Text);
            if (user_id == "" || password == "" || !ga.filterString3(user_id))
            {
                _reactor.showReactor(false, "Please enter a valid ID and Password");
            }
            else
            {
                untouchableKeyElements();
                try
                {
                    sc.open();
                    sc.command = sc.SQLCommand("SELECT user_id FROM users WHERE (user_id = '" + user_id + "' OR username = '" + user_id + "') AND password = '" + password + "'");
                    sc.reader = sc.command.ExecuteReader();
                    if (sc.reader.Read())
                    {
                        tbUserID.Clear();
                        tbUserPassword.Clear();
                        _fui.processUserIdentification(Convert.ToString(sc.reader["user_id"]), this);
                    }
                    else
                    {
                        _reactor.showReactor(false, "Invalid login credentials");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    sc.close();
                    touchableKeyElements();
                }
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            login();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                tbUserPassword.UseSystemPasswordChar = false;
            }
            else
            {
                tbUserPassword.UseSystemPasswordChar = true;
            }
        }
    }
}
