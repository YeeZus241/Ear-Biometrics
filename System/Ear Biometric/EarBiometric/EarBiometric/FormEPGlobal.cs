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
using Emgu.CV.Util;
namespace EarBiometric
{
    public partial class FormEPGlobal : Form
    {

        // initialize data for draggable form
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        global_arguments ga = new global_arguments();
        ear_process ep = new ear_process();
        FormReactor _reactor = new FormReactor();
        public FormEarProcess _fep;
        public bool _blurring_median = true;
        public bool _pca_gray = true;
        public bool _pca_histogram = true;
        public bool _pca_blur = true;
        public bool _pca_edge = true;
        public int _median_threshold = 0;
        public double _edge_threshold_1 = 0;
        public double _edge_threshold_2 = 0;

        public Mat _ear_observed = null;
        public string _current_feature_detector = "SURF";
        public string _current_matcher = "BF";
        public FormEPGlobal()
        {
            InitializeComponent();
            _median_threshold = ep._median_threshold;
            _edge_threshold_1 = ep._edge_threshold_1;
            _edge_threshold_2 = ep._edge_threshold_2;
        }

        private void FormEPGlobal_Load(object sender, EventArgs e)
        {
            tbMedianKSize.Text = _median_threshold.ToString();
            tbEdgeThreshold1.Text = _edge_threshold_1.ToString();
            tbEdgeThreshold2.Text = _edge_threshold_2.ToString();
        }

        public void insertEarFeaturesDetected(Mat ear_model, Mat ear_observed, int model_size, int observed_size, long model_time, long observed_time)
        {

            pbEarModel.Image = ear_model.Bitmap;
            pbEarObserved.Image = ear_observed.Bitmap;
            ga.invokeSetTBText(tbEarModelKeypoints, model_size.ToString());
            ga.invokeSetTBText(tbEarObservedKeypoints, observed_size.ToString());
            ga.invokeSetTBText(tbEarModelDetectionTime, model_time.ToString() + " MS");
            ga.invokeSetTBText(tbEarObservedDetectionTime, observed_time.ToString() + " MS");
        }

        public void insertEarFeaturesMatched(Mat matched, int keypoints_matched, int keypoints_base, long matched_time)
        {
            pbEarMatched.Image = matched.Bitmap;
            ga.invokeSetTBText(tbMatchedKeypoints, keypoints_matched + "/" + keypoints_base);
            ga.invokeSetTBText(tbMatchedKepointsPercentage, String.Format("{0:0}", ga.calculateDecreasePercentage(keypoints_base, keypoints_matched)) + "%");
            ga.invokeSetTBText(tbEarMatchedTime, matched_time.ToString() + " MS");
        }
        public void syncAlgorithm(Mat ear)
        {
            pbNewAlgorithm.Image = ear.Bitmap;
        }

        public void insertROI(Mat ear)
        {
            if (ear == null)
            {
                pbEarROI.Image = null;
            }
            else
            {
                pbEarROI.Image = ear.Bitmap;
            }
        }

        public void insertGray(Mat ear)
        {
            if (ear == null)
            {
                pbEarGray.Image = null;
            }
            else
            {
                pbEarGray.Image = ear.Bitmap;
            }
        }
        public void insertHistogram(Mat ear)
        {
            if (ear == null)
            {
                pbEarHistogram.Image = null;
            }
            else
            {
                pbEarHistogram.Image = ear.Bitmap;
            }
        }
        public void insertBlur(Mat ear)
        {
            if (ear == null)
            {
                pbEarBlur.Image = null;
            }
            else
            {
                pbEarBlur.Image = ear.Bitmap;
            }
        }
        public void insertEdge(Mat ear)
        {
            if (ear == null)
            {
                pbEarEdge.Image = null;
            }
            else
            {
                pbEarEdge.Image = ear.Bitmap;
            }
        }

        private void cbGrayScale_CheckedChanged(object sender, EventArgs e)
        {
            if (cbGrayScale.Checked)
            {
                _pca_gray = true;
            }
            else
            {
                _pca_gray = false;
                _pca_histogram = false;
                cbHistogram.Checked = false;
            }
        }
        private void cbHistogram_CheckedChanged(object sender, EventArgs e)
        {
            if (cbHistogram.Checked)
            {
                _pca_histogram = true;
                _pca_gray = true;
                cbGrayScale.Checked = true;
            }
            else
            {
                _pca_histogram = false;
            }
        }

        private void cbBlur_CheckedChanged(object sender, EventArgs e)
        {
            if (cbBlur.Checked)
            {
                _pca_blur = true;
            }
            else
            {
                _pca_blur = false;
            }
        }

        private void cbEdge_CheckedChanged(object sender, EventArgs e)
        {
            if (cbEdge.Checked)
            {
                _pca_edge = true;
            }
            else
            {
                _pca_edge = false;
            }
        }

        private void saveData()
        {
            try
            {
                int tb_median = Convert.ToInt32(tbMedianKSize.Text);
                if (tb_median % 2 == 0)
                {
                    _reactor.showReactor(false, "Median value should be odd numbers.");
                }
                else
                {
                    _median_threshold = tb_median;
                }
            }
            catch
            {
                _reactor.showReactor(false, "Invalid Median Value.");
            }

            double th1 = 50;
            double th2 = 125;
            try
            {
                th1 = Convert.ToDouble(tbEdgeThreshold1.Text);
                th2 = Convert.ToDouble(tbEdgeThreshold2.Text);
                _edge_threshold_1 = th1;
                _edge_threshold_2 = th2;
            }
            catch
            {
                _reactor.showReactor(false, "Invalid Edge Threshold value.");
            }
        }
        private void btnProcess_Click(object sender, EventArgs e)
        {
            saveData();
            _fep.globalProcessEar();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Hide();
            _fep.Show();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void rbBlurringMedian_CheckedChanged(object sender, EventArgs e)
        {
            if (rbBlurringMedian.Checked)
            {
                _blurring_median = true;
            }
        }

        private void rbBlurringGaussian_CheckedChanged(object sender, EventArgs e)
        {
            if (rbBlurringGaussian.Checked)
            {
                _blurring_median = false;
            }
        }

        private bool checkEarImage(Mat frame)
        {
            bool r = false;
            Mat[] detected_ear = ep.detectEarImage(frame);
            if (!detected_ear[0].IsEmpty)
            {
                _ear_observed = detected_ear[0];
                saveData();
                _fep.globalProcessEar();
                r = true;
            }
            return r;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult opf = ofdUploadObservedEar.ShowDialog();
            if (opf == DialogResult.OK)
            {
                Mat img;
                try
                {
                    img = CvInvoke.Imread(ofdUploadObservedEar.FileName);
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

        private void button2_Click(object sender, EventArgs e)
        {
            _ear_observed = null;
            saveData();
            _fep.globalProcessEar();
        }

        private void rbFeaturesSURF_CheckedChanged(object sender, EventArgs e)
        {
            _current_feature_detector = "SURF";
        }

        private void rbFeaturesSIFT_CheckedChanged(object sender, EventArgs e)
        {
            _current_feature_detector = "SIFT";
        }

        private void btnProcessFeatures_Click(object sender, EventArgs e)
        {
            saveData();
            _fep.globalProcessEar();
        }

        private void rbMatcherBF_CheckedChanged(object sender, EventArgs e)
        {
            _current_matcher = "BF";
        }

        private void rbMatcherFLANN_CheckedChanged(object sender, EventArgs e)
        {
            _current_matcher = "FLANN";
        }
    }
}
