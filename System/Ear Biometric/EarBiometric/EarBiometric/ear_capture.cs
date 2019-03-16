using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
namespace EarBiometric
{
    public partial class ear_capture : Form
    {
        public ear_capture()
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
        ear_process ep = new ear_process();
        FormReactor _reactor = new FormReactor();
        public ear_structure es;
        VideoCapture _capture;
        Mat _frame = new Mat();
        Mat _ear_captured_uncropped;
        private int _ear_detected = 0;
        private int _delay_threshold = 3;
        private int _delay_holder = 0;
        private void ear_capture_Load(object sender, EventArgs e)
        {
            try
            {
                _capture = new VideoCapture(0);
                _capture.ImageGrabbed += landFrame;
            }
            catch (NullReferenceException excpt)
            {
                MessageBox.Show(excpt.Message);
            }
            _frame = new Mat();
            _delay_holder = _delay_threshold;
            tbRecognizeDelay.Text = _delay_holder.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private Mat extractSureEar(Mat ear)
        {
            Mat r = null;
            Mat[] detected = ep.detectEarImage(ear);
            if (detected[0].IsEmpty == false)
            {
                r = detected[0];
            }
            return r;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (_ear_captured_uncropped == null)
            {
                _reactor.showReactor(false, "Ear Image not found");
            }
            else if (tbEarName.Text == "")
            {
                _reactor.showReactor(false, "Ear name is mandatory");
            }
            else if (!ga.filterString2(tbEarName.Text))
            {
                _reactor.showReactor(false, "Invalid Ear name");
            }
            else
            {
                
                Mat ear_cropped = extractSureEar(_ear_captured_uncropped);
                if (ear_cropped != null) {
                    _capture.Dispose();
                    es.pushNewEar(ear_cropped, _ear_captured_uncropped, tbEarName.Text);
                    es.Show();
                    this.Close();
                }
                else
                {
                    _reactor.showReactor(false, "Ear image is not normalize properly, try it again");
                }
            }
        }

        // starts safe thread element
        private void invokeSetTBText(TextBox tb, string text)
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
        private bool checkEarImage(Mat a, bool webcam)
        {
            bool r = false;
            Mat[] detected = ep.detectEarImage(a);
            if (detected[0].IsEmpty == false)
            {
                pbCameraFrame.Image = detected[1].Bitmap;
                if (_delay_holder <= 0 || !webcam)
                {
                    if (!webcam)
                    {
                        invokeSetTBText(tbScanStatus, "Ear detected.");
                    }
                    else
                    { 
                        invokeSetTBText(tbScanStatus, "Normalizing ear...");
                    }
                    _ear_captured_uncropped = detected[2];
                    using (Mat pca_ear = ep.processEarPCA(detected[0])) {
                        pbEarCaptured.Image = pca_ear.Bitmap;
                    }
                    _ear_detected++;
                    invokeSetTBText(tbErrorsAttempt, _ear_detected.ToString());
                }
                else
                {
                    _delay_holder--;
                    ga.invokeSetTBText(tbRecognizeDelay, _delay_holder.ToString());
                    ga.invokeSetTBText(tbScanStatus, "Delaying...");
                }
                r = true;
            }
            else
            {
                _delay_holder = _delay_threshold;
                ga.invokeSetTBText(tbRecognizeDelay, _delay_holder.ToString());
            }
            return r;
        }
        private void landFrame(object sender, EventArgs arg)
        {
            if (_capture != null && _capture.Ptr != IntPtr.Zero)
            {
                _capture.Retrieve(_frame);
                if (!checkEarImage(_frame, true))
                {
                    invokeSetTBText(tbScanStatus, "Scanning ear...");
                    pbCameraFrame.Image = _frame.Bitmap;
                }
            }
        }

        private void changeButtonCaptureStyle(bool a)
        {
            if (a)
            {
                button4.BackColor = Color.FromArgb(230, 126, 34);
                button4.ForeColor = Color.White;
            }
            else
            {
                button4.BackColor = Color.FromArgb(46, 204, 113);
                button4.ForeColor = Color.White;
            }
        }
        private void clearBufferedData()
        {
            _delay_holder = _delay_threshold;
            _ear_detected = 0;
            invokeSetTBText(tbScanStatus, "IDLE");
            ga.invokeSetTBText(tbRecognizeDelay, _delay_holder.ToString());
            invokeSetTBText(tbErrorsAttempt, "0");
            pbCameraFrame.Image = null;
            timerClearBufferedData.Enabled = true;
        }
        private void stopCapturing()
        {
            _capture.Stop();
            button4.Text = "Scan";
            changeButtonCaptureStyle(false);
            clearBufferedData();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (button4.Text == "Scan")
            {
                _capture.Start();
                if (_capture.IsOpened) {
                    button4.Text = "Grab";
                    changeButtonCaptureStyle(true);
                }
                else
                {
                    _reactor.showReactor(false, "Hubble Camera not found");
                }
            }
            else
            {
                stopCapturing();
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            stopCapturing();
            _capture.Dispose();
            this.Hide();
            es.Show();
            timerFormCloser.Enabled = true;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
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

        private void timerClearBufferedData_Tick(object sender, EventArgs e)
        {
            clearBufferedData();
            timerClearBufferedData.Enabled = false;
        }

        private void pbUploadEarImage_Click(object sender, EventArgs e)
        {
            stopCapturing();
            DialogResult opf = ofdUploadEarImage.ShowDialog();
            if (opf == DialogResult.OK)
            {
                Mat img;
                try
                {
                    img = CvInvoke.Imread(ofdUploadEarImage.FileName);
                }
                catch
                {
                    _reactor.showReactor(false, "Invalid file format.");
                    return;
                }
                if (checkEarImage(img, false) == false)
                {
                    _reactor.showReactor(false, "No ear has been found in the image.");
                }
            }
        }

        private void timerFormCloser_Tick(object sender, EventArgs e)
        {
            timerFormCloser.Enabled = false;
            this.Close();
        }

        private void pbEarCaptured_Click(object sender, EventArgs e)
        {

        }
    }
}
