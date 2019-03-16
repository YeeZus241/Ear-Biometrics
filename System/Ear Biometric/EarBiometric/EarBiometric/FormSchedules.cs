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
    public partial class FormSchedules : Form
    {
        public FormSchedules()
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

        sql_connection sc = new sql_connection();
        global_arguments ga = new global_arguments();
        FormReactor _reactor = new FormReactor();
        public manage_users _manage_users { get; set; }
        public string _user_id = "";
        public string _department_id = "";
        private void loadCurrentSchedules()
        {
            dgvWeeks.Rows.Clear();
            try
            {
                sc.open();
                sc.command = sc.SQLCommand("SELECT schedules.schedule_id, schedules.week_name, schedules.time_in, schedules.time_out FROM users INNER JOIN users_schedules ON users.user_id = users_schedules.user_id INNER JOIN schedules ON users_schedules.schedule_id = schedules.schedule_id WHERE users.user_id = '" + _user_id + "'");
                sc.reader = sc.command.ExecuteReader();
                while (sc.reader.Read())
                {
                    dgvWeeks.Rows.Add(Convert.ToString(sc.reader["schedule_id"]), Convert.ToString(sc.reader["week_name"]), Convert.ToDateTime(sc.reader["time_in"]).ToString("h:mm:ss tt"), Convert.ToDateTime(sc.reader["time_out"]).ToString("h:mm:ss tt"));
                }
                changeWeekChoicesData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sc.close();
            }
            changeWeekChoicesData();
        }

        private void applyDepartmentSchedules(string department_id)
        {
            dgvWeeks.Rows.Clear();
            try
            {
                sc.open();
                sc.command = sc.SQLCommand("SELECT schedules.schedule_id, schedules.week_name, schedules.time_in, schedules.time_out FROM departments INNER JOIN departments_schedules ON departments.department_id = departments_schedules.department_id INNER JOIN schedules ON departments_schedules.schedule_id = schedules.schedule_id WHERE departments.department_id = '" + department_id + "'");
                sc.reader = sc.command.ExecuteReader();
                while (sc.reader.Read())
                {
                    dgvWeeks.Rows.Add("--", Convert.ToString(sc.reader["week_name"]), Convert.ToDateTime(sc.reader["time_in"]).ToString("h:mm:ss tt"), Convert.ToDateTime(sc.reader["time_out"]).ToString("h:mm:ss tt"));
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
            changeWeekChoicesData();
        }
        private bool isSchedulesAdjusted()
        {
            bool r = false;
            try
            {

            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sc.close();
            }
            return r;
        }
        private void loadDepartments()
        {
            cbRegDepartments.Items.Clear();
            try
            {
                sc.open();
                sc.command = sc.SQLCommand("SELECT name FROM departments");
                sc.reader = sc.command.ExecuteReader();
                while (sc.reader.Read())
                {
                    var item = Convert.ToString(sc.reader["name"]);
                    if (!cbRegDepartments.Items.Contains(item)) {
                        cbRegDepartments.Items.Add(item);
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
        }
        private void changeWeekChoicesData()
        {
            cmbWeek.Items.Clear();
            foreach (var i in ga._week_names)
            {
                bool exists = false;
                for (var j = 0; j < dgvWeeks.Rows.Count; j++)
                {
                    if (Convert.ToString(dgvWeeks.Rows[j].Cells[1].Value) == i)
                    {
                        exists = true;
                    }
                }
                if (!exists)
                {
                    cmbWeek.Items.Add(i);
                }
            }
        }
        public void loadSchedulesData(string user_id)
        {
            loadDepartments();
            string department = "";
            try
            {
                sc.open();
                sc.command = sc.SQLCommand("SELECT users.user_id, users.department_id, departments.name FROM users INNER JOIN departments ON users.department_id = departments.department_id WHERE users.user_id = '" + user_id + "'");
                sc.reader = sc.command.ExecuteReader();
                if (sc.reader.Read())
                {
                    _department_id = Convert.ToString(sc.reader["department_id"]);
                    _user_id = Convert.ToString(sc.reader["user_id"]);
                    department = Convert.ToString(sc.reader["name"]);
                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sc.close();
                cbRegDepartments.Text = department;
            }
            // load main current schedule
            loadCurrentSchedules();
        }
        // complete this form
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            _manage_users.Show();
            this.Hide();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        
        private string getDepartmentID(string name)
        {
            string r = "";
            try
            {
                sc.open();
                sc.command = sc.SQLCommand("SELECT department_id FROM departments WHERE name = '" + name + "'");
                sc.reader = sc.command.ExecuteReader();
                while (sc.reader.Read())
                {
                    r = Convert.ToString(sc.reader["department_id"]);
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
        private void FormSchedules_Load(object sender, EventArgs e)
        {
            sc.activate_SQLConnection();
            if (_user_id == "") {
                loadDepartments();
            }
            changeWeekChoicesData();
        }
        private bool checkIfWeekExists(string a)
        {
            bool r = false;
            for (int i=0;i<dgvWeeks.Rows.Count;i++)
            {
                if (Convert.ToString(dgvWeeks.Rows[i].Cells[0].Value) == a)
                {
                    r = true;
                    break;
                }
            }
            return r;
        }
        
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string week = cmbWeek.Text;
            var time_in = dtTimeIn.Value;
            var time_out = dtTimeOut.Value;
            if (ga.checkScheduleTime(time_in, time_out))
            {
                if (cmbWeek.Items.Count <= 0)
                {
                    _reactor.showReactor(true, "All week name has been added");
                }
                else if (week == "")
                {
                    _reactor.showReactor(false, "Please select a week name");
                }
                else if (checkIfWeekExists(week))
                {
                    _reactor.showReactor(false, week + "schedule already exists, remove first before updating new");
                }
                else
                {
                    dgvWeeks.Rows.Add("--", week, time_in.ToString("h:mm:ss tt"), time_out.ToString("h:mm:ss tt"));
                }
                changeWeekChoicesData();
            }
            else
            {
                _reactor.showReactor(false, "Invalid date selected");
            }
        }

        
        private void cbRegDepartments_SelectedIndexChanged(object sender, EventArgs e)
        {
            string department_id = getDepartmentID(cbRegDepartments.Text);
            _department_id = department_id;
            if (checkBox1.Checked)
            {
                applyDepartmentSchedules(department_id);
            }
        }

        public void updateUserSchedules(string user_id)
        {
            // save data
            try
            {
                sc.open();
                for (var i=0;i<dgvWeeks.Rows.Count;i++)
                {
                    string schedule_id = Convert.ToString(dgvWeeks.Rows[i].Cells[0].Value);
                    string week_name = Convert.ToString(dgvWeeks.Rows[i].Cells[1].Value);
                    string time_in = Convert.ToString(ga.convertToSQLDateTime(Convert.ToDateTime(DateTime.Now.ToString("M/d/yyyy") + " " + dgvWeeks.Rows[i].Cells[2].Value)));
                    string time_out = Convert.ToString(ga.convertToSQLDateTime(Convert.ToDateTime(DateTime.Now.ToString("M/d/yyyy") + " " + dgvWeeks.Rows[i].Cells[3].Value)));
                    if (schedule_id == "--")
                    {
                        string fetched_schedule_id = ga.getScheduleID(week_name, time_in, time_out);
                        sc.command = sc.SQLCommand("INSERT INTO users_schedules(user_id, schedule_id) VALUES('" + user_id + "','" + fetched_schedule_id + "')");
                        sc.command.ExecuteNonQuery();
                    }
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
        
        private bool deleteCurrentUserSchedules(string user_id)
        {
            bool r = false;
            try
            {
                sc.open();
                sc.command = sc.SQLCommand("DELETE FROM users_schedules WHERE user_id = '" + user_id + "'");
                sc.command.ExecuteNonQuery();
                r = true;
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sc.close();
            }
            return r;
        }
        private void button1_Click(object sender, EventArgs e)
        {
           
            if (_user_id != "" && checkBox1.Checked) {
                DialogResult dr = MessageBox.Show("Are you sure you want to use the current schedules of the departments? if you continue all current schedules of this user will be deleted", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (dr == DialogResult.OK) {
                    if (deleteCurrentUserSchedules(_user_id)) {
                        this.Hide();
                        _manage_users.Show();
                    }
                }
            }
            else
            {
                this.Hide();
                _manage_users.Show();
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to remove this week name?", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                try
                {
                    string schedule_id = dgvWeeks.CurrentRow.Cells[0].Value.ToString();
                    if (schedule_id == "--") {
                        dgvWeeks.Rows.RemoveAt(dgvWeeks.CurrentRow.Index);
                    }
                    else {
                        sc.open();
                        sc.command = sc.SQLCommand("DELETE FROM users_schedules WHERE schedule_id = '" + schedule_id + "'");
                        if (sc.command.ExecuteNonQuery() > 0)
                        {
                            dgvWeeks.Rows.RemoveAt(dgvWeeks.CurrentRow.Index);
                        }
                    }
                    changeWeekChoicesData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Please select week name to delete " + ex.Message);
                }
                finally
                {
                    sc.close();
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                applyDepartmentSchedules(_department_id);
            }
            else if (_user_id != "")
            {
                loadCurrentSchedules();
            }
        }
    }
}
