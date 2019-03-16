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
    public partial class administrator_login : Form
    {
        public administrator_login()
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
        FormReactor _reactor = new FormReactor();
        int _login_attempt_threshold = 3;
        int _login_minutes_threshold = 7;
        private void administrator_login_Load(object sender, EventArgs e)
        {
        }
        private string fetchAdminID(string admin_id)
        {
            string r = "none";
            try
            {
                sc.open();
                sc.command = sc.SQLCommand("SELECT admin_id FROM administrators WHERE (admin_id = '" + admin_id + "' OR username = '" + admin_id + "')");
                sc.reader = sc.command.ExecuteReader();
                if (sc.reader.Read())
                {
                    r = Convert.ToString(sc.reader["admin_id"]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sc.close();
            }
            return r;
        }
        private void invalidLoginCredentials(string admin_id)
        {
            string f_admin_id = fetchAdminID(admin_id);
            if (f_admin_id == "none")
            {
                _reactor.showReactor(false, "Invalid login credentials");
            }
            else
            {
                try
                {
                    sc.open();
                    sc.command = sc.SQLCommand("INSERT INTO administrators_login_attempt(admin_id, date_attempt, status) VALUES('" + f_admin_id + "', '" + ga.toSQLDateTime() + "','locked')");
                    if (sc.command.ExecuteNonQuery() > 0)
                    {
                        _reactor.showReactor(false, "Invalid login password, Your account will lock after " + _login_attempt_threshold + " mistakes attempted");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    sc.close();
                }
            }
        }
        private void loginAttemptPardon(string admin_id)
        {
            DateTime date_today = ga.GlobalDateToday();
            string f_admin_id = fetchAdminID(admin_id);
            try
            {
                sc.open();
                sc.command = sc.SQLCommand("UPDATE administrators_login_attempt SET status = 'unlocked' WHERE admin_id = '" + f_admin_id + "' AND DATE(date_attempt) = '" + date_today.ToString("yyyy-MM-dd") + "' AND status = 'locked'");
                sc.command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sc.close();
            }
        }
        // Login Security Attempt
        private bool loginAttempt(string admin_id)
        {
            bool r = true;
            DateTime date_today = ga.GlobalDateToday();
            try
            {
                sc.open();
                sc.command = sc.SQLCommand("SELECT admin.admin_id, COUNT(attempt.admin_id) AS max_attempt, MAX(attempt.date_attempt) AS last_attempt FROM administrators AS admin INNER JOIN administrators_login_attempt AS attempt ON admin.admin_id = attempt.admin_id WHERE (admin.admin_id = '" + admin_id + "' OR admin.username = '" + admin_id + "') AND DATE(attempt.date_attempt) = '" + date_today.ToString("yyyy-MM-dd") + "' AND attempt.status = 'locked'");
                sc.reader = sc.command.ExecuteReader();
                if (sc.reader.Read())
                {
                    int total_attempt = Convert.ToInt32(sc.reader["max_attempt"]);
                    if (total_attempt >= _login_attempt_threshold)
                    {
                        int remaining = _login_minutes_threshold - Convert.ToInt32((date_today - Convert.ToDateTime(sc.reader["last_attempt"])).TotalMinutes);
                        if (remaining > 0)
                        {
                            r = false;
                            string msg_minutes = (remaining > 1) ? "minutes" : "minute";
                            _reactor.showReactor(false, "Your account has been locked temporarily for " + remaining + " " + msg_minutes + ".");
                        }
                        else
                        {
                            sc.close();
                            loginAttemptPardon(admin_id);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sc.close();
            }
            return r;
        }
        private void untouchableKeyElements()
        {
            tbAdminID.Enabled = false;
            tbAdminPassword.Enabled = false;
            btnLogin.Enabled = false;
            btnLogin.Text = "Logging in...";
        }
        private void touchableKeyElements()
        {
            tbAdminID.Enabled = true;
            tbAdminPassword.Enabled = true;
            btnLogin.Enabled = true;
            btnLogin.Text = "Login";
        }
        private void login()
        {
            string admin_id = tbAdminID.Text;
            string password = ga.toMD5(tbAdminPassword.Text);
            if (admin_id == "" || password == "" || !ga.filterString3(admin_id))
            {
                _reactor.showReactor(false, "Please enter a valid ID and Password");
            }
            else
            {
                untouchableKeyElements();
                if (loginAttempt(admin_id))
                {
                    try
                    {
                        sc.open();
                        sc.command = sc.SQLCommand("SELECT admin_id, status FROM administrators WHERE (admin_id = '" + admin_id + "' OR username = '" + admin_id + "') AND password = '" + password + "'");
                        sc.reader = sc.command.ExecuteReader();
                        if (sc.reader.Read())
                        {
                            tbAdminID.Clear();
                            tbAdminPassword.Clear();
                            if (Convert.ToString(sc.reader["status"]) == "Inactive")
                            {
                                _reactor.showReactor(false, "Sorry you are already inactive in the record.");
                            }
                            else
                            {
                                Form1 f = new Form1();
                                f.session_admin_id = Convert.ToString(sc.reader["admin_id"]);
                                f._form_login = this;
                                this.Hide();
                                f.Show();
                            }
                            sc.close();
                            loginAttemptPardon(admin_id);
                        }
                        else
                        {
                            sc.close();
                            invalidLoginCredentials(admin_id);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Login failed " + ex.Message);
                    }
                    finally
                    {
                        sc.close();
                        touchableKeyElements();
                    }
                }
                else
                {
                    touchableKeyElements();
                }
            }
        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                tbAdminPassword.UseSystemPasswordChar = false;
            }
            else
            {
                tbAdminPassword.UseSystemPasswordChar = true;
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tbAdminID_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                login();
            }
        }

        private void tbAdminPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                login();
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            login();
        }
    }
}
