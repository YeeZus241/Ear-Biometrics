using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms; // temporary will remove this later
using System.IO;
using MySql.Data;
using MySql.Data.MySqlClient;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;
using Emgu.CV.Features2D;
using Emgu.CV.XFeatures2D;
using Emgu.CV.Flann;
using Emgu.CV.Shape;
// all function to process the ear

// Step by step of preprocessing ear image
// 1. Ear Image Acquisition
// 2. Gray Scale Conversion
// 3. Histogram Equalization
// 4. Median Filter || ksize = 25
// 5. Edge Detection || th1 = 75, th2 = 175

// Steps for Ear Features
// 1. Feature Detection using Speeded Up Robust Features (SURF) || Hessian = 500
// 2. Feature Matching using Brute Force (BFMatcher) and K-Nearest Neighbor (KnnMatch) || Distance = Euclidean L2

namespace EarBiometric
{
    class ear_process
    {
        global_arguments ga = new global_arguments();
        public CascadeClassifier detector_left_ear;
        public CascadeClassifier detector_right_ear;

        public int _median_threshold = 25;
        public double _edge_threshold_1 = 75;
        public double _edge_threshold_2 = 150;

        public ear_process()
        {
            detector_left_ear = new CascadeClassifier(ga.getComponentsPath() + "\\haar_cascade\\haarcascade_mcs_leftear.xml");
            detector_right_ear = new CascadeClassifier(ga.getComponentsPath() + "\\haar_cascade\\haarcascade_mcs_rightear.xml");
        }

        //Prevents gap for nomalization, Modified by: Jeffrey Ongcay
        public Rectangle[] preventNormalizationGap(Mat uncropped, Rectangle[] a)
        {
            for (int i = 0; i < a.Length; i++)
            {
                int x = a[i].X - 12;
                int y = a[i].Y - 12;
                int width = a[i].Width + 15;
                int height = a[i].Height + 15;
                if (x >= 0 && y >= 0 && width <= uncropped.Width && height <= uncropped.Height)
                {
                    a[i].X = x;
                    a[i].Y = y;
                    a[i].Width = width;
                    a[i].Height = height;
                }
            }
            return a;
        }

        // draws rectangle in detected image
        public Mat drawRectangle(Mat a, Rectangle b)
        {
            Mat gray = new Mat();
            // I alocate img to gray temporarily to prevent replacing the original image || will find solution soon :)
            CvInvoke.CvtColor(a, gray, ColorConversion.Bgr2Gray);
            CvInvoke.Rectangle(gray, b, new Rgb(Color.White).MCvScalar, 2);
            return gray;
        }

        public bool saveEarImageToFolder(string path, string filename, Mat ear)
        {
            bool r = false;
            bool confirm = false;
            if (File.Exists(path + filename + ga._global_ear_extension))
            {
                DialogResult dr = MessageBox.Show("The file: " + filename + " already exists in path: " + path + ". do you wish to replace it?", "File already exists", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    confirm = true;
                }
                r = true;
            }
            else
            {
                confirm = true;
            }
            if (confirm)
            {
                if (CvInvoke.Imwrite(path + filename + ga._global_ear_extension, ear))
                {
                    r = true;
                }
            }
            return r;
        }

        //detect ear images Mat[0] = normalized ear | Mat[1] = Draws rectangle in an image Mat[2] = ROI values
        public Mat[] detectEarImage(Mat a)
        {
            Mat[] ear_imgs = new Mat[3];
            ear_imgs[0] = new Mat(); // for cropped ear
            ear_imgs[1] = new Mat(); // for uncropped ear draws rectangle in ROI
            ear_imgs[2] = new Mat(); // for uncropped ear
            Rectangle roi = new Rectangle();
            Rectangle[] left_ear;
            Rectangle[] right_ear;
            bool is_detected = false;
            try
            {
                using (Mat gray = PCAEarGray(a))
                {
                    using (Mat histogram = PCAEarHistogram(gray))
                    {
                        left_ear = detector_left_ear.DetectMultiScale(histogram, 1.1, 10);
                        right_ear = detector_right_ear.DetectMultiScale(histogram, 1.1, 10);
                    }
                }
                if (left_ear.Length > 0)
                {
                    left_ear = preventNormalizationGap(a, left_ear);
                    roi = left_ear[0];
                    is_detected = true;
                }
                else if (right_ear.Length > 0)
                {
                    right_ear = preventNormalizationGap(a, right_ear);
                    roi = right_ear[0];
                    is_detected = true;
                }
                if (is_detected)
                {
                    ear_imgs[0] = PCAEarROI(a, roi);
                    ear_imgs[1] = drawRectangle(a, roi);
                    ear_imgs[2] = a;
                }
            }
            catch
            {
            }
            return ear_imgs;
        }

