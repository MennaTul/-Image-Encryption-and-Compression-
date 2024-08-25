using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageEncryptCompress
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        RGBPixel[,] ImageMatrix;

        string openedFilePath;

        public void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                openedFilePath = openFileDialog1.FileName;
                ImageMatrix = ImageOperations.OpenImage(openedFilePath);
                ImageOperations.DisplayImage(ImageMatrix, pictureBox1);


                int width = ImageOperations.GetWidth(ImageMatrix);
                int height = ImageOperations.GetHeight(ImageMatrix);
                txtWidth.Text = width.ToString();
                txtHeight.Text = height.ToString();


            }
        }

        private void btnEncrypt_Click_1(object sender, EventArgs e)
        {
            if (ImageMatrix == null)
            {
                MessageBox.Show("Please open an image first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            long intialSeed = 0;
            string InitialSeedString = InitialSeed.Text;
            bool containsCharacter = InitialSeedString.Any(c => c != '0' && c != '1');
            if (containsCharacter)//  Seed contains chars
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (char c in InitialSeedString)
                {
                    string binaryChar = Convert.ToString(c, 2).PadLeft(8, '0');
                    stringBuilder.Append(binaryChar);
                }
                InitialSeedString = stringBuilder.ToString();
                intialSeed = Convert.ToInt64(InitialSeedString, 2);
            }
            else //  Seed contains only 0 & 1
            {
                intialSeed = Convert.ToInt64(InitialSeedString, 2);
            }
            byte tapPosition = Convert.ToByte(TapPosition.Text);
            byte NumOfBits = (byte)InitialSeedString.Length;
            try
            {
                if (tapPosition >= NumOfBits)
                {
                    throw new InvalidOperationException("Tap position cannot be greater than or equal to the number of bits.");
                }
                EncryptImage(intialSeed, tapPosition, NumOfBits);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "bmp files (*.bmp)|*.bmp|All files (*.*)|*.*";
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox2.Image.Save(saveFileDialog1.FileName, ImageFormat.Bmp);
            }


        }
        private async void EncryptImage(long seed, byte tapPosition, byte bits)
        {
            Stopwatch watch = new Stopwatch();
            LFSREncryption lfsrEncryption = new LFSREncryption(seed, tapPosition, bits);
            watch.Start();
            for (int i = 0; i < ImageMatrix.GetLength(0); i++)  //O(N*N)
            {
                await Task.Run(() =>
                {
                    for (int j = 0; j < ImageMatrix.GetLength(1); j++)
                    {
                        ImageMatrix[i, j].red = lfsrEncryption.EncryptByte(ImageMatrix[i, j].red);
                        ImageMatrix[i, j].green = lfsrEncryption.EncryptByte(ImageMatrix[i, j].green);
                        ImageMatrix[i, j].blue = lfsrEncryption.EncryptByte(ImageMatrix[i, j].blue);
                    }
                }).
                ContinueWith((ant) =>
                {
                    string te = watch.Elapsed.ToString(@"m\:ss");
                    lblTimeTaken.Text = "Time Taken : " + te;
                }
                , TaskScheduler.FromCurrentSynchronizationContext());
            }
            watch.Stop();
            string tsOut = watch.Elapsed.ToString(@"m\:ss");
            lblTimeTaken.Text = "Time Taken : " + tsOut;
            ImageOperations.DisplayImage(ImageMatrix, pictureBox2);
        }






        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void Compress_Click(object sender, EventArgs e)
        {

            
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            Compression C = new Compression();
            C.comp(ImageMatrix, openedFilePath, InitialSeed.Text, TapPosition.Text);

            
            stopwatch.Stop();

            MessageBox.Show($"Compression completed in {stopwatch.ElapsedMilliseconds / 1000} seconds.", "Compression Time");


        }



        private void decompress_Click(object sender, EventArgs e)
        {
            RGBPixel[,] Image;
            Compression DC = new Compression();

            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            Image = DC.Decomp();

            stopwatch.Stop();

            InitialSeed.Text = Compression.initialseed;

            TapPosition.Text = Compression.taposition;

            MessageBox.Show("The image is decompressed!");

            MessageBox.Show($"Compression completed in {stopwatch.ElapsedMilliseconds / 1000} seconds.", "Compression Time");


            ImageOperations.DisplayImage(Image, pictureBox2);

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "bmp files (*.bmp)|*.bmp|All files (*.*)|*.*";
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox2.Image.Save(saveFileDialog1.FileName, ImageFormat.Bmp);
            }
            ImageMatrix = Image;
        }






    }



}



