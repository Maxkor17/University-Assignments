using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Сomponent_compression
{
    public partial class Form1 : Form
    {
        PictureBox inputImage;
        string path = "G:\\IT\\Repository\\Сomponent compression\\Сomponent compression\\assets\\";
        int[,] X0R = new int[320, 200];
        int[,] X0G = new int[320, 200];
        int[,] X0B = new int[320, 200];
        enum ColorKey { Red, Green, Blue }
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Button myButton = new Button();
            myButton.Text = "Compress";
            myButton.Location = new System.Drawing.Point(305, 250);
            this.Controls.Add(myButton);
            myButton.Click += MyButtonClick;

            inputImage = new PictureBox();
            inputImage.Image = Image.FromFile(path + "photo.jpg");
            inputImage.SizeMode = PictureBoxSizeMode.StretchImage;
            inputImage.Location = new System.Drawing.Point(10, 10);
            inputImage.Size = new System.Drawing.Size(320, 200);
            this.Controls.Add(inputImage);
        }
        private void MyButtonClick(object sender, EventArgs e)
        {
            Bitmap inputBitmap = new Bitmap(inputImage.Image);

            // Перетворення картинки в матрицю
            for (int i = 0; i < 320; i++)
            {
                for (int j = 0; j < 200; j++)
                {
                    X0R[i, j] = inputBitmap.GetPixel(i, j).R;
                    X0G[i, j] = inputBitmap.GetPixel(i, j).G;
                    X0B[i, j] = inputBitmap.GetPixel(i, j).B;
                }
            }
            WriteArrayInFile(X0R, "X0R");
            WriteArrayInFile(X0G, "X0G");
            WriteArrayInFile(X0B, "X0B");

            // Кодування

            // Формуємо X8 (8x8)
            int[,] X8R = FormComponent(ColorKey.Red, 8);
            int[,] X8G = FormComponent(ColorKey.Green, 8);
            int[,] X8B = FormComponent(ColorKey.Blue, 8);
            WriteArrayInFile(X8R, "X8R");
            WriteArrayInFile(X8G, "X8G");
            WriteArrayInFile(X8B, "X8B");

            // Відновлюємо X8 (8x8)
            X8R = RecoverComponent(X8R, 8);
            X8G = RecoverComponent(X8G, 8);
            X8B = RecoverComponent(X8B, 8);
            WriteArrayInFile(X8R, "RecoveryX8R");
            WriteArrayInFile(X8G, "RecoveryX8G");
            WriteArrayInFile(X8B, "RecoveryX8B");

            // Формуємо X4 (4x4)
            int[,] X4R = FormComponent(ColorKey.Red, 4);
            int[,] X4G = FormComponent(ColorKey.Green, 4);
            int[,] X4B = FormComponent(ColorKey.Blue, 4);
            WriteArrayInFile(X4R, "X4R");
            WriteArrayInFile(X4G, "X4G");
            WriteArrayInFile(X4B, "X4B");

            // Знаходимо дельту X8 - X4
            int[,] deltaX8R = ArraySubtraction(X8R, X4R);
            int[,] deltaX8G = ArraySubtraction(X8G, X4G);
            int[,] deltaX8B = ArraySubtraction(X8B, X4B);
            WriteArrayInFile(deltaX8R, "DeltaX8R");
            WriteArrayInFile(deltaX8G, "DeltaX8G");
            WriteArrayInFile(deltaX8B, "DeltaX8B");

            // Відновлюємо X4 (4x4)
            X4R = RecoverComponent(X4R, 4);
            X4G = RecoverComponent(X4G, 4);
            X4B = RecoverComponent(X4B, 4);
            WriteArrayInFile(X4R, "RecoveryX4R");
            WriteArrayInFile(X4G, "RecoveryX4G");
            WriteArrayInFile(X4B, "RecoveryX4B");

            // Формуємо X2 (2x2)
            int[,] X2R = FormComponent(ColorKey.Red, 2);
            int[,] X2G = FormComponent(ColorKey.Green, 2);
            int[,] X2B = FormComponent(ColorKey.Blue, 2);
            WriteArrayInFile(X2R, "X2R");
            WriteArrayInFile(X2G, "X2G");
            WriteArrayInFile(X2B, "X2B");

            // Знаходимо дельту X4 - X2
            int[,] deltaX4R = ArraySubtraction(X4R, X2R);
            int[,] deltaX4G = ArraySubtraction(X4G, X2G);
            int[,] deltaX4B = ArraySubtraction(X4B, X2B);
            WriteArrayInFile(deltaX4R, "DeltaX4R");
            WriteArrayInFile(deltaX4G, "DeltaX4G");
            WriteArrayInFile(deltaX4B, "DeltaX4B");

            // Відновлюємо X2 (2x2)
            X2R = RecoverComponent(X2R, 2);
            X2G = RecoverComponent(X2G, 2);
            X2B = RecoverComponent(X2B, 2);
            WriteArrayInFile(X2R, "RecoveryX2R");
            WriteArrayInFile(X2G, "RecoveryX2G");
            WriteArrayInFile(X2B, "RecoveryX2B");

            // Знаходимо дельту X2 - X0 (X0 - початкова матриця)
            int[,] deltaX2R = ArraySubtraction(X2R, X0R);
            int[,] deltaX2G = ArraySubtraction(X2G, X0G);
            int[,] deltaX2B = ArraySubtraction(X2B, X0B);
            WriteArrayInFile(deltaX2R, "DeltaX2R");
            WriteArrayInFile(deltaX2G, "DeltaX2G");
            WriteArrayInFile(deltaX2B, "DeltaX2B");

            // Декодування
            X8R = FormComponent(ColorKey.Red, 8);
            X8G = FormComponent(ColorKey.Green, 8);
            X8B = FormComponent(ColorKey.Blue, 8);

            X8R = RecoverComponent(X8R, 8);
            X8G = RecoverComponent(X8G, 8);
            X8B = RecoverComponent(X8B, 8);

            X4R = ArraySubtraction(X8R, deltaX8R);
            X4G = ArraySubtraction(X8G, deltaX8G);
            X4B = ArraySubtraction(X8B, deltaX8B);
            WriteArrayInFile(X4R, "X4R + deltaX8R");
            WriteArrayInFile(X4G, "X4G + deltaX8G");
            WriteArrayInFile(X4B, "X4B + deltaX8B");

            X4R = RecoverComponent(X4R, 4);
            X4G = RecoverComponent(X4G, 4);
            X4B = RecoverComponent(X4B, 4);
            X2R = ArraySubtraction(X4R, deltaX4R);
            X2G = ArraySubtraction(X4G, deltaX4G);
            X2B = ArraySubtraction(X4B, deltaX4B);
            WriteArrayInFile(X2R, "X2R + deltaX4R");
            WriteArrayInFile(X2G, "X2G + deltaX4G");
            WriteArrayInFile(X2B, "X2B + deltaX4B");

            X2R = RecoverComponent(X2R, 2);
            X2G = RecoverComponent(X2G, 2);
            X2B = RecoverComponent(X2B, 2);
            X0R = ArraySubtraction(X2R, deltaX2R);
            X0G = ArraySubtraction(X2G, deltaX2G);
            X0B = ArraySubtraction(X2B, deltaX2B);
            WriteArrayInFile(X0R, "X0R + deltaX2R");
            WriteArrayInFile(X0G, "X0G + deltaX2G");
            WriteArrayInFile(X0B, "X0B + deltaX2B");

            Bitmap outputBitmap = new Bitmap(320, 200);
            for (int i = 0; i < 320; i++)
            {
                for (int j = 0; j < 200; j++)
                {
                    Color color = Color.FromArgb(X0R[i, j], X0G[i, j], X0B[i, j]);
                    outputBitmap.SetPixel(i, j, color);
                }
            }
            outputBitmap.Save(path + "photo_compressed.bmp");

            // Виведення відновленої картинки
            PictureBox outputImage = new PictureBox();
            outputImage.Image = Image.FromFile(path + "photo_compressed.bmp");
            outputImage.SizeMode = PictureBoxSizeMode.StretchImage;
            outputImage.Location = new System.Drawing.Point(350, 10);
            outputImage.Size = new System.Drawing.Size(320, 200);
            this.Controls.Add(outputImage);
        }

        int[,] FormComponent(ColorKey colorKey, int key)
        {
            int[,] X = new int[320, 200];
            for (int i = 0; i < 320; i += key)
            {
                for (int j = 0; j < 200; j += key)
                {
                    int sum = 0;
                    for (int x = 0; x < key; x++)
                    {
                        for (int  y = 0; y < key; y++)
                        {
                            if (colorKey == ColorKey.Red)
                            {
                                sum += X0R[i + x, j + y];
                            }
                            else if (colorKey == ColorKey.Green)
                            {
                                sum += X0G[i + x, j + y];
                            }
                            else
                            {
                                sum += X0B[i + x, j + y];
                            }
                        }
                    }
                    X[i, j] = sum / (key * key);
                }
            }
            return X;
        }
        int[,] RecoverComponent(int[,] X, int key)
        {
            for (int i = 0; i < 320; i += key)
            {
                for (int j = 0; j < 200; j += key)
                {
                    // Середнє число у першому рядку матриці
                    if (j + key < 200)
                    {
                        X[i, j + key / 2] = (X[i, j] + X[i, j + key])/2;
                    }
                    else
                    {
                        X[i, j + key / 2] = X[i, j] / 2;
                    }

                    // Середнє число у першому стовпці матриці
                    if (i + key < 320)
                    {
                        X[i + key / 2, j] = (X[i, j] + X[i + key, j])/2;
                    }
                    else
                    {
                        X[i + key / 2, j] = X[i, j] / 2;
                    }
                }
            }

            for (int i = 0; i < 320; i += key)
            {
                for (int j = 0; j < 200; j += key)
                {
                    // Число в середині матриці
                    X[i + key / 2, j + key / 2] += X[i, j];
                    if (i + key < 320) X[i + key / 2, j + key / 2] += X[i + key, j];
                    if (j + key < 200) X[i + key / 2, j + key / 2] += X[i, j + key];
                    if (i + key < 320 && j + key < 200) X[i + key / 2, j + key / 2] += X[i + key, j + key];
                    X[i + key / 2, j + key / 2] = X[i + key / 2, j + key / 2] / 4;
                }
            }
            return X;
        }
        int[,] ArraySubtraction(int[,] array1, int[,] array2)
        {
            for (int i = 0; i < 320; i++)
            {
                for (int j = 0; j < 200; j++)
                {
                    array1[i, j] = array1[i, j] - array2[i, j];
                }
            }
            return array1;
        }
        private void WriteArrayInFile(int[,] pixels, string fileName)
        {
            using (StreamWriter sw = new StreamWriter(path + fileName + ".txt"))
            {

                for (int i = 0; i <= 8; i++)
                {
                    for (int j = 0; j <= 8; j++)
                    {
                        if (pixels[i, j] != 0)
                        {
                            sw.Write(pixels[i, j] + " ");
                        }
                        else
                        {
                            sw.Write("·" + " ");
                        }
                    }
                    sw.WriteLine();
                }
            }
        }
    }
}
