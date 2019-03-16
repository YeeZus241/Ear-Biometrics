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
    public partial class FormReactor : Form
    {
        public FormReactor()
        {
            InitializeComponent();
        }
        global_arguments ga = new global_arguments();

        public void showReactor(bool a, string msg)
        {
            textBox1.Text = msg;
            string icon_success_path = ga.getResourcesPath() + "checked (1).png";
            string icon_error_path = ga.getResourcesPath() + "cancel (1)1.png";
            if (a)
            {
                pictureBox1.Image = Image.FromFile(icon_success_path);
                this.Show();
                timerCloseReactor.Enabled = true;
            }
            else
            {
                pictureBox1.Image = Image.FromFile(icon_error_path);
                timerShake.Enabled = true;
                try
                {
                    this.ShowDialog();
                }
                catch { }
            }
            this.BringToFront();
        }
        private void FormReactor_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        int[] shake_array = { 15, 30, 15, 0, -15, -30, -15, 0, 15, 30, 15, 0, -15, -30, -15, 0, 15, 30, 15, 0, -15, -30, -15, 0 };
        private void shake(int i)
        {
            this.Left += shake_array[i];
        }
        int s = 0;
        private void timer_shake_Tick(object sender, EventArgs e)
        {
            
        }

        private void timerShake_Tick(object sender, EventArgs e)
        {
            shake(s);
            s++;
            if (s == shake_array.Length - 1)
            {
                s = 0;
                timerShake.Enabled = false;
            }
        }

        private void timerCloseReactor_Tick(object sender, EventArgs e)
        {
            this.Hide();
            timerCloseReactor.Enabled = false;
        }
    }
}
