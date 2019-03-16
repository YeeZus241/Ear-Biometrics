using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
namespace EarBiometric
{
    class global_arguments
    {
        sql_connection sc = new sql_connection();
        public string[] _week_names = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
        public string _global_ear_extension = ".png";
        public string toSQLDateTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
        public DateTime GlobalDateToday()
        {
            return Convert.ToDateTime(DateTime.Now);
        }
        public string convertToSQLDateTime(DateTime a)
        {
            return a.ToString("yyyy-MM-dd HH:mm:ss");
        }

        // hashing password method using MD5 algorithm
        public string toMD5(string a)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(a));
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                // Return the hexadecimal string.
                return sBuilder.ToString();
            }

        }

        // @"^\w+$" for username
        // @"^[a-zA-Z]+$" for letters only
        // @"^[a-zA-Z0-9() -]+$"
        private bool composeRegexStringFilter(string a, string b)
        {
            bool r = true;
            Regex letters = new Regex(b);
            if (!letters.IsMatch(a))
            {
                r = false;
            }
            return r;
        }
        // letters only
        public bool filterString1(string a)
        {
            return composeRegexStringFilter(a, @"^[a-zA-Z ]+$");
        }
        // allows a-z A-Z 0-9 ()
        public bool filterString2(string a)
        {
            return composeRegexStringFilter(a, @"^[a-zA-Z0-9_+() -]+$");
        }
        // for username
        public bool filterString3(string a)
        {
            return composeRegexStringFilter(a, @"^\w+$");
        }
        private string renewPath(int rev)
        {
            string path = "";
            string[] renew_path = Directory.GetCurrentDirectory().Split('\\');
            for (int i = 0; i < renew_path.Length - rev; i++)
            {
                path += renew_path[i] + "\\";
            }
            return path;
        }

        public string getResourcesPath()
        {
            return renewPath(2) + "Resources\\";
        }

        public string getComponentsPath()
        {
            return renewPath(3) + "data\\components\\";
        }
        public string getEarsCroppedDatabase()
        {
            return renewPath(3) + "data\\database\\ears\\cropped\\";
        }

        public string getEarsUncroppedDatabase()
        {
            return renewPath(3) + "data\\database\\ears\\uncropped\\";
        }

        // Convert an image to byte (Usually used for storing image to database)
        public byte[] imageToByte(Image ear)
        {
            Bitmap bmp = new Bitmap(ear);
            MemoryStream ms = new MemoryStream();
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            //ear.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] r = ms.ToArray();
            return r;
        }

        // Convert byte to image (Usually used for fetching image from database)
        public Image byteToImage(byte[] ear)
        {
            byte[] ear_image = (byte[])ear;
            MemoryStream ms = new MemoryStream(ear_image);
            Bitmap bmp = new Bitmap(Image.FromStream(ms));
            return bmp;
        }

        public string getScheduleID(string week, string time_in, string time_out)
        {
            string r = "";
            try
            {
                sc.open();
                sc.command = sc.SQLCommand("SELECT schedule_id FROM schedules WHERE week_name = '" + week + "' AND TIME(time_in) = '" + time_in + "' AND TIME(time_out) = '" + time_out + "'");
                sc.reader = sc.command.ExecuteReader();
                if (sc.reader.Read())
                {
                    r = Convert.ToString(sc.reader["schedule_id"]);
                }
                else
                {
                    sc.close();
                    sc.open();
                    sc.command = sc.SQLCommand("INSERT INTO schedules(week_name, time_in, time_out) VALUES('" + week + "','" + time_in + "','" + time_out + "'); SELECT LAST_INSERT_ID()");
                    r = Convert.ToString(sc.command.ExecuteScalar());
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

        public bool checkScheduleTime(DateTime time_in, DateTime time_out)
        {
            bool r = true;
            // timeout cannot be am , except tt_in == tt_out
            string tt_in = time_in.ToString("tt");
            string tt_out = time_out.ToString("tt");
            if (time_in >= time_out || tt_in == "PM" && tt_out == "AM")
            {
                r = false;
            }
            return r;
        }

        public void applyDGVColumnPageStyle(DataGridView dgv, List<string> names)
        {
            dgv.Columns.Clear();
            dgv.ColumnCount = names.Count;
            for (var i = 0; i < names.Count; i++)
            {
                dgv.Columns[i].HeaderText = names[i];
            }
        }

        public string[] analyzeUserRemarks(string time_in, string time_out, string f_remarks, string status)
        {
            string[] r = new string[5];
            string[] remarks = Convert.ToString(f_remarks).Split('-');
            string[] in_components = remarks[0].Split('&');
            time_in = Convert.ToDateTime(time_in).ToString("hh:mm tt");
            time_out = Convert.ToDateTime(time_out).ToString("hh:mm tt");
            var status_in = (in_components[0] == "late") ? time_in + " (late " + in_components[1] + ")" : time_in;
            var status_out = (remarks[1] == "early") ? time_out + " (early out)" : time_out;
            var final_remarks = "";
            if (status == "out")
            {
                final_remarks = (remarks[0] != "late" && remarks[1] != "early") ? "completed" : "Incomplete";
            }
            else
            {
                final_remarks = "Not logged out";
                status_out = "Not logged out";
            }
            r[0] = status_in;
            r[1] = status_out;
            r[2] = final_remarks;
            r[3] = in_components[0];
            r[4] = remarks[1];
            return r;
        }

        public void changeButtonCaptureStyle(Button btn, bool a)
        {
            if (a)
            {
                btn.BackColor = Color.FromArgb(230, 126, 34);
                btn.ForeColor = Color.White;
            }
            else
            {
                btn.BackColor = Color.FromArgb(46, 204, 113);
                btn.ForeColor = Color.White;
            }
        }

        public void invokeShowForm(Form f)
        {
            if (f.InvokeRequired)
            {
                f.Invoke((Action)delegate { f.Show(); f.BringToFront(); });
            }
            else
            {
                f.Show();
            }
        }

        public void invokeHideForm(Form f)
        {
            if (f.InvokeRequired)
            {
                f.Invoke((Action)delegate { f.Hide(); });
            }
            else
            {
                f.Hide();
            }
        }

        public void invokeSetTBText(TextBox tb, string text)
        {
            if (tb.InvokeRequired)
            {
                tb.Invoke((Action)delegate { tb.Text = text; });
            }
            else
            {
                tb.Text = text;
            }
        }

        public double calculateDecreasePercentage(int value_original, int value_new)
        {
            double difference = 0;
            double decerase = value_original - value_new;
            decerase = (decerase / value_original) * 100;
            difference = (100 - decerase);
            return difference;
        }

        public void invokeSetLabelText(Label lb, string text)
        {
            if (lb.InvokeRequired)
            {
                lb.Invoke((Action)delegate { lb.Text = text; });
            }
            else
            {
                lb.Text = text;
            }
        }
    }
}
