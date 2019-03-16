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
    public partial class form_user_identified : Form
    {
        public form_user_identified()
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

        // main classes starts here
        sql_connection sc = new sql_connection();
        global_arguments ga = new global_arguments();
        FormReactor _reactor = new FormReactor();
        public Form _from_form;
        int _minutes_to_timeout = 0;
        int _left_to_time_out = 0;
        int _close_fui_threshold = 7;
        private bool isUserActive(string user_id)
        {
            bool r = false;
            try
            {
                sc.open();
                sc.command = sc.SQLCommand("SELECT users.user_id FROM users WHERE users.user_id = '" + user_id + "' AND users.status = 'Active'");
                sc.reader = sc.command.ExecuteReader();
                if (sc.reader.Read())
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
        private string isEarValid(string angle_id)
        {
            string r = "not_valid";
            try
            {
                sc.open();
                sc.command = sc.SQLCommand("SELECT users.user_id FROM users INNER JOIN ears ON users.user_id = ears.user_id INNER JOIN angles ON ears.ear_id = angles.ear_id WHERE angles.angle_id = '" + angle_id + "'");
                sc.reader = sc.command.ExecuteReader();
                if (sc.reader.Read())
                {
                    r = Convert.ToString(sc.reader["user_id"]);
                }
                else
                {

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

        public bool processUserIdentification(string user_id, Form from_form)
        {
            bool r = false;
            _from_form = from_form;
            if (!isUserActive(user_id))
            {
                showUserInformation(user_id, "inactive");
                r = true;
            }
            else
            {
                string[] already_in = isUserInThisDay(user_id);
                string attendance_id = already_in[0];
                string time_in = already_in[1];
                if (attendance_id == "not")
                {
                    // Insert new attendance
                    string remarks = getUserRemarks(user_id, "in");
                    if (remarks == "no_schedule")
                    {
                        showUserInformation(user_id, "no_schedule");
                        r = true;
                    }
                    else if (remarks == "very-late")
                    {
                        showUserInformation(user_id, "very_late");
                        r = true;
                    }
                    else
                    {
                        // get the attendance ID
                        try
                        {
                            sc.open();
                            sc.command = sc.SQLCommand("INSERT INTO attendance(user_id, time_in, time_out, remarks, status) VALUES('" + user_id + "','" + ga.toSQLDateTime() + "','" + ga.toSQLDateTime() + "','" + remarks + "','in'); SELECT LAST_INSERT_ID()");
                            string last_id = Convert.ToString(sc.command.ExecuteScalar());
                            if (last_id != "0")
                            {
                                r = true;
                                sc.close();
                                // change to attendance ID from angle iD
                                showRemarks(last_id);
                                showUserInformation(user_id, "in");
                            }
                            else
                            {
                                MessageBox.Show("Error while inserting your time in the database");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error while inserting attendance data " + ex.Message);
                        }
                        finally
                        {
                            sc.close();
                        }
                    }
                }
                else
                {
                    string remarks = getUserRemarks(user_id, "out");
                    var time_in_minutes = (Convert.ToDateTime(ga.toSQLDateTime()).TimeOfDay - Convert.ToDateTime(time_in).TimeOfDay).Minutes;
                    if (time_in_minutes < _minutes_to_timeout)
                    {
                        _left_to_time_out = _minutes_to_timeout - time_in_minutes;
                        showUserInformation(user_id, "out_quickly");
                        r = true;
                    }
                    else
                    {
                        try
                        {
                            sc.open();
                            sc.command = sc.SQLCommand("UPDATE attendance SET time_out = '" + ga.toSQLDateTime() + "', remarks = CONCAT(remarks,'" + remarks + "'), status = 'out' WHERE attendance_id = '" + attendance_id + "' AND status != 'out'");
                            if (sc.command.ExecuteNonQuery() > 0)
                            {
                                r = true;
                                sc.close();
                                // change to attendance ID from angle iD
                                showRemarks(attendance_id);
                                showUserInformation(user_id, "out");
                            }
                            else
                            {
                                r = true;
                                sc.close();
                                showUserInformation(user_id, "out_already");
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
            return r;
        }

        public bool attemptUser(string angle_id, Form from_from)
        {
            bool r = false;
            string user_id = isEarValid(angle_id);
            if (user_id == "not_valid")
            {
                _reactor.showReactor(false, "The Recognized Ear Image not found in user information database. Angle ID: " + angle_id);
                r = true;
            }
            r = processUserIdentification(user_id, from_from);
            return r;
        }

        private string[] isUserInThisDay(string user_id)
        {
            string[] r = new string[2];
            string week_name = DateTime.Now.DayOfWeek.ToString();
            try
            {
                sc.open();
                sc.command = sc.SQLCommand("SELECT attendance_id, time_in FROM attendance WHERE user_id = '" + user_id + "' AND DATE(time_in) = DATE('" + ga.toSQLDateTime() + "')");
                sc.reader = sc.command.ExecuteReader();
                if (sc.reader.Read())
                {
                    r[0] = Convert.ToString(sc.reader["attendance_id"]);
                    r[1] = Convert.ToString(sc.reader["time_in"]);
                }
                else
                {
                    r[0] = "not";
                    r[1] = "not";
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

        private string getUserRemarks(string user_id, string status)
        {
            string r = "";
            string[] schedules = getUserSchedule(user_id);
            if (schedules[0] == "none")
            {
                r = "no_schedule";
            }
            else
            {
                var required_in = Convert.ToDateTime(schedules[0]).TimeOfDay;
                var required_out = Convert.ToDateTime(schedules[1]).TimeOfDay;

                // start algorithm of night shifter
                var date_today = Convert.ToDateTime(ga.toSQLDateTime()).TimeOfDay;
                if (status == "in")
                {
                    var current_time = ga.GlobalDateToday().TimeOfDay;
                    if (current_time > required_out)
                    {
                        r = "very-late";
                    }
                    else if (current_time > required_in)
                    {
                        int hours_late = Convert.ToInt32((current_time - required_in).TotalHours);
                        int minutes_late = Convert.ToInt32((current_time - required_in).TotalMinutes);
                        int seconds_late = Convert.ToInt32((current_time - required_in).TotalSeconds);
                        var time_late = "";
                        if (hours_late > 0)
                        {
                            string s = (hours_late > 1) ? " hours" : " hour";
                            time_late = hours_late.ToString() + s;
                        }
                        else if (minutes_late > 0)
                        {
                            string s = (minutes_late > 1) ? " minutes" : " minute";
                            time_late = minutes_late.ToString() + s;
                        }
                        else if (seconds_late > 0)
                        {
                            string s = (seconds_late > 1) ? " seconds" : " second";
                            time_late = seconds_late.ToString() + s;
                        }
                        r = "late&" + time_late + "-";
                    }
                    else
                    {
                        r = "in&00:00:00-";
                    }
                }
                else if (status == "out")
                {
                    if (ga.GlobalDateToday().TimeOfDay < required_out)
                    {
                        r = "early";
                    }
                    else
                    {
                        r = "out";
                    }
                }
            }
            return r;
        }

        private string[] getUserSchedule(string user_id)
        {
            string week_name = DateTime.Now.DayOfWeek.ToString();
            string[] schedules = new string[2];
            // 0 = in
            // 1 = out
            try
            {
                sc.open();
                sc.command = sc.SQLCommand("SELECT schedules.week_name, schedules.time_in, schedules.time_out FROM users_schedules INNER JOIN schedules ON users_schedules.schedule_id = schedules.schedule_id WHERE users_schedules.user_id = '" + user_id + "' AND schedules.week_name = '" + week_name + "'");
                sc.reader = sc.command.ExecuteReader();
                if (sc.reader.Read())
                {
                    schedules[0] = Convert.ToString(sc.reader["time_in"]);
                    schedules[1] = Convert.ToString(sc.reader["time_out"]);
                }
                else
                {
                    schedules[0] = "none";
                    schedules[1] = "none";
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
            return schedules;
        }

        
        private void showFUI()
        {
            ga.invokeHideForm(_from_form);
            //   timerCloseFUI.Enabled = true;
            _close_fui_threshold = 7;
            this.ShowDialog();
        }
        private void hideFUI()
        {
            this.Hide();
            ga.invokeShowForm(_from_form);
        }

        private void globalINStatus(bool a, string sign_status, string remarks)
        {
            if (a)
            {
                panelPIStatus.BackColor = Color.FromArgb(59, 89, 152);
            }
            else
            {
                panelPIStatus.BackColor = Color.FromArgb(231, 75, 58);
            }
            ga.invokeSetLabelText(labelSignStatus, sign_status);
            ga.invokeSetTBText(tbRemarksComment, remarks);
        }
        public void showUserInformation(string user_id, string is_in)
        {
            try
            {
                sc.open();
                sc.command = sc.SQLCommand("SELECT first_name, middle_name, last_name, (SELECT photo FROM photos WHERE user_id = users.user_id AND photo_type = 'profile') as photo FROM users WHERE users.user_id = '" + user_id + "'");
                sc.reader = sc.command.ExecuteReader();
                while (sc.reader.Read())
                {
                    DateTime t = DateTime.Now;
                    ga.invokeSetLabelText(labelCurrentTime, t.ToString("MMM dd") + " | " + t.DayOfWeek + " | " + t.ToString("h:mm tt"));
                    try
                    {
                        byte[] photo_bytes = (byte[])sc.reader["photo"];
                        pbProfilePhoto.Image = ga.byteToImage(photo_bytes);
                    }
                    catch
                    {
                        pbProfilePhoto.Image = Image.FromFile(ga.getResourcesPath() + "face-man.jpg");
                    }
                    string middle_name = Convert.ToString(sc.reader["middle_name"]);
                    if (middle_name.Length > 0)
                    {
                        middle_name = " " + middle_name[0] + ". ";
                    }
                    else
                    {
                        middle_name = " ";
                    }
                    string name = Convert.ToString(sc.reader["first_name"] + middle_name + sc.reader["last_name"]);
                    ga.invokeSetTBText(tbFullName, name);
                    if (is_in == "in")
                    {
                        // angle = attendance id
                    }
                    else if (is_in == "out")
                    {
                        // angle = attendance id
                    }
                    else if (is_in == "out_already")
                    {
                        globalINStatus(true, "Signed out.", "You've already signed out just now.");
                    }
                    else if (is_in == "out_quickly")
                    {
                        globalINStatus(false, "--", "You can sign out in " + _left_to_time_out + " minutes, ");
                    }
                    else if (is_in == "no_schedule")
                    {
                        globalINStatus(false, "No Schedule.", "Sorry, You don't have schedule today (" + DateTime.Now.DayOfWeek + "), cannot sign you in.");
                    }
                    else if (is_in == "very_late")
                    {
                        globalINStatus(false, "Very late.", "Sorry, You are very late cannot sign you in today.");
                    }
                    else if (is_in == "inactive")
                    {
                        globalINStatus(false, "Inactive.", "Sorry, You are now inactive from this department.");
                    }
                    showFUI();
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

        private void showRemarks(string attendance_id)
        {
            try
            {
                sc.open();
                sc.command = sc.SQLCommand("SELECT * FROM attendance WHERE attendance_id = '" + attendance_id + "'");
                sc.reader = sc.command.ExecuteReader();
                if (sc.reader.Read())
                {
                    string time_in = Convert.ToString(sc.reader["time_in"]);
                    string time_out = Convert.ToString(sc.reader["time_out"]);
                    string f_remarks = Convert.ToString(sc.reader["remarks"]);
                    string status = Convert.ToString(sc.reader["status"]);
                    string[] remarks = ga.analyzeUserRemarks(time_in, time_out, f_remarks, status);
                    if (status == "in")
                    {
                        globalINStatus(true, "Signed in.", remarks[0]);
                    }else if (status == "out")
                    {
                        globalINStatus(true, "Signed out.", remarks[1]);
                    }

                    if (remarks[3] == "late" || remarks[4] == "early")
                    {
                        tbRemarksComment.ForeColor = Color.Red;
                    }
                    else
                    {
                        tbRemarksComment.ForeColor = Color.FromArgb(75, 79, 86);
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
        private void form_user_identified_Load(object sender, EventArgs e)
        {
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
            hideFUI();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            hideFUI();
        }

        private void timerCloseFUI_Tick(object sender, EventArgs e)
        {
            if (_close_fui_threshold == 0)
            {
                hideFUI();
                timerCloseFUI.Enabled = false;
            }
            _close_fui_threshold--;
        }
    }
}
