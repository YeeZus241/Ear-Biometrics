using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using MySql.Data;
using MySql.Data.MySqlClient;
using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.Structure;

namespace EarBiometric
{
    public partial class ear_structure : Form
    {
        public ear_structure()
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
        global_arguments ga = new global_arguments();
        sql_connection sc = new sql_connection();
        ear_process ep = new ear_process();
        // END
        private string _user_id = "";
        FormReactor _reactor = new FormReactor();
        public manage_users mu;
        List<Mat> _ears_cropped = new List<Mat>();
        List<Mat> _ears_uncropped = new List<Mat>();
        List<string> _new_ears_name = new List<string>();
        public void loadEarStructure(string user_id)
        {
            _user_id = user_id;
            // starts fetch from database
            dataGridView1.Rows.Clear();
            try
            {
                sc.open();
                sc.command = sc.SQLCommand("SELECT * FROM angles AS a INNER JOIN ears AS e ON a.ear_id = e.ear_id WHERE e.user_id = '" + user_id + "'");
                sc.reader = sc.command.ExecuteReader();
                while (sc.reader.Read())
                {
                    dataGridView1.Rows.Add(Convert.ToString(sc.reader["angle_id"]), Convert.ToString(sc.reader["description"]), Convert.ToDateTime(sc.reader["date_added"]).ToString("MMM dd, yyyy - h:mm tt"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database error: " + ex.Message);
            }
            finally
            {
                sc.close();
            }
        }
        private void ear_structure_Load(object sender, EventArgs e)
        {
            sc.activate_SQLConnection();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            mu.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            ear_capture ec = new ear_capture();
            ec.es = this;
            ec.Show();
        }
        // DateTime.Now.ToString("MMM dd, yyyy - hh:mm tt") will be added when fetched from database

        public void reloadEarsData()
        {
            if (_user_id != "")
            {
                loadEarStructure(_user_id);
            }
            else
            {
                dataGridView1.Rows.Clear();
            }
            for (var i = 0; i < _ears_cropped.Count; i++)
            {
                Mat ear = _ears_cropped[i];
                string name = _new_ears_name[i];
                dataGridView1.Rows.Add(Convert.ToString("new_" + i), name, ga.toSQLDateTime());
            }
        }
        public void pushNewEar(Mat cropped, Mat uncropped, string name)
        {
            _ears_cropped.Add(cropped);
            _ears_uncropped.Add(uncropped);
            _new_ears_name.Add(name);
            reloadEarsData();
        }

        private void activateCurrentCell()
        {
            try
            {
                string angle_id = Convert.ToString(dataGridView1.CurrentRow.Cells[0].Value);
                string[] new_ear = angle_id.Split('_');
                if (new_ear.Length == 2)
                {
                    int stack_count = Convert.ToInt32(new_ear[1]);
                    if (cbViewCannyEdge.Checked)
                    {
                        pictureBox1.Image = ep.processEarPCA(_ears_cropped[stack_count]).Bitmap;
                    }
                    else
                    {
                        pictureBox1.Image = _ears_cropped[stack_count].Bitmap;
                    }
                }
                else
                {
                    string ear_path = ga.getEarsCroppedDatabase() + angle_id + ga._global_ear_extension;
                    if (File.Exists(ear_path))
                    {
                        using (Mat ear_read = CvInvoke.Imread(ear_path))
                        {
                            if (cbViewCannyEdge.Checked)
                            {
                                pictureBox1.Image = ep.processEarPCA(ear_read).Bitmap;
                            }
                            else
                            {
                                pictureBox1.Image = ear_read.Bitmap;
                            }
                        }
                    }
                    else
                    {
                        pictureBox1.Image = null;
                        _reactor.showReactor(false, "The ear Angle ID: " + angle_id + " not found in the database.");
                    }
                }
            }
            catch
            {
                _reactor.showReactor(false, "Select Angle");
            }
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            activateCurrentCell();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            mu.Show();
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

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string angle_id = Convert.ToString(dataGridView1.CurrentRow.Cells[0].Value);
                var conf = MessageBox.Show("Are you sure you want to delete this angle?", "Delete ear angle", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (conf == DialogResult.Yes)
                {
                    string[] new_ear = angle_id.Split('_');
                    if (new_ear.Length == 2)
                    {
                        int stack_count = Convert.ToInt32(new_ear[1]);
                        _ears_cropped.RemoveAt(stack_count);
                        _new_ears_name.RemoveAt(stack_count);
                        reloadEarsData();
                    }
                    else
                    {
                        string ear_path = ga.getEarsCroppedDatabase() + angle_id + ga._global_ear_extension;
                        File.Delete(ear_path);
                        if (File.Exists(ear_path))
                        {
                            MessageBox.Show("File is currently using in other program, cannot delete this time");
                        }
                        else
                        {
                            try
                            {
                                sc.open();
                                sc.command = sc.SQLCommand("DELETE FROM angles WHERE angle_id = '" + angle_id + "'");
                                if (sc.command.ExecuteNonQuery() > 0)
                                {
                                    sc.close();
                                    loadEarStructure(_user_id);
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
            catch
            {
                _reactor.showReactor(false, "Select Angle to delete");
            }
        }

        private string getEarID(string user_id)
        {
            string r = "";
            try
            {
                sc.open();
                sc.command = sc.SQLCommand("SELECT ear_id FROM ears WHERE user_id = '" + user_id + "'");
                r = Convert.ToString(sc.command.ExecuteScalar());
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

        public void updateUserEars(string user_id)
        {
            string ear_id = getEarID(user_id);
            if (ear_id != "" && ear_id != null)
            {
                try
                {
                    sc.open();
                    for (var i = 0; i < _ears_cropped.Count; i++)
                    {
                        Mat cropped = _ears_cropped[i];
                        Mat uncropped = _ears_uncropped[i];
                        byte[] ear_bytes = ga.imageToByte(cropped.Bitmap);
                        string name = _new_ears_name[i];
                        string date_added = ga.toSQLDateTime();
                        sc.command = sc.SQLCommand("INSERT INTO angles(ear_id, ear_image, description, date_added) VALUES('" + ear_id + "', @ear_image,'" + name + "','" + date_added + "'); SELECT LAST_INSERT_ID()");
                        sc.command.Parameters.Add("@ear_image", MySqlDbType.Blob).Value = ear_bytes;
                        string last_id = Convert.ToString(sc.command.ExecuteScalar());
                        if (last_id != "" && last_id != null)
                        {
                            // save for cropped ear

                            //using (Mat pca_cropped = ep.processEarPCA(cropped))
                            //{
                            //    // use this method if you want to trained directly to database

                            //}

                            if (!(ep.saveEarImageToFolder(ga.getEarsCroppedDatabase(), last_id, cropped)))
                            {
                                _reactor.showReactor(false, "Error while saving cropped ear image to folder.");
                            }


                            // save for uncropped ear
                            if (!Directory.Exists(ga.getEarsUncroppedDatabase() + user_id))
                            {
                                Directory.CreateDirectory(ga.getEarsUncroppedDatabase() + user_id);
                            }

                            if (!ep.saveEarImageToFolder(ga.getEarsUncroppedDatabase() + user_id + "\\", last_id, uncropped))
                            {
                                _reactor.showReactor(false, "Error while saving uncropped ear image to folder.");
                            }
                        }
                        else
                        {
                            _reactor.showReactor(false, "Error while inserting Angle ID in the database.");
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
            else
            {
                MessageBox.Show("Error while finding Ear ID");
            }
        }

        private void cbViewCannyEdge_CheckedChanged(object sender, EventArgs e)
        {
            activateCurrentCell();
        }
    }
}
