using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Exception = System.Exception;

namespace PDP_Project
{
    public partial class Form1 : Form
    {
        Bitmap Im1, Im2;
        private Image loadedImage;
        public Form1()
        {
            InitializeComponent();
            Im1 = new Bitmap(pictureBox1.Image);
            Im2 = new Bitmap(pictureBox2.Image);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog1.ShowDialog();
                loadedImage = Image.FromFile(openFileDialog1.FileName);
                Im1 = new Bitmap(loadedImage);
                pictureBox1.Image = Im1;
                pictureBox1.Refresh();
            }
            catch (Exception exception)
            {
                Debug.WriteLine("Something went wrong when opening image. Exception: "+exception.Message);
            }

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                saveFileDialog1.Filter = "Data Files (*.png)|*.png";
                saveFileDialog1.DefaultExt = "png";
                saveFileDialog1.AddExtension = true;
                saveFileDialog1.ShowDialog();
                Im2.Save(saveFileDialog1.FileName);
            }
            catch (Exception exception)
            {
                Debug.WriteLine("Something went wrong when saving image. Exception: "+exception.Message);
            }
        }

        private void grayscaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Im2 = new Bitmap(Im1);
            for (int i = 0; i < Im1.Height; i++)
            for (int j = 0; j < Im1.Width; j++)
            {
                Color c = Im1.GetPixel(j, i);
                int grayScale = (int)((c.R * 0.3) + (c.G * 0.59) + (c.B * 0.11));
                Im2.SetPixel(j, i, Color.FromArgb(c.A, grayScale,grayScale,grayScale));
            }
            pictureBox2.Image = Im2; pictureBox2.Refresh();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void swapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = Im2; pictureBox1.Refresh();
            pictureBox2.Image = Im1; pictureBox2.Refresh();

            Im1 = new Bitmap(pictureBox1.Image);
            Im2 = new Bitmap(pictureBox2.Image);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
    }
}
