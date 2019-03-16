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
namespace EarBiometric
{
    public partial class manage_users : Form
    {
        public manage_users()
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

        // initialize classes
        global_arguments ga = new global_arguments();
        sql_connection sc = new sql_connection();
        // END

        // FORMS
        public Form1 home;
        private ear_structure estructure;
        private FormSchedules _form_schedules;
        private FormReactor _reactor = new FormReactor();
        // END

        // GLOBAL VARIABLES
        public bool global_edit = false;
        public string _update_user_id = "";
        public string _selected_user_id = "";
        public string _fetched_old_username = "";
        // END
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void requestEdit(string user_id)
        {
            restoreRegisterForm();
            _update_user_id = user_id;
            loadOtherParentForms();
            global_edit = true;
            try
            {
                sc.open();
                sc.command = sc.SQLCommand("SELECT * FROM users AS u INNER JOIN departments AS d ON u.department_id = d.department_id WHERE u.user_id = '" + user_id + "'");
                sc.reader = sc.command.ExecuteReader();
                while (sc.reader.Read())
                {
                    pictureBox1.Image = pictureBox2.Image;
                    string username = Convert.ToString(sc.reader["username"]);
                    txtUserID.Text = Convert.ToString(sc.reader["user_id"]);
                    _fetched_old_username = username;
                    tbUsername.Text = username;
                    cbRegStatus.Text = Convert.ToString(sc.reader["status"]);
                    txtFirstname.Text = Convert.ToString(sc.reader["first_name"]);
                    txtMiddlename.Text = Convert.ToString(sc.reader["middle_name"]);
                    txtLastname.Text = Convert.ToString(sc.reader["last_name"]);
                    tbRegMobile.Text = Convert.ToString(sc.reader["mobile_number"]);
                }
                button4.Text = "Update";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sc.close();
            }

            // for ears
            try
            {
                sc.open();
                sc.command = sc.SQLCommand("SELECT COUNT(angles.angle_id) AS total_angles, MIN(angles.date_added) AS last_ear_date FROM users INNER JOIN ears ON ears.user_id = users.user_id INNER JOIN angles ON angles.ear_id = ears.ear_id WHERE users.user_id = '" + user_id + "'");
                sc.reader = sc.command.ExecuteReader();
                while (sc.reader.Read())
                {
                    string last_ear_date = Convert.ToString(sc.reader["last_ear_date"]);
                    button3.Text = "Update (" + Convert.ToString(sc.reader["total_angles"]) + ")";
                    labelLastEarDate.Text = (last_ear_date != "") ? last_ear_date : "No ear angles";
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

        private void loadOtherParentForms()
        {
            estructure.loadEarStructure(_update_user_id);
            _form_schedules.loadSchedulesData(_update_user_id);
        }

        private void unloadOtherParentsFormsData()
        {
            //estructure.unloadEarStructureData();
            //_form_schedules.unloadSchedulesData();
            estructure.Dispose();
            _form_schedules.Dispose();
            estructure = new ear_structure();
            _form_schedules = new FormSchedules();
        }

        private void updateOtherParentForms(string user_id)
        {
            estructure.updateUserEars(user_id);
            _form_schedules.updateUserSchedules(user_id);
        }

        bool _update_photo = false;
        private void attachUserPhoto(string user_id)
        {
            if (pictureBox1.Image != null && _update_photo)
            {
                byte[] photo_bytes = ga.imageToByte(pictureBox1.Image);
                try
                {
                    sc.open();
                    sc.command = sc.SQLCommand("UPDATE photos SET photo_type = 'prev_profile' WHERE user_id = '" + user_id + "' AND photo_type = 'profile'");
                    sc.command.ExecuteNonQuery();
                    sc.command = sc.SQLCommand("INSERT INTO photos(user_id, photo, photo_type) VALUES('" + user_id + "', @photo, 'profile')");
                    sc.command.Parameters.Add("@photo", MySqlDbType.Blob).Value = photo_bytes;
                    sc.command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error attaching image" + ex.Message);
                }
                finally
                {
                    sc.close();
                }
            }
        }

        private string registerUser()
        {
            string r = "";
            string status = cbRegStatus.Text;
            string username = tbUsername.Text;
            string first_name = txtFirstname.Text;
            string middle_name = txtMiddlename.Text;
            string last_name = txtLastname.Text;
            string mobile_number = tbRegMobile.Text;
            string department_id = _form_schedules._department_id;
            try
            {
                sc.open();
                sc.command = sc.SQLCommand("INSERT INTO users(department_id, username, first_name, middle_name, last_name, mobile_number, password, status, date_added) VALUES('" + department_id + "','" + username + "','" + first_name + "','" + middle_name + "','" + last_name + "','" + mobile_number + "','tmp_password','" + status + "','" + ga.toSQLDateTime() + "'); SELECT LAST_INSERT_ID()");
                r = Convert.ToString(sc.command.ExecuteScalar());
                sc.command = sc.SQLCommand("INSERT INTO ears(user_id, date_added) VALUES('" + r + "','" + ga.toSQLDateTime() + "')");
                if (sc.command.ExecuteNonQuery() > 0)
                {
                    sc.close();
                    attachUserPhoto(r);
                    _reactor.showReactor(true, "User has been successfully registered!");
                }
                else
                {
                    _reactor.showReactor(false, "Unknown error while registering it might be the value you entered");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return r;
        }
        private string updateUser()
        {
            string r = _update_user_id;
            try
            {
                string username = tbUsername.Text;
                string first_name = txtFirstname.Text;
                string middle_name = txtMiddlename.Text;
                string last_name = txtLastname.Text;
                string mobile_number = tbRegMobile.Text;
                string status = cbRegStatus.Text;
                string department_id = _form_schedules._department_id;
                sc.open();
                sc.command = sc.SQLCommand("UPDATE users SET department_id = '" + department_id + "', username = '" + username + "', first_name = '" + first_name + "', middle_name = '" + middle_name + "', last_name = '" + last_name + "', mobile_number = '" + mobile_number + "', status = '" + status + "' WHERE user_id = '" + _update_user_id + "'");
                if (sc.command.ExecuteNonQuery() > 0)
                {
                    sc.close();
                    attachUserPhoto(_update_user_id);
                    _reactor.showReactor(true, "User information has been successfully updated!");
                }
                else
                {
                    _reactor.showReactor(false, "Unknown error while registering, it might be the value you entered.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return r;
        }

        private bool[] checkCredentialsValues(bool is_edit)
        {
            bool[] r = new bool[2];
            r[0] = true;
            r[1] = false;
            string confirm = tbConfirmPassword.Text;
            string password = tbPassword.Text;
            if (!is_edit || password != "")
            {
                if (password == "")
                {
                    _reactor.showReactor(false, "Credential password required.");
                    r[0] = false;
                }
                else if (password != confirm)
                {
                    _reactor.showReactor(false, "Confirm password not matched.");
                    r[0] = false;
                }
                r[1] = true;
            }
            return r;
        }
        private void updatePassword(string user_id, bool required)
        {
            string password = ga.toMD5(tbPassword.Text);
            if (required) {
                try
                {
                    sc.open();
                    sc.command = sc.SQLCommand("UPDATE users SET password = '" + password + "' WHERE user_id = '" + user_id + "'");
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
        }

        private bool isUsernameExists(string username)
        {
            bool r = false;
            try
            {
                sc.open();
                sc.command = sc.SQLCommand("SELECT COUNT(username) AS c FROM users WHERE username = '" + username + "'");
                if (Convert.ToInt32(sc.command.ExecuteScalar()) > 0)
                {
                    r = true;
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

        private void updateUserInformation()
        {
            if (tbUsername.Text == "" || txtFirstname.Text == "" || txtLastname.Text == "" || tbRegMobile.Text == "" || cbRegStatus.Text == "")
            {
                _reactor.showReactor(false, "Fields are mandatory");
            }
            else if (!ga.filterString3(tbUsername.Text) || !ga.filterString1(txtFirstname.Text) || (!ga.filterString1(txtMiddlename.Text) && txtMiddlename.Text != "") || !ga.filterString1(txtLastname.Text) || !ga.filterString2(tbRegMobile.Text))
            {
                _reactor.showReactor(false, "Some of your values are invalid");
            }
            else if (isUsernameExists(tbUsername.Text) && _fetched_old_username != tbUsername.Text)
            {
                _reactor.showReactor(false, "Username: " + tbUsername.Text + " already used.");
            }
            else if (_form_schedules._department_id == string.Empty)
            {
                _reactor.showReactor(false, "Schedules and Departments are not settle");
            }
            else
            {
                bool[] check_password = checkCredentialsValues(global_edit);
                if (check_password[0])
                {
                    button4.Enabled = false;
                    string user_id = "";
                    if (global_edit)
                    {
                        button4.Text = "Updating...";
                        user_id = updateUser();
                        updatePassword(user_id, check_password[1]);
                        updateOtherParentForms(user_id);
                        restoreRegisterForm();
                    }
                    else
                    {
                        button4.Text = "Registering...";
                        user_id = registerUser();
                        updatePassword(user_id, check_password[1]);
                        updateOtherParentForms(user_id);
                        restoreRegisterForm();
                    }

                }
            }
        }

        private void manage_users_Load(object sender, EventArgs e)
        {
            _form_schedules = new FormSchedules();
            estructure = new ear_structure();
            //pictureBox1.Image = Image.FromFile("D:\\Pictures\\Dasha Taran\\20398833_1424594000967391_4601616716710019072_n.jpg");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            estructure.mu = this;
            this.Hide();
            estructure.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            updateUserInformation();
        }

        private void searchUser()
        {
            string name = textBox1.Text;
            if (ga.filterString2(name))
            {
                dataGridView1.Rows.Clear();
                try
                {
                    sc.open();
                    sc.command = sc.SQLCommand("SELECT user_id, first_name, last_name FROM users WHERE (first_name LIKE '%" + name + "%' OR last_name LIKE '%" + name + "%'  OR user_id LIKE '%" + name + "%') LIMIT 25");
                    sc.reader = sc.command.ExecuteReader();
                    while (sc.reader.Read())
                    {
                        dataGridView1.Rows.Add(sc.reader["user_id"], sc.reader["first_name"] + " " + sc.reader["last_name"]);
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
        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            searchUser();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                string user_id = Convert.ToString(dataGridView1.CurrentRow.Cells[0].Value);
                requestEdit(user_id);
                tabControl1.SelectedTab = tabPage2;
            }
            catch
            {
                _reactor.showReactor(false, "Please select a user");
            }
        }
        private void restoreRegisterForm()
        {
            if (global_edit)
            {
                fetchInformation(_update_user_id);
            }
            global_edit = false;
            _update_photo = false;
            tabControl1.SelectedTab = tabPage1;
            pictureBox1.Image = Image.FromFile(ga.getResourcesPath() + "face-man.jpg");
            txtUserID.Text = "--";
            txtFirstname.Clear();
            txtMiddlename.Clear();
            txtLastname.Clear();
            tbRegMobile.Clear();
            labelLastEarDate.Text = "Not yet";
            _fetched_old_username = "";
            tbUsername.Clear();
            tbPassword.Clear();
            tbConfirmPassword.Clear();
            unloadOtherParentsFormsData();
            button3.Text = "Update";
            button4.Text = "Register";
            button4.Enabled = true;
        }
        private void btnCancelEdit_Click(object sender, EventArgs e)
        {
            restoreRegisterForm();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dr = opfProfilePhoto.ShowDialog();
            if (dr == DialogResult.OK)
            {
                Mat checker;
                try
                {
                    checker = CvInvoke.Imread(opfProfilePhoto.FileName);
                    pictureBox1.Image = Image.FromFile(opfProfilePhoto.FileName);
                    _update_photo = true;
                }
                catch
                {
                    _reactor.showReactor(false, "Invalid filename");
                }

            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            home.Show();
            this.Close();
        }

        private void fetchInformation(string user_id)
        {
            try
            {
                sc.open();
                sc.command = sc.SQLCommand("SELECT *, (SELECT photo FROM photos WHERE user_id = u.user_id AND photo_type = 'profile') as photo FROM users AS u INNER JOIN departments AS d ON u.department_id = d.department_id WHERE u.user_id = '" + user_id + "'");
                sc.reader = sc.command.ExecuteReader();
                while (sc.reader.Read())
                {
                    try
                    {
                        byte[] photo_bytes = (byte[])sc.reader["photo"];
                        pictureBox2.Image = ga.byteToImage(photo_bytes);
                    }
                    catch
                    {
                        pictureBox2.Image = Image.FromFile(ga.getResourcesPath() + "face-man.jpg");
                    }
                    textBox2.Text = Convert.ToString(sc.reader["user_id"]);
                    textBox3.Text = Convert.ToString(sc.reader["status"]);
                    textBox4.Text = Convert.ToString(sc.reader["first_name"]);
                    tbInfoMiddle.Text = Convert.ToString(sc.reader["middle_name"]);
                    textBox6.Text = Convert.ToString(sc.reader["last_name"]);
                    tbInfoMobile.Text = Convert.ToString(sc.reader["mobile_number"]);
                    tbInfoDepartments.Text = Convert.ToString(sc.reader["name"]);
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
        private string currentSelectedUserID()
        {
            try
            {
                _selected_user_id = Convert.ToString(dataGridView1.CurrentRow.Cells[0].Value);
            }
            catch
            {
                _reactor.showReactor(false, "Please select a user");
            }
            return _selected_user_id;
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (currentSelectedUserID() != "")
            {
                fetchInformation(currentSelectedUserID());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (currentSelectedUserID() != "")
            {
                FormTimeline ft = new FormTimeline();
                this.Hide();
                ft._mu = this;
                ft._user_id = currentSelectedUserID();
                ft.Show();
            }
        }
        private void btnViewSchedules_Click(object sender, EventArgs e)
        {
            if (currentSelectedUserID() != "")
            {
                _form_schedules._manage_users = this;
                _form_schedules._user_id = currentSelectedUserID();
                _form_schedules.Show();
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
            _form_schedules._manage_users = this;
            _form_schedules.Show();
        }

        private void refreshStatistics()
        {
            int total_ears = 0;
            int total_users = 0;
            dgvStatisticUsers.Rows.Clear();
            try
            {
                sc.open();
                sc.command = sc.SQLCommand("SELECT user_id, first_name, last_name, (SELECT COUNT(ears.user_id) FROM ears INNER JOIN angles ON ears.ear_id = angles.ear_id WHERE ears.user_id = users.user_id) AS ears FROM users ORDER BY ears DESC");
                sc.reader = sc.command.ExecuteReader();
                while (sc.reader.Read())
                {
                    string name = Convert.ToString(sc.reader["first_name"]) + " " + Convert.ToString(sc.reader["last_name"]);
                    int ears = Convert.ToInt32(sc.reader["ears"]);
                    total_ears += ears;
                    total_users++;
                    dgvStatisticUsers.Rows.Add(Convert.ToString(sc.reader["user_id"]), name, ears);
                }
                tbStatisticRegisteredUser.Text = total_users.ToString();
                tbStatisticEarsAngles.Text = total_ears.ToString();
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

        private void btnStatisticsRefresh_Click(object sender, EventArgs e)
        {
            refreshStatistics();
        }

        private void txtLastname_KeyUp(object sender, KeyEventArgs e)
        {
            string password = txtLastname.Text.ToLower();
            string compress = "";
            for (var i=0;i<password.Length;i++)
            {
                if (password[i] != ' ')
                {
                    compress += password[i];
                }
            }
            if (!global_edit)
            {
                tbPassword.Text = compress;
                tbConfirmPassword.Text = compress;
            }
        }
    }
}