        // step 1
        public Mat PCAEarROI(Mat a, Rectangle b)
        {
            Mat roi = new Mat(a, b);
            // 130x150 size if masked
            return roi;
        }

        // step 2
        public Mat PCAEarGray(Mat ear)
        {
            Mat gray = new Mat();
            CvInvoke.CvtColor(ear, gray, ColorConversion.Bgr2Gray);
            return gray;
        }

        // step 3
        public Mat PCAEarHistogram(Mat ear)
        {
            Mat histogram = new Mat();
            CvInvoke.EqualizeHist(ear, histogram);
            return histogram;
        }

        // step 4
        public Mat PCAEarBlur(Mat ear, int ksize)
        {
            Mat blur = new Mat();
            CvInvoke.MedianBlur(ear, blur, ksize);
            return blur;
        }

        // step 5
        public Mat PCAEarEdge(Mat ear, double th1, double th2)
        {
            Mat edge = new Mat();
            CvInvoke.Canny(ear, edge, th1, th2);
            return edge;
        }

        // Compress all Preprocessing Method
        public Mat processEarPCA(Mat ear)
        {
            Mat r;
            using (Mat gray = PCAEarGray(ear))
            {
                using (Mat histogram = PCAEarHistogram(gray))
                {
                    // median value = 17
                    using (Mat blur = PCAEarBlur(histogram, _median_threshold))
                    {
                        Mat canny = PCAEarEdge(blur, _edge_threshold_1, _edge_threshold_2);
                        r = canny;
                    }
                }
            }
            return r;
        }


        // Start comparing image
        // 1. Feature Extraction #SURF
        // 2. Feature Matching #KNN Match # Brute Force
        private SURF features = new SURF(500);
        private BFMatcher matcher;
        public int[] CompareEar(Mat ear_model, Mat ear_observed)
        {
            int[] r = new int[2];
            // r[0] = over score
            // r[1] = matched score
            //CvInvoke.Imshow("A", ear_model);
            //CvInvoke.Imshow("B", ear_observed);
            //CvInvoke.WaitKey(0);


            using (VectorOfKeyPoint k1 = new VectorOfKeyPoint())
            {
                using (VectorOfKeyPoint k2 = new VectorOfKeyPoint())
                {
                    using (Mat d1 = new Mat())
                    {
                        using (Mat d2 = new Mat())
                        {
                            // Extract Keypoints and Descriptors
                            features.DetectAndCompute(ear_model, null, k1, d1, false);
                            features.DetectAndCompute(ear_observed, null, k2, d2, false);

                            // K-Nearest Neighbor Matching algorithm || Brute Force (BF) 
                            using (matcher = new BFMatcher(DistanceType.L2))
                            {
                                matcher.Add(d1);
                                if (k1.Size > 0 && k2.Size > 0)
                                {
                                    int k = 2;
                                    double uniqueness_threshold = 0.8;
                                    Mat mask;
                                    using (VectorOfVectorOfDMatch matches = new VectorOfVectorOfDMatch())
                                    {
                                        matcher.KnnMatch(d2, matches, k, null);
                                        mask = new Mat(matches.Size, 1, DepthType.Cv8U, 1);
                                        mask.SetTo(new MCvScalar(255));
                                        Features2DToolbox.VoteForUniqueness(matches, uniqueness_threshold, mask);
                                    }
                                    int count_non_zero = CvInvoke.CountNonZero(mask);
                                    r[0] = k1.Size;
                                    r[1] = count_non_zero;
                                }
                            }
                        }
                    }
                }
            }
            return r;
        }

        double _similarity_points = 75; //

        // recognize ear using the highest points in the image database
        public string recognizeEarHighestSimilarity(string[] db_ears, Mat cropped_observed)
        {
            string r = "not_found";
            double highest = 0;
            // model ear, done preprocessing. || remove preprocessing, if captured image already trained in the database
            // observed ear, done preprocessing 
            using (Mat ear_observed = processEarPCA(cropped_observed))
            {
                foreach (var ear in db_ears)
                {
                    using (Mat ear_model = processEarPCA(CvInvoke.Imread(ear)))
                    {
                        int[] score = CompareEar(ear_model, ear_observed);
                        double similarity = score[1]; //ga.calculateDecreasePercentage(score[0], score[1]);
                        if (similarity >= highest)
                        {
                            string angle_id = Path.GetFileNameWithoutExtension(ear);
                            highest = similarity;
                            r = angle_id;
                        }
                    }
                }
            }
            if (highest < _similarity_points)
            {
                r = "not_found";
            }
            // MessageBox.Show(highest.ToString());
            return r;
        }
    }
}
