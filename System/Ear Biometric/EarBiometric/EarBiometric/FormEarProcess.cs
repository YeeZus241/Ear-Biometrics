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
using System.Threading;
using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.XFeatures2D;
using Emgu.CV.Flann;
using Emgu.CV.Structure;
namespace EarBiometric
{
    public partial class FormEarProcess : Form
    {
        public FormEarProcess()
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

        //initialize classes
        global_arguments ga = new global_arguments();
        sql_connection sc = new sql_connection();
        ear_process ep = new ear_process();
        FormReactor _reactor = new FormReactor();
        public Form1 _home { get; set; }
        FormEPGlobal _ep_global;
        VideoCapture _capture;
        Mat _frame = new Mat();
        Mat _ear_detected;
        Rectangle _ear_ROI;
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            stopCapturing();
            _capture.Dispose();
            _home.Show();
            this.Hide();
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

        private void FormEarProcess_Load(object sender, EventArgs e)
        {
            _ep_global = new FormEPGlobal();
            _ep_global._fep = this;
            try
            {
                _capture = new VideoCapture(0);
                //_capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameWidth, 325);
                //_capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameHeight, 250);
                _capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.Fps);
                _capture.ImageGrabbed += landFrame;
            }
            catch (NullReferenceException excpt)
            {
                MessageBox.Show(excpt.Message);
            }
            tbMedianPixel.Text = ep._median_threshold.ToString();
            tbEdgeThreshold1.Text = ep._edge_threshold_1.ToString();
            tbEdgeThreshold2.Text = ep._edge_threshold_2.ToString();
            //Mat prep = CvInvoke.Imread("C:\\Users\\fukui\\Desktop\\Computer Vision report\\17757360_1248847381899421_7400962068647398150_n.jpg");
            //Mat canny = new Mat();
            //Mat gaussian = new Mat();
            //CvInvoke.Canny(prep, canny, 50, 150);
            //CvInvoke.GaussianBlur(prep, gaussian, new Size(25, 25), 3.7, 3.7);
            //CvInvoke.Imshow("Bortong's Smile", gaussian) ;
        }

        private bool checkEarImage(Mat frame)
        {
            bool r = false;
            Mat[] detected_ear = detectEarImage(frame);
            if (!detected_ear[0].IsEmpty)
            {
                pbCamera.Image = detected_ear[1].Bitmap;
                _ear_detected = detected_ear[0];
                globalProcessEar();
                r = true;
            }
            return r;
        }

        private void landFrame(object sender, EventArgs arg)
        {
            if (_capture != null && _capture.Ptr != IntPtr.Zero)
            {
                _capture.Retrieve(_frame);
                if (!checkEarImage(_frame))
                {
                    pbCamera.Image = _frame.Bitmap;
                }
            }
        }

        public Mat[] detectEarImage(Mat a)
        {
            Mat[] ear_imgs = new Mat[2];
            ear_imgs[0] = new Mat();
            ear_imgs[1] = new Mat();
            Rectangle roi = new Rectangle();
            Rectangle[] left_ear;
            Rectangle[] right_ear;
            bool is_detected = false;
            try
            {
                using (Mat gray = ep.PCAEarGray(a))
                {
                    using (Mat histogram = ep.PCAEarHistogram(gray))
                    {
                        left_ear = ep.detector_left_ear.DetectMultiScale(histogram, 1.1, 10);
                        right_ear = ep.detector_right_ear.DetectMultiScale(histogram, 1.1, 10);
                    }
                }
                if (left_ear.Length > 0)
                {
                    left_ear = ep.preventNormalizationGap(a, left_ear);
                    roi = left_ear[0];
                    is_detected = true;
                }
                else if (right_ear.Length > 0)
                {
                    right_ear = ep.preventNormalizationGap(a, right_ear);
                    roi = right_ear[0];
                    is_detected = true;
                }
                if (is_detected)
                {
                    _ear_ROI = roi;
                    ear_imgs[0] = ep.PCAEarROI(a, roi);
                    ear_imgs[1] = ep.drawRectangle(a, roi);
                }
            }
            catch
            {
                MessageBox.Show("OB");
            }
            return ear_imgs;
        }

