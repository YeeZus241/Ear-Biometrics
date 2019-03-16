using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;

namespace EarBiometric
{
    public partial class ear_scanner : Form
    {
        public ear_scanner()
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
        global_arguments ga = new global_arguments();
        sql_connection sc = new sql_connection();
        ear_process ep = new ear_process();
        FormReactor _reactor;
        form_user_identified _fui;
        VideoCapture _capture;
        Mat _frame;
        public Form1 _home;
        string[] _db_ears;
        private bool _buffering_identified_ear = false;
        private int _delay_threshold = 7;
        private int _delay_holder = 0;
        private int _attempt_recognize = 0;
        private int _attempt_threshold = 5;
        List<long> _average_recognition_time = new List<long>();
        long _last_average_recognition_time = 0;
        Mat _uploaded_ear = null;
        private void initializeParentForms()
        {
            _reactor = new FormReactor();
            _fui = new form_user_identified();
        }
        private void globalInitializer()
        {
            _db_ears = Directory.GetFiles(ga.getEarsCroppedDatabase(), "*" + ga._global_ear_extension, SearchOption.AllDirectories);
            initializeParentForms();
            if (_db_ears.Length == 0)
            {
                _reactor.showReactor(false, "Warning no ears has been registered.");
            }
            try
            {
                _capture = new VideoCapture(0);
                //_capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameWidth, 325);
                //_capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameHeight, 250);
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
        private void ear_scanner_Load(object sender, EventArgs e)
        {
            globalInitializer();
        }

        private void calculateAverageRecognitionTime()
        {
            int stack_count = _average_recognition_time.Count;
            long sum = 0;
            for (var i = 0; i < stack_count; i++)
            {
                sum += +_average_recognition_time[i];
            }
            _last_average_recognition_time = sum / stack_count;
        }

        private bool recognizeEarImage(Mat ear_observed)
        {
            bool r = false;
            Stopwatch watch = new Stopwatch();
            watch.Start();

            // here start comparing

            // this approach fetch all ear record in database and get the highest feature score, using this is slower but increase the accuracy

            string angle_id = ep.recognizeEarHighestSimilarity(_db_ears, ear_observed);
            watch.Stop();
            _average_recognition_time.Add(watch.ElapsedMilliseconds);
            calculateAverageRecognitionTime();

            if (angle_id != "not_found")
            {
                if (!_fui.attemptUser(angle_id, this))
                {
                    MessageBox.Show("Unknow error while processing user database");
                }
                r = true;
            }

            return r;
        }

        // starts safe thread element
        delegate void SetTextElementCallback(string a);
        private void SetTextElement_button2(string text)
        {
            if (this.button2.InvokeRequired)
            {
                SetTextElementCallback d = new SetTextElementCallback(SetTextElement_button2);
                button2.Invoke(d, new object[] { text });
            }
            else
            {
                this.button2.Text = text;
            }
        }

        private bool checkEarImage(Mat frame, bool webcam, bool recognize)
        {
            bool r = false;
            if (_buffering_identified_ear == false)
            {
                Mat[] detected = ep.detectEarImage(frame);
                if (!detected[0].IsEmpty)
                {
                    pictureBox1.Image = detected[1].Bitmap;
                    if (recognize) {
                        if (_delay_holder <= 0 || !webcam)
                        {
                            _buffering_identified_ear = true;
                            ga.invokeSetTBText(tbScanStatus, "Recognizing ear...");
                            if (recognizeEarImage(detected[0]))
                            {
                                stopCapturing();
                            }
                            else
                            {
                                _attempt_recognize++;
                                ga.invokeSetTBText(tbErrorsAttempt, _attempt_recognize.ToString());
                                _buffering_identified_ear = false;
                                if (!webcam)
                                {
                                    _reactor.showReactor(false, "No match found in the database.");
                                    stopCapturing();
                                }
                                if (_attempt_recognize == _attempt_threshold)
                                {
                                    _reactor.showReactor(false, "No match found after " + _attempt_threshold + " attempt to recognize.");
                                    stopCapturing();
                                }
                            }
                        }
                        else
                        {
                            _delay_holder--;
                            ga.invokeSetTBText(tbRecognizeDelay, _delay_holder.ToString());
                            ga.invokeSetTBText(tbScanStatus, "Delaying...");
                        }
                    }else if (!webcam && !recognize)
                    {
                        ga.invokeSetTBText(tbScanStatus, "Ear detected.");
                        SetTextElement_button2("Recognize");
                    }
                    r = true;
                }
                else
                {
                    _delay_holder = _delay_threshold;
                    ga.invokeSetTBText(tbRecognizeDelay, _delay_holder.ToString());
                }
            }
            return r;
        }
        private void landFrame(object sender, EventArgs arg)
        {
            if (_capture != null && _capture.Ptr != IntPtr.Zero)
            {
                _capture.Retrieve(_frame);
                if (!checkEarImage(_frame, true, true))
                {
                    pictureBox1.Image = _frame.Bitmap;
                    ga.invokeSetTBText(tbScanStatus, "Scanning ear...");
                }
            }
        }

        private void stopCapturing()
        {
            _capture.Stop();
            button2.Enabled = true;
            ga.changeButtonCaptureStyle(button2, false);
            SetTextElement_button2("Scan");
            clearBufferedData();
        }
        private void clearBufferedData()
        {
            _delay_holder = _delay_threshold;
            _buffering_identified_ear = false;
            _attempt_recognize = 0;
            _uploaded_ear = null;
            pictureBox1.Image = null;
            _average_recognition_time.Clear();
            ga.invokeSetTBText(tbErrorsAttempt, "0");
            ga.invokeSetTBText(tbRecognizeDelay, _delay_holder.ToString());
            ga.invokeSetTBText(tbScanStatus, "IDLE");
            timerClearBufferedData.Enabled = true;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Text == "Scan" || button2.Text == "Recognize")
            {
                if (_uploaded_ear != null) {
                    SetTextElement_button2("Recognizing...");
                    button2.Enabled = false;
                    checkEarImage(_uploaded_ear, false, true);
                }
                else {
                    _capture.Start();
                    if (_capture.IsOpened) {
                        SetTextElement_button2("Stop");
                        ga.changeButtonCaptureStyle(button2, true);
                    }
                    else
                    {
                        _reactor.showReactor(false, "Hubble Camera not found");
                    }
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
            _home.Show();
            timerFormCloser.Enabled = true;
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
            DialogResult dr = ofdUploadEarImage.ShowDialog();
            if (dr == DialogResult.OK)
            {
                Mat ear;
                try
                {
                    ear = CvInvoke.Imread(ofdUploadEarImage.FileName);
                }
                catch
                {
                    _reactor.showReactor(false, "Invalid file format.");
                    return;
                }
                if (checkEarImage(ear, false, false) == false)
                {
                    _reactor.showReactor(false, "No ear has been found in the image.");
                }
                else
                {
                    _uploaded_ear = ear;
                }
            }
        }

        private void timerFormCloser_Tick(object sender, EventArgs e)
        {
            timerFormCloser.Enabled = false;
            this.Close();
        }

        private void pbAverageRecognition_Click(object sender, EventArgs e)
        {
            _reactor.showReactor(true, String.Format("Average Recognition time: {0} Milliseconds", _last_average_recognition_time));
        }
        FormManualLogin _form_manual_login;
        private void pbManualLogin_Click(object sender, EventArgs e)
        {
            stopCapturing();
            ga.invokeHideForm(this);
            _form_manual_login = new FormManualLogin();
            _form_manual_login.es = this;
            _form_manual_login.Show();
        }
    }
}
