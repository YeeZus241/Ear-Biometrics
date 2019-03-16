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
namespace EarBiometric
{
    public partial class FormSettings : Form
    {
        public FormSettings()
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

        // initialize classes
        sql_connection sc = new sql_connection();
        global_arguments ga = new global_arguments();
        FormReactor _reactor = new FormReactor();
        public Form1 _home;
        private enum RequestUpdate
        {
            Register,
            Update
        }
        private RequestUpdate current_RU = RequestUpdate.Register;
        private string _admin_id = "";
        public string _current_admin_id = "";
        public void initializeAdminInformation(string admin_id)
        {
            this._current_admin_id = admin_id;
            try
            {
                sc.open();
                sc.command = sc.SQLCommand("SELECT * FROM administrators WHERE admin_id = '" + admin_id + "'");
                sc.reader = sc.command.ExecuteReader();
                while (sc.reader.Read())
                {
                    byte[] photo_bytes = (byte[])sc.reader["photo"];
                    pictureBox3.Image = ga.byteToImage(photo_bytes);
                    textBox11.Text = Convert.ToString(sc.reader["first_name"] + " " + sc.reader["last_name"]);
                    textBox15.Text = Convert.ToString(sc.reader["username"]);
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
        private bool checkAdministratorPassword(string admin_id, string password)
        {
            bool r = false;
            try
            {
                sc.open();
                sc.command = sc.SQLCommand("SELECT password FROM administrators WHERE admin_id = '" + admin_id + "'");
                sc.reader = sc.command.ExecuteReader();
                while (sc.reader.Read())
                {
                    r = (Convert.ToString(sc.reader["password"]) == ga.toMD5(password)) ? true : false;
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
        public bool isUsernameExists(string a)
        {
            bool r = false;
            try
            {
                sc.open();
                sc.command = sc.SQLCommand("SELECT COUNT(username) FROM administrators WHERE username = '" + a + "'");
                if (Convert.ToInt16(sc.command.ExecuteScalar()) > 0)
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
        private string _fetched_old_username = "";
        private void updateAdministrator()
        {
            string status = comboBox1.Text;
            string firstname = textBox10.Text;
            string lastname = textBox9.Text;
            string username = textBox8.Text;
            string password = textBox13.Text;
            string c_password = textBox14.Text;
            if (status == "" || firstname == "" || lastname == "" || username == "")
            {
                _reactor.showReactor(false, "The fields are mandatory! please fill up");
            }
            else if (!ga.filterString1(firstname) || !ga.filterString1(lastname) || !ga.filterString3(username))
            {
                _reactor.showReactor(false, "Some of your values are invalid");
            }
            else
            {
                if (password != c_password)
                {
                    _reactor.showReactor(false, "Confirmation of the password not match!");
                }
                else
                {
                    if (isUsernameExists(username) && _fetched_old_username != username)
                    {
                        _reactor.showReactor(false, "The username you entered is already exists");
                    }
                    else
                    {
                        byte[] photo_bytes = ga.imageToByte(pictureBox1.Image);
                        if (current_RU == RequestUpdate.Register)
                        {
                            if (password == "")
                            {
                                _reactor.showReactor(false, "Enter a pwassword before registering");
                            }
                            else
                            {
                                if (isUsernameExists(username))
                                {
                                    _reactor.showReactor(false, "The username you entered already exists in the database");
                                }
                                else
                                {
                                    try
                                    {
                                        sc.open();
                                        sc.command = sc.SQLCommand("INSERT INTO administrators(photo, username, first_name, last_name, status, password) VALUES(@admin_photo, '" + username + "','" + firstname + "','" + lastname + "','" + status + "','" + ga.toMD5(password) + "')");
                                        sc.command.Parameters.Add("@admin_photo", sc.MysqlTypeBlob()).Value = photo_bytes;
                                        if (sc.command.ExecuteNonQuery() > 0)
                                        {
                                            _reactor.showReactor(true, "New administrator has been successfully registered");
                                            restoreAdministratorForm();
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show("error " + ex.Message);
                                    }
                                    finally
                                    {
                                        sc.close();
                                    }
                                }
                            }
                        }
                        else if (current_RU == RequestUpdate.Update)
                        {
                            if (checkAdministratorPassword(_admin_id, textBox7.Text) == false)
                            {
                                _reactor.showReactor(false, "Invalid current password of this administrator");
                            }
                            else
                            {
                                string sql = "UPDATE administrators SET photo = @admin_photo, username = '" + username + "',  first_name = '" + firstname + "', last_name = '" + lastname + "', status = '" + status + "' WHERE admin_id = '" + _admin_id + "'";
                                if (textBox13.Text != "")
                                {
                                    sql = "UPDATE administrators SET photo = @admin_photo, username = '" + username + "',  first_name = '" + firstname + "', last_name = '" + lastname + "', status = '" + status + "', password = '" + ga.toMD5(password) + "' WHERE admin_id = '" + _admin_id + "'";
                                }
                                try
                                {
                                    sc.open();
                                    sc.command = sc.SQLCommand(sql);
                                    sc.command.Parameters.Add("@admin_photo", MySqlDbType.Blob).Value = photo_bytes;
                                    if (sc.command.ExecuteNonQuery() > 0)
                                    {
                                        sc.close();
                                        _reactor.showReactor(true, "Administrator settings has been updated!");
                                        showAdminInfo(_admin_id);
                                        initializeAdminInformation(_current_admin_id);
                                        restoreAdministratorForm();
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
                    }
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            updateAdministrator();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult dr = opfAdminPhoto.ShowDialog();
            if (dr == DialogResult.OK)
            {
                Mat checker;
                try
                {
                    checker = CvInvoke.Imread(opfAdminPhoto.FileName);
                    pictureBox1.Image = checker.Bitmap;
                }
                catch
                {
                    _reactor.showReactor(false, "Invalid file name");
                }

            }
            else
            {

            }
        }
        private void restoreAdministratorForm()
        {
            tabControl1.SelectedTab = tabPage3;
            current_RU = RequestUpdate.Register;
            _admin_id = "";
            pictureBox1.Image = null;
            textBox12.Text = "--";
            comboBox1.Text = "Active";
            textBox10.Clear();
            textBox9.Clear();
            textBox8.Clear();
            textBox13.Clear();
            textBox14.Clear();
            textBox7.Clear();
            button2.Text = "Register";
            _fetched_old_username = "";
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {
            sc.activate_SQLConnection();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            restoreAdministratorForm();
        }
        private void requestAdminUpdate(string admin_id)
        {
            current_RU = RequestUpdate.Update;
            _admin_id = admin_id;
            try
            {
                sc.open();
                sc.command = sc.SQLCommand("SELECT * FROM administrators WHERE admin_id = '" + admin_id + "'");
                sc.reader = sc.command.ExecuteReader();
                while (sc.reader.Read())
                {
                    byte[] photo_bytes = (byte[])sc.reader["photo"];
                    pictureBox1.Image = ga.byteToImage(photo_bytes);
                    textBox12.Text = Convert.ToString(sc.reader["admin_id"]);
                    comboBox1.Text = Convert.ToString(sc.reader["status"]);
                    textBox10.Text = Convert.ToString(sc.reader["first_name"]);
                    textBox9.Text = Convert.ToString(sc.reader["last_name"]);
                    textBox8.Text = Convert.ToString(sc.reader["username"]);
                    _fetched_old_username = Convert.ToString(sc.reader["username"]);
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
            button2.Text = "Update";
            tabControl1.SelectedTab = tabPage2;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            requestAdminUpdate(textBox2.Text);
        }
        private void searchAdministrator()
        {

            string key = textBox1.Text;
            if (ga.filterString2(key))
            {
                dataGridView1.Rows.Clear();
                try
                {
                    sc.open();
                    sc.command = sc.SQLCommand("SELECT admin_id, first_name, last_name FROM administrators WHERE (first_name LIKE '%" + key + "%' OR  last_name LIKE '%" + key + "%')");
                    sc.reader = sc.command.ExecuteReader();
                    while (sc.reader.Read())
                    {
                        dataGridView1.Rows.Add(Convert.ToString(sc.reader["admin_id"]), Convert.ToString(sc.reader["first_name"] + " " + sc.reader["last_name"]));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error " + ex.Message);
                }
                finally
                {
                    sc.close();
                }
            }
        }
        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            searchAdministrator();
        }
        private void showAdminInfo(string admin_id)
        {
            try
            {
                sc.open();
                sc.command = sc.SQLCommand("SELECT * FROM administrators WHERE admin_id = '" + admin_id + "'");
                sc.reader = sc.command.ExecuteReader();
                while (sc.reader.Read())
                {
                    byte[] photo_bytes = (byte[])sc.reader["photo"];
                    pictureBox2.Image = ga.byteToImage(photo_bytes);
                    textBox2.Text = Convert.ToString(sc.reader["admin_id"]);
                    textBox3.Text = Convert.ToString(sc.reader["status"]);
                    textBox4.Text = Convert.ToString(sc.reader["first_name"]);
                    textBox5.Text = Convert.ToString(sc.reader["last_name"]);
                    textBox6.Text = Convert.ToString(sc.reader["username"]);
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
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string admin_id = Convert.ToString(dataGridView1.CurrentRow.Cells[0].Value);
                showAdminInfo(admin_id);
            }
            catch
            {

            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            requestAdminUpdate(_current_admin_id);
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            _home.Show();
            this.Close();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
    }
}
