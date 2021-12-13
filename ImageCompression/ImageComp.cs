using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageCompression
{
    public partial class ImageComp : Form
    {
        PictureBox firstImageBox;
        PictureBox secondImageBox;
        Label PsnrValue;
        Label psnrException;

        public ImageComp()
        {
            InitializeComponent();
        }

        private void ImageComp_Load(object sender, EventArgs e)
        {
            var loadFirstImageButton = new Button();
            loadFirstImageButton.Size = new Size(80,20);
            loadFirstImageButton.Location = new Point(229, 20);
            loadFirstImageButton.Text = "Загрузить";
            loadFirstImageButton.Click += LoadFirstImageButton_Click;
            this.Controls.Add(loadFirstImageButton);

            var loadSecondImageButton = new Button();
            loadSecondImageButton.Size = new Size(80,20);
            loadSecondImageButton.Location = new Point(763, 20);
            loadSecondImageButton.Text = "Загрузить";
            loadSecondImageButton.Click += LoadSecondImageButton_Click;
            this.Controls.Add(loadSecondImageButton);

            var saveFirstImageButton = new Button();
            saveFirstImageButton.Size = new Size(80, 20);
            saveFirstImageButton.Location = new Point(229, 41);
            saveFirstImageButton.Text = "Сохранить";
            saveFirstImageButton.Click += SaveFirstImageButton_Click;
            this.Controls.Add(saveFirstImageButton);

            var saveSecondImageButton = new Button();
            saveSecondImageButton.Size = new Size(80, 20);
            saveSecondImageButton.Location = new Point(763, 41);
            saveSecondImageButton.Text = "Сохранить";
            saveSecondImageButton.Click += SaveSecondImageButton_Click;
            this.Controls.Add(saveSecondImageButton);

            firstImageBox = new PictureBox();
            firstImageBox.Size = new Size(512, 512);
            firstImageBox.Location = new Point(25, 65);
            firstImageBox.BackColor = Color.LightGray;
            this.Controls.Add(firstImageBox);

            secondImageBox = new PictureBox();
            secondImageBox.Size = new Size(512, 512);
            secondImageBox.Location = new Point(547, 65);
            secondImageBox.BackColor = Color.LightGray;
            this.Controls.Add(secondImageBox);

            var countPSNRButton = new Button();
            countPSNRButton.Size = new Size(80, 20);
            countPSNRButton.Location = new Point(1080, 100);
            countPSNRButton.Text = "PSNR";
            countPSNRButton.Click += CountPSNR_Click;
            this.Controls.Add(countPSNRButton);

            var grayEqualWeightsButton = new Button();
            grayEqualWeightsButton.Size = new Size(80, 20);
            grayEqualWeightsButton.Location = new Point(1080, 120);
            grayEqualWeightsButton.Text = "В ЧБ(EW)";
            grayEqualWeightsButton.Click += GrayEqualWeight_Click;
            this.Controls.Add(grayEqualWeightsButton);

            var grayCCIRButton = new Button();
            grayCCIRButton.Size = new Size(80, 20);
            grayCCIRButton.Location = new Point(1080, 140);
            grayCCIRButton.Text = "В ЧБ(CCIR)";
            grayCCIRButton.Click += GrayCCIR_Click;
            this.Controls.Add(grayCCIRButton);

            var toYCbCrButton = new Button();
            toYCbCrButton.Size = new Size(80, 20);
            toYCbCrButton.Location = new Point(1080, 160);
            toYCbCrButton.Text = "В YCbCr";
            toYCbCrButton.Click += YCbCr_Click;
            this.Controls.Add(toYCbCrButton);

            var toRGBButton = new Button();
            toRGBButton.Size = new Size(80, 20);
            toRGBButton.Location = new Point(1080, 180);
            toRGBButton.Text = "В RGB";
            toRGBButton.Click += RGB_Click;
            this.Controls.Add(toRGBButton);

            psnrException = new Label();
            psnrException.Size = new Size(200, 40);
            psnrException.Location = new Point(1080, 205);
            psnrException.Text = "Ошибка.";
            psnrException.ForeColor = Color.Red;
            psnrException.Visible = false;
            this.Controls.Add(psnrException);

            var PsnrLabel = new Label();
            PsnrLabel.Size = new Size(40, 20);
            PsnrLabel.Location = new Point(1080, 250);
            PsnrLabel.Text = "PSNR:";
            this.Controls.Add(PsnrLabel);

            PsnrValue = new Label();
            PsnrValue.Size = new Size(50, 20);
            PsnrValue.Location = new Point(1121, 250);
            this.Controls.Add(PsnrValue);

            var timer = new Timer();
            timer.Interval = 3000;
            timer.Tick += (object s, EventArgs ev) => { psnrException.Visible = false; };
            timer.Start();
        }

        private void GrayEqualWeight_Click(object sender, EventArgs e)
        {
            var bitmap = Calculator.ToGrayByEqualWeights(firstImageBox.Image);

            secondImageBox.Image = bitmap;
        }

        private void GrayCCIR_Click(object sender, EventArgs e)
        {
            var bitmap = Calculator.ToGrayByCCIR(firstImageBox.Image);

            secondImageBox.Image = bitmap;
        }

        private void YCbCr_Click(object sender, EventArgs e)
        {
            secondImageBox.Image = Calculator.ToYCbCr(firstImageBox.Image);
        }

        private void RGB_Click(object sender, EventArgs e)
        {
            secondImageBox.Image = Calculator.ToRGB(secondImageBox.Image);
        }

        private void CountPSNR_Click(object sender, EventArgs e)
        {
            try
            {
                PsnrValue.Text = Calculator.CountPSNR(firstImageBox.Image, secondImageBox.Image);
            }
            catch(Exception exc)
            {
                psnrException.Visible = true;
                return;
            }
        }

        private void LoadFirstImageButton_Click(object sender, EventArgs e)
        {
            LoadImage(firstImageBox);
        }

        private void LoadSecondImageButton_Click(object sender, EventArgs e)
        {
            LoadImage(secondImageBox);
        }

        private void SaveFirstImageButton_Click(object sender, EventArgs e)
        {
            SaveImage(firstImageBox);
        }

        private void SaveSecondImageButton_Click(object sender, EventArgs e)
        {
            SaveImage(secondImageBox);
        }

        public void LoadImage(PictureBox pictureBox)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Файлы изображений|*.bmp;*.png;*.jpg";
            if (openDialog.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                var bitmap = LoadBitmap(openDialog.FileName);
                pictureBox.Image = bitmap;
            }
            catch (OutOfMemoryException ex)
            {
                MessageBox.Show("Ошибка чтения картинки");
                return;
            }
        }

        public void SaveImage(PictureBox pictureBox)
        {
            if (pictureBox.Image != null)
            {
                SaveFileDialog savedialog = new SaveFileDialog();
                savedialog.Title = "Сохранить картинку как...";
                //отображать ли предупреждение, если пользователь указывает имя уже существующего файла
                savedialog.OverwritePrompt = true;
                //отображать ли предупреждение, если пользователь указывает несуществующий путь
                savedialog.CheckPathExists = true;
                //список форматов файла, отображаемый в поле "Тип файла"
                savedialog.Filter = "Image Files(*.BMP)|*.BMP|Image Files(*.JPG)|*.JPG|Image Files(*.GIF)|*.GIF|Image Files(*.PNG)|*.PNG|All files (*.*)|*.*";
                //отображается ли кнопка "Справка" в диалоговом окне
                savedialog.ShowHelp = true;
                if (savedialog.ShowDialog() == DialogResult.OK) //если в диалоговом окне нажата кнопка "ОК"
                {
                    try
                    {
                        pictureBox.Image.Save(savedialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                    catch
                    {
                        MessageBox.Show("Невозможно сохранить изображение", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        public static Bitmap LoadBitmap(string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                return new Bitmap(fs);
        }
    }
}