        // PCA function
        private Mat EarPreprocessing(Mat ear)
        {
            Mat PCA_EAR = ear;

            // Image Acquisition
            pbEarROI.Image = ear.Bitmap;
            ga.invokeSetLabelText(labelAcquisitionX, "X = " + _ear_ROI.X.ToString());
            ga.invokeSetLabelText(labelAcquisitionY, "Y = " + _ear_ROI.Y.ToString());
            ga.invokeSetLabelText(labelAcquisitionWidth, "Width = " + _ear_ROI.Width.ToString());
            ga.invokeSetLabelText(labelAcquisitionHeight, "Height = " + _ear_ROI.Height.ToString());
            _ep_global.insertROI(PCA_EAR);

            // Gray Scale Conversion
            if (_ep_global._pca_gray)
            {
                PCA_EAR = ep.PCAEarGray(ear);
                pbEarGray.Image = PCA_EAR.Bitmap;
                _ep_global.insertGray(PCA_EAR);
            }
            else
            {
                pbEarGray.Image = null;
                _ep_global.insertGray(null);
            }
            // Histogram
            if (_ep_global._pca_histogram)
            {
                PCA_EAR = ep.PCAEarHistogram(PCA_EAR);
                pbEarHistogram.Image = PCA_EAR.Bitmap;
                _ep_global.insertHistogram(PCA_EAR);
            }
            else
            {
                pbEarHistogram.Image = null;
                _ep_global.insertHistogram(null);
            }

            // Blur


            if (_ep_global._pca_blur)
            {
                if (_ep_global._blurring_median)
                {
                    PCA_EAR = ep.PCAEarBlur(PCA_EAR, _ep_global._median_threshold);
                }
                else
                {
                    CvInvoke.GaussianBlur(PCA_EAR, PCA_EAR, new Size(5, 5), 1.5, 1.5);
                }
                pbEarBlur.Image = PCA_EAR.Bitmap;
                _ep_global.insertBlur(PCA_EAR);
            }
            else
            {
                pbEarBlur.Image = null;
                _ep_global.insertBlur(null);
            }

            // Edge
            if (_ep_global._pca_edge)
            {

                PCA_EAR = ep.PCAEarEdge(PCA_EAR, _ep_global._edge_threshold_1, _ep_global._edge_threshold_2);
                pbEarEdge.Image = PCA_EAR.Bitmap;
                _ep_global.insertEdge(PCA_EAR);
            }
            else
            {
                pbEarEdge.Image = null;
                _ep_global.insertEdge(null);
            }
            return PCA_EAR;
        }

        // Recognition function
        private SURF surf_features = new SURF(500);
        private SIFT sift_features = new SIFT(500);
        private ORBDetector orb_features = new ORBDetector(500);

