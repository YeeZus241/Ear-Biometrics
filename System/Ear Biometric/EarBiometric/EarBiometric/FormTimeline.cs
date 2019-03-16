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
    public partial class FormTimeline : Form
    {
        public FormTimeline()
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

        public string _user_id { get ; set; }
        public manage_users _mu { get; set; }

        // start initialize main classes
        sql_connection sc = new sql_connection();
        global_arguments ga = new global_arguments();
        private string[] _month_names = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;
        private enum GridTimiline
        {
            Years,
            Months,
            Days
        }
        private GridTimiline _current_grid_timeline = GridTimiline.Years;
        private void loadUserTimeline(string user_id)
        {
            // profiles
            try
            {
                sc.open();
                sc.command = sc.SQLCommand("SELECT users.first_name, users.last_name, photos.photo FROM users INNER JOIN photos ON users.user_id = photos.user_id WHERE users.user_id = '" + user_id + "' AND photo_type = 'profile'");
                sc.reader = sc.command.ExecuteReader();
                while (sc.reader.Read())
                {
                    byte[] photo_bytes = (byte[])sc.reader["photo"];
                    pbProfilePhoto.Image = ga.byteToImage(photo_bytes);
                    tbProfileName.Text = Convert.ToString(sc.reader["first_name"] + " " + sc.reader["last_name"]);
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
        private string getLowestAttendanceDate(string user_id)
        {
            string r = "";
            try
            {
                sc.open();
                sc.command = sc.SQLCommand("SELECT MIN(time_in) AS lowest FROM attendance WHERE user_id = '" + user_id + "'");
                sc.reader = sc.command.ExecuteReader();
                while (sc.reader.Read())
                {
                    r = Convert.ToString(sc.reader["lowest"]);
                }
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
        private int _entered_year = 0;
        private int _entered_month = 0;
        private void changeStatisticData()
        {
            if (_current_grid_timeline == GridTimiline.Years)
            {
                List<string> names = new List<string>() {"Year","Total IN"};
                ga.applyDGVColumnPageStyle(dgvTimeline, names);
                string lowest = getLowestAttendanceDate(_user_id);
                if (lowest != "") {
                    try
                    {
                        string year = Convert.ToDateTime(lowest).ToString("yyyy");
                        var year_today = Convert.ToDateTime(ga.toSQLDateTime()).ToString("yyyy");
                        for (var i=Convert.ToInt32(year);i<=Convert.ToInt32(year_today);i++) {
                            try
                            {
                                sc.open();
                                sc.command = sc.SQLCommand("SELECT COUNT(time_in) AS years FROM attendance WHERE user_id = '" + _user_id + "' AND YEAR(time_in) = '" + i + "'");
                                sc.reader = sc.command.ExecuteReader();
                                while (sc.reader.Read())
                                {
                                    dgvTimeline.Rows.Add(i.ToString(), Convert.ToString(sc.reader["years"]));
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
                        MessageBox.Show("Unknown error");
                    }
                }
                else
                {
                    MessageBox.Show("No date to show");
                }
            }
            else if (_current_grid_timeline == GridTimiline.Months)
            {
                List<string> names = new List<string>() { "Month", "Total IN","Lates" };
                ga.applyDGVColumnPageStyle(dgvTimeline, names);
                for (var i=0;i<_month_names.Length;i++)
                {
                    string month = _month_names[i];
                    if (month != "") {
                        try
                        {
                            sc.open();
                            sc.command = sc.SQLCommand("SELECT COUNT(time_in) AS c, (SELECT COUNT(time_in) FROM attendance WHERE user_id = '" + _user_id + "' AND YEAR(time_in) = '" + _entered_year + "' AND MONTH(time_in) = '" + (i + 1) + "' AND remarks LIKE '%late%') AS lates, (SELECT COUNT(time_in) FROM attendance WHERE user_id = '" + _user_id + "' AND YEAR(time_in) = '" + _entered_year + "' AND MONTH(time_in) = '" + (i + 1) + "' AND remarks LIKE '%early%') AS early_out FROM attendance WHERE user_id = '" + _user_id + "' AND YEAR(time_in) = '" + _entered_year + "' AND MONTH(time_in) = '" + (i+1) + "'");
                            sc.reader = sc.command.ExecuteReader();
                            while (sc.reader.Read())
                            {
                                dgvTimeline.Rows.Add(month, Convert.ToString(sc.reader["c"]),"late " + Convert.ToString(sc.reader["lates"]) + ", early out " + Convert.ToString(sc.reader["early_out"]));
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
                }
            }
            else if (_current_grid_timeline == GridTimiline.Days)
            {
                List<string> names = new List<string>() { "Day", "In-Status", "Out-Status", "Remarks" };
                ga.applyDGVColumnPageStyle(dgvTimeline, names);

                // days in month || check remarks
                int days = DateTime.DaysInMonth(_entered_year, _entered_month);
                for (var i = 1; i <= days; i++)
                {
                    try
                    {
                        sc.open();
                        sc.command = sc.SQLCommand("SELECT * FROM attendance WHERE user_id = '" + _user_id + "' AND YEAR(time_in) = '" + _entered_year + "' AND MONTH(time_in) = '" + _entered_month + "' AND DAY(time_in) = '" + i + "'");
                        sc.reader = sc.command.ExecuteReader();
                        while (sc.reader.Read())
                        {
                            string[] remarks = ga.analyzeUserRemarks(Convert.ToString(sc.reader["time_in"]), Convert.ToString(sc.reader["time_out"]), Convert.ToString(sc.reader["remarks"]), Convert.ToString(sc.reader["status"]));
                            dgvTimeline.Rows.Add(i.ToString(), remarks[0], remarks[1], remarks[2]);
                            if (remarks[2] == "Incomplete") {
                                dgvTimeline.Rows[dgvTimeline.Rows.Count - 1].Cells[3].Style.ForeColor = Color.Red;
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
            }

        }
        private void FormTimeline_Load(object sender, EventArgs e)
        {
            loadUserTimeline(_user_id);
            changeStatisticData();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            _mu.Show();
            this.Close();
        }
        private void timelineGridBackwards()
        {
            if (_current_grid_timeline == GridTimiline.Days)
            {
                _current_grid_timeline = GridTimiline.Months;
            }else if (_current_grid_timeline == GridTimiline.Months)
            {
                _current_grid_timeline = GridTimiline.Years;
            }
            changeStatisticData();
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            timelineGridBackwards();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        
        private void dgvTimeline_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string year = Convert.ToString(dgvTimeline.CurrentRow.Cells[0].Value);
                if (_current_grid_timeline == GridTimiline.Years)
                {
                    _current_grid_timeline = GridTimiline.Months;
                    _entered_year = Convert.ToInt32(year);
                    //clearing combo box year
                    cbYears.Items.Clear();
                    cbYears.Items.Add(_entered_year);
                    cbYears.Text = _entered_year.ToString();
                }else if (_current_grid_timeline == GridTimiline.Months)
                {
                    _current_grid_timeline = GridTimiline.Days;
                    // convert month name to number
                    for (var i = 0; i < _month_names.Length; i++)
                    {
                        if (_month_names[i] == year)
                        {
                            _entered_month = i + 1;
                            break;
                        }
                    }
                    //clearing combo box months
                    cbMonths.Items.Clear();
                    cbMonths.Items.Add(_month_names[_entered_month-1]);
                    cbMonths.Text = _month_names[_entered_month - 1];
                }
                changeStatisticData();
            }
            catch
            {
                MessageBox.Show("Please select a row");
            }
        }
    }
}
