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
    public partial class FormDeparments : Form
    {
        public FormDeparments()
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

        static global_arguments ga = new global_arguments();
        sql_connection sc = new sql_connection();
        FormReactor _reactor = new FormReactor();
        public Form1 home { get; set; }
       
        private enum RequestModify
        {
            Register,
            Update
        }
        private enum DepartmenPages
        {
            page1,
            page2,
            page3
        }
        string _update_department_id = "";
        string _selected_department_id = "";
        RequestModify current_modify = RequestModify.Register;
        DepartmenPages _next_department_page = DepartmenPages.page1;
        DepartmenPages _current_department_page;
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            home.Show();
            this.Close();
        }

        private void FormDeparments_Load(object sender, EventArgs e)
        {
            changeDepartmentStatisticsData();
            changeWeekChoicesData();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            updateDepartments();
        }

        public string createNewDepartment(string name)
        {
            string r = "";
            try
            {
                sc.open();
                sc.command = sc.SQLCommand("INSERT INTO departments(name) VALUES('" + name + "'); SELECT LAST_INSERT_ID() AS last_id");
                sc.reader = sc.command.ExecuteReader();
                if (sc.reader.Read())
                {
                    r = Convert.ToString(sc.reader["last_id"]);
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
            if (r == "")
            {
                _reactor.showReactor(false, "Department " + name + " already exists try difference department name");
            }
            return r;
        }

        private bool newSchedules(string department_id)
        {
            bool r = true;
            try
            {
                sc.open();
                for (var i = 0; i < dgvWeeks.Rows.Count; i++)
                {
                    string schedule_id = Convert.ToString(dgvWeeks.Rows[i].Cells[0].Value);
                    string week_name = Convert.ToString(dgvWeeks.Rows[i].Cells[1].Value);
                    string time_in = Convert.ToString(ga.convertToSQLDateTime(Convert.ToDateTime(DateTime.Now.ToString("M/d/yyyy") + " " + dgvWeeks.Rows[i].Cells[2].Value)));
                    string time_out = Convert.ToString(ga.convertToSQLDateTime(Convert.ToDateTime(DateTime.Now.ToString("M/d/yyyy") + " " + dgvWeeks.Rows[i].Cells[3].Value)));
                    if (schedule_id == "--")
                    {
                        string fetched_schedule_id = ga.getScheduleID(week_name, time_in, time_out);
                        sc.command = sc.SQLCommand("INSERT INTO departments_schedules(department_id, schedule_id) VALUES('" + department_id + "','" + fetched_schedule_id + "')");
                        if (sc.command.ExecuteNonQuery() <= 0)
                        {
                            r = false;
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
            if (r) {
                _reactor.showReactor(true, "Succesfully updated");
                restoreDepartmentForms();
            }
            else
            {
                MessageBox.Show("Unknown error while updating weeks");
            }
            return r;
        }
        private void updateDepartments()
        {
            string department_name = tbRegDepartments.Text;
            if (department_name == "")
            {
                _reactor.showReactor(false, "Enter department name");
            }else if (!ga.filterString2(department_name))
            {
                _reactor.showReactor(false, "Invalid department name");
            }
            else if (dgvWeeks.Rows.Count <= 0)
            {
                _reactor.showReactor(false, "Please add atleast 1 schedule in week");
            }
            else
            {
                if (current_modify == RequestModify.Register)
                {
                    // create departments first then get the ID
                    string department_id = createNewDepartment(department_name);
                    if (department_id != "")
                    {
                        newSchedules(department_id);
                    }
                }
                else if (current_modify == RequestModify.Update)
                {
                    try
                    {
                        sc.open();
                        sc.command = sc.SQLCommand("UPDATE departments SET name = '" + department_name + "' WHERE department_id = '" + _update_department_id +  "'");
                        sc.command.ExecuteNonQuery();
                    }catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        sc.close();
                    }
                    newSchedules(_update_department_id);
                }
            }
        }
        private void analyzeHoursRequired()
        {
            var time_in = dateTimePicker1.Value; //Value.ToString("HH:mm:ss");
            var time_out = dateTimePicker2.Value; //.Value.ToString("HH:mm:ss");
            int hours = Math.Abs(Convert.ToInt32(time_in.Hour - time_out.Hour));
            textBox2.Text = hours.ToString();
        }
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            analyzeHoursRequired();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            analyzeHoursRequired();
        }
        private string _search_value = "";
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
        private void restoreDepartmentForms()
        {
            tabControl1.SelectedTab = tabPage1;
            _update_department_id = "";
            current_modify = RequestModify.Register;
            dgvWeeks.Rows.Clear();
            tbRegDepartments.Clear();
            changeDepartmentStatisticsData();
            textBox2.Clear();
            changeWeekChoicesData();
        }
        private void requestDepartmentUpdate(string a)
        {
            _update_department_id = a;
            dgvWeeks.Rows.Clear();
            current_modify = RequestModify.Update;
            try
            {
                sc.open();
                sc.command = sc.SQLCommand("SELECT departments.name, schedules.schedule_id, schedules.week_name, schedules.time_in, schedules.time_out FROM departments INNER JOIN departments_schedules ON departments.department_id = departments_schedules.department_id INNER JOIN schedules ON departments_schedules.schedule_id = schedules.schedule_id WHERE departments.department_id = '" + a + "'");
                sc.reader = sc.command.ExecuteReader();
                while (sc.reader.Read())
                {
                    tbRegDepartments.Text = Convert.ToString(sc.reader["name"]);
                    dgvWeeks.Rows.Add(Convert.ToString(sc.reader["schedule_id"]), Convert.ToString(sc.reader["week_name"]), Convert.ToDateTime(sc.reader["time_in"]).ToString("h:mm:ss tt"), Convert.ToDateTime(sc.reader["time_out"]).ToString("h:mm:ss tt"));
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
            tabControl1.SelectedTab = tabPage2;
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string department_id = (_selected_department_id != "") ? _selected_department_id : Convert.ToString(dgvCurrentDepartments.CurrentRow.Cells[0].Value);
                requestDepartmentUpdate(department_id);
            }
            catch
            {
                _reactor.showReactor(false, "Please select a row");
            }
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            string search_value = textBox1.Text;
            if (ga.filterString2(search_value))
            {
                _search_value = search_value;
                _next_department_page = DepartmenPages.page1;
                changeDepartmentStatisticsData();
            }
        }

        private bool checkIfWeekExists(string a)
        {
            bool r = false;
            for (int i = 0; i < dgvWeeks.Rows.Count; i++)
            {
                if (Convert.ToString(dgvWeeks.Rows[i].Cells[0].Value) == a)
                {
                    r = true;
                    break;
                }
            }
            return r;
        }

        private void btnAddWeek_Click(object sender, EventArgs e)
        {
            string week = cmbWeek.Text;
            var time_in = dateTimePicker1.Value;
            var time_out = dateTimePicker2.Value;
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

        private void panel1_MouseDown_1(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        private bool checkDepartmentsRemaining(string department_id)
        {
            bool r = false;
            try
            {
                sc.open();
                sc.command = sc.SQLCommand("SELECT COUNT(department_id) AS c FROM departments_schedules WHERE department_Id = '" + department_id + "'");
                sc.reader = sc.command.ExecuteReader();
                if (sc.reader.Read())
                {
                    if (Convert.ToInt16(sc.reader["c"]) > 1) {
                        r = true;
                    }
                }
                else
                {
                    MessageBox.Show("Cannot delete, atleast there are 1 schedule in week", "Cannot Delete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
        private void btnRemove_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to remove this week name?", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                try
                {
                    string schedule_id = dgvWeeks.CurrentRow.Cells[0].Value.ToString();
                    if (schedule_id == "--")
                    {
                        dgvWeeks.Rows.RemoveAt(dgvWeeks.CurrentRow.Index);
                    }
                    else
                    {
                        if (checkDepartmentsRemaining(_update_department_id))
                        {
                            sc.open();
                            sc.command = sc.SQLCommand("DELETE FROM departments_schedules WHERE schedule_id = '" + schedule_id + "'");
                            if (sc.command.ExecuteNonQuery() > 0)
                            {
                                dgvWeeks.Rows.RemoveAt(dgvWeeks.CurrentRow.Index);
                            }
                            else
                            {
                                _reactor.showReactor(false, "Unknown error while deleteing schedule");
                            }
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

        private void button3_Click(object sender, EventArgs e)
        {
            restoreDepartmentForms();
        }

        private void changeDepartmentStatisticsData()
        {
            try
            {
                if (_next_department_page == DepartmenPages.page1)
                {
                    dgvCurrentDepartments.Rows.Clear();
                    _next_department_page = DepartmenPages.page2;
                    _current_department_page = DepartmenPages.page1;

                    List<string> names = new List<string>() { "Department ID", "Name", "Total week hours"};
                    ga.applyDGVColumnPageStyle(dgvCurrentDepartments, names);
                    try
                    {
                        sc.open();
                        sc.command = sc.SQLCommand("SELECT departments.department_id, departments.name FROM departments INNER JOIN departments_schedules ON departments.department_id = departments_schedules.department_id INNER JOIN schedules ON schedules.schedule_id = departments_schedules.schedule_id WHERE (departments.department_id LIKE '%" + _search_value + "' OR departments.name LIKE '%" + _search_value + "%') GROUP BY departments.department_id");
                        sc.reader = sc.command.ExecuteReader();
                        while (sc.reader.Read())
                        {
                            string department_id = Convert.ToString(sc.reader["department_id"]);
                            dgvCurrentDepartments.Rows.Add(department_id, Convert.ToString(sc.reader["name"]), "Calculating...");
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
                    try
                    {
                        for (int i=0;i<dgvCurrentDepartments.Rows.Count;i++)
                        {
                            string department_id = Convert.ToString(dgvCurrentDepartments.Rows[i].Cells[0].Value);
                            sc.open();
                            sc.command = sc.SQLCommand("SELECT TIMESTAMPDIFF(hour, time_in, time_out) AS hours_in_week FROM departments INNER JOIN departments_schedules ON departments.department_id = departments_schedules.department_id INNER JOIN schedules ON departments_schedules.schedule_id = schedules.schedule_id WHERE departments.department_id = '" + department_id + "'");
                            sc.reader = sc.command.ExecuteReader();
                            int hours_in_week = 0;
                            while (sc.reader.Read())
                            {
                                hours_in_week += Convert.ToInt32(sc.reader["hours_in_week"]);
                            }
                            dgvCurrentDepartments.Rows[i].Cells[2].Value = hours_in_week.ToString();
                            sc.close();
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
                else if (_next_department_page == DepartmenPages.page2) {
                    // show week schedules
                    _current_department_page = DepartmenPages.page2;
                    List<string> names = new List<string>() { "Week Name", "Time IN", "Time Out", "Total Hours required" };
                    ga.applyDGVColumnPageStyle(dgvCurrentDepartments, names);
                    try
                    {
                        sc.open();
                        sc.command = sc.SQLCommand("SELECT schedules.week_name, schedules.time_in, schedules.time_out FROM departments INNER JOIN departments_schedules ON departments.department_id = departments_schedules.department_id INNER JOIN schedules ON departments_schedules.schedule_id = schedules.schedule_id WHERE departments.department_id = '" + _selected_department_id + "'");
                        sc.reader = sc.command.ExecuteReader();
                        while (sc.reader.Read())
                        {
                            DateTime req_in = Convert.ToDateTime(sc.reader["time_in"]);
                            DateTime req_out = Convert.ToDateTime(sc.reader["time_out"]);
                            string diff_hours = (req_out - req_in).TotalHours.ToString();
                            dgvCurrentDepartments.Rows.Add(Convert.ToString(sc.reader["week_name"]), req_in.ToString("h:mm:ss tt"), req_out.ToString("h:mm:ss tt"), diff_hours);
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
            catch
            {
                MessageBox.Show("Please select");
            }
        }
        private void dgvCurrentDepartments_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            changeDepartmentStatisticsData();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (_next_department_page == DepartmenPages.page2)
            {
                _next_department_page = DepartmenPages.page1;
            }
            changeDepartmentStatisticsData();
        }

        private void dgvCurrentDepartments_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (_current_department_page == DepartmenPages.page1)
            {
                try
                {
                    _selected_department_id = Convert.ToString(dgvCurrentDepartments.CurrentRow.Cells[0].Value);
                }
                catch
                {
                    MessageBox.Show("Please select a row");
                }
            }
        }
    }
}