        private BFMatcher BF_matcher;
        private FlannBasedMatcher FLANN_matcher;
        // private FlannBasedMatcher matcher;
        // Match the extracted features
        private void featuresMatch(Mat ear_model, Mat ear_observed)
        {
            long model_detect_time = 0;
            long observed_detect_time = 0;
            long matched_time = 0;
            using (VectorOfKeyPoint k1 = new VectorOfKeyPoint())
            {
                using (VectorOfKeyPoint k2 = new VectorOfKeyPoint())
                {
                    using (Mat d1 = new Mat())
                    {
                        using (Mat d2 = new Mat())
                        {

                            Mat resize_model = new Mat();
                            Mat resize_observed = new Mat();

                            double model_width = ear_model.Width;
                            double model_height = ear_model.Height;

                            double observed_width = ear_observed.Width;
                            double observed_height = ear_observed.Height;

                            double aspectRatio = observed_height / model_height;

                            CvInvoke.Resize(ear_observed, resize_observed, new Size(Convert.ToInt32(observed_width / aspectRatio), Convert.ToInt32(model_height)));
                            //ear_observed = resize_observed;

                            Stopwatch watch;
                            // Extract Keypoints and Descriptors
                            watch = new Stopwatch();
                            watch.Start();



                            if (_ep_global._current_feature_detector == "SURF")
                            {
                                surf_features.DetectAndCompute(ear_model, null, k1, d1, false);
                            }
                            else if (_ep_global._current_feature_detector == "SIFT")
                            {
                                sift_features.DetectAndCompute(ear_model, null, k1, d1, false);
                            }

                            watch.Stop();
                            model_detect_time = watch.ElapsedMilliseconds;

                            watch = new Stopwatch();
                            watch.Start();

                            if (_ep_global._current_feature_detector == "SURF")
                            {
                                surf_features.DetectAndCompute(ear_observed, null, k2, d2, false);
                            }
                            else if (_ep_global._current_feature_detector == "SIFT")
                            {
                                sift_features.DetectAndCompute(ear_observed, null, k2, d2, false);
                            }

                            watch.Stop();
                            observed_detect_time = watch.ElapsedMilliseconds;

                            // draw keypoints
                            using (Mat draw_model = new Mat())
                            {
                                using (Mat draw_observed = new Mat())
                                {
                                    Features2DToolbox.DrawKeypoints(ear_model, k1, draw_model, new Bgr(46, 204, 113));
                                    Features2DToolbox.DrawKeypoints(ear_observed, k2, draw_observed, new Bgr(46, 204, 113));
                                    // converts the keypoints image to gray to prevent pointer errors
                                    using (Mat gray_model = new Mat())
                                    {
                                        using (Mat gray_observed = new Mat())
                                        {
                                            CvInvoke.CvtColor(draw_model, gray_model, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);
                                            CvInvoke.CvtColor(draw_observed, gray_observed, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);
                                            ibEarFeatures.Image = gray_model;
                                            ga.invokeSetLabelText(labelEarFeaturesKeypoints, "Total keypoints = " + k1.Size.ToString());
                                            //start for global form
                                            _ep_global.insertEarFeaturesDetected(gray_model, gray_observed, k1.Size, k2.Size, model_detect_time, observed_detect_time);
                                        }
                                    }
                                }
                            }
                            // K-Nearest Neighbor Matching algorithm || Brute Force (BF) || Fast Library for Appriximate Nearest Neighbor (FLANN)



                            watch = new Stopwatch();
                            watch.Start();

                            using (LinearIndexParams ip = new LinearIndexParams())
                            {
                                using (SearchParams sp = new SearchParams())
                                {
                                    using (FLANN_matcher = new FlannBasedMatcher(ip, sp))
                                    {

                                        using (BF_matcher = new BFMatcher(DistanceType.L2))
                                        {
                                            BF_matcher.Add(d1);
                                            FLANN_matcher.Add(d1);
                                            if (k1.Size > 0 && k2.Size > 0)
                                            {
                                                int k = 2;
                                                double uniqueness_threshold = 0.8;
                                                Mat mask;
                                                Mat lines = new Mat();
                                                using (VectorOfVectorOfDMatch matches = new VectorOfVectorOfDMatch())
                                                {
                                                    if (_ep_global._current_matcher == "BF")
                                                    {
                                                        BF_matcher.KnnMatch(d2, matches, k, null);
                                                    }
                                                    else if (_ep_global._current_matcher == "FLANN")
                                                    {
                                                        FLANN_matcher.KnnMatch(d2, matches, k, null);
                                                    }
                                                    mask = new Mat(matches.Size, 1, DepthType.Cv8U, 1);
                                                    mask.SetTo(new MCvScalar(255));
                                                    Features2DToolbox.VoteForUniqueness(matches, uniqueness_threshold, mask);
                                                    Features2DToolbox.DrawMatches(ear_model, k1, ear_observed, k2, matches, lines, new Bgr(Color.White).MCvScalar, new Bgr(Color.Gray).MCvScalar, mask);
                                                }
                                                int count_non_zero = CvInvoke.CountNonZero(mask);
                                                watch.Stop();
                                                matched_time = watch.ElapsedMilliseconds;
                                                ibEarFeaturesMatch.Image = lines;

                                                string matched_keypoints = count_non_zero + "/" + k1.Size.ToString();
                                                ga.invokeSetLabelText(labelMatchedKeypoints, "Matched Keypoints: " + matched_keypoints);
                                                _ep_global.insertEarFeaturesMatched(lines, count_non_zero, k1.Size, matched_time);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void EarRecognition()
        {
            Mat ear_model = EarPreprocessing(_ear_detected);
            Mat ear_observed = (_ep_global._ear_observed == null) ? ear_model : EarPreprocessing(_ep_global._ear_observed);
            featuresMatch(ear_model, ear_observed);
        }

        public void globalProcessEar()
        {
            if (_ear_detected == null)
            {
                _reactor.showReactor(false, "Capture an ear to process.");
            }
            else
            {
                EarRecognition();
            }
        }
        private void stopCapturing()
        {
            _capture.Stop();
            btnPushStateCamera.Text = "Start";
            ga.changeButtonCaptureStyle(btnPushStateCamera, false);
        }

        private void btnPushStateCamera_Click(object sender, EventArgs e)
        {
            if (btnPushStateCamera.Text == "Start")
            {
                _capture.Start();
                if (_capture.IsOpened)
                {
                    btnPushStateCamera.Text = "Stop";
                    ga.changeButtonCaptureStyle(btnPushStateCamera, true);
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

        private void btnGlobalForm_Click(object sender, EventArgs e)
        {
            // this.Hide();
            _ep_global.Show();
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
                if (checkEarImage(img) == false)
                {
                    _reactor.showReactor(false, "No ear has been found in the image.");
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            globalProcessEar();
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            globalProcessEar();
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            globalProcessEar();
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            globalProcessEar();
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            globalProcessEar();
        }

        private void timerFormCloser_Tick(object sender, EventArgs e)
        {
            timerFormCloser.Enabled = false;
            this.Close();
        }
    }
}
