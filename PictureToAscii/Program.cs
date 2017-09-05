using System;
using System.Drawing;
using System.ComponentModel;
using System.IO;
namespace PictureToAscii
{
    class MainClass
    {
        static string charset = "MNHQ&OC?7>!:-;.";
        public static void Main(string[] args)
        {
            var res = GenerateString("./test.bmp");
            Console.WriteLine(res);
        }
        private static int[,] RGB2GRAY(Bitmap bmp)
        {
            int[,] Gray = new int[bmp.Width, bmp.Height];
            Color curColor;
            for (int i = 0; i < bmp.Width;i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    //get pixels one by one
                    curColor = bmp.GetPixel(i, j);
                    //caculate grayscale of this pixel
                    Gray[i, j] = (int)(curColor.R * 0.299 + curColor.G * 0.587 + curColor.B * 0.114);
                }
            }

            return Gray;
        }
		//calculate blocks' size
		//int rowSize = pictureBox1.Width;
		//int ColSize = rowSize;
        private static string GenerateString(string picPath)
        {
            //read a picture from filestream
            FileStream fs = File.OpenRead(picPath);
            int fileLength = 0;

            fileLength = (int)fs.Length;
            Byte[] image = new Byte[fileLength];
            fs.Read(image, 0, fileLength);
            Console.WriteLine(fs.Name);
            System.Drawing.Image result = System.Drawing.Image.FromStream(fs);
            fs.Close();
            Bitmap img = new Bitmap(result);

            int[,] grayImg = RGB2GRAY(img);
            //Blocks' size
            int rowSize = img.Width / 100 + 1;
            int colSize = rowSize / 2;

            string res = null;

            for (int h = 0; h < img.Height / rowSize;h++)
            {
                int hOffset = h * rowSize;
                for (int w = 0; w < img.Width / colSize;w++)
                {
                    int wOffset = w * colSize;
                    int avgGray = 0;
                    for (int x = 0; x < rowSize;x++)
                    {
                        for (int y = 0; y < colSize;y++)
                        {
                            avgGray += grayImg[wOffset + y, hOffset + x];
                        }
                    }
                    avgGray /= rowSize * colSize;

                    //wich gray grade?
                    if (avgGray / 17 < charset.Length)
                    {
                        res += charset[avgGray / 17];
                    }
                    else
                    {
                        res += " ";
                    }
                    int percentComplete = (int)((float)(h * img.Width / colSize + w) / (float)((img.Height / rowSize) * (img.Width / colSize)) * 100);
                    Console.WriteLine(percentComplete);
				}
                res += "\r\n";
            }
            return res;
        }
    }
}
