using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDP_Project
{
    internal class BasicSimpleThreadingTasks
    {
        public static Bitmap BlackAndWhite(Bitmap image, int processCount)
        {
            int columnWidth = image.Width / processCount;
            List<Task<Bitmap>> tasks = new List<Task<Bitmap>>();
            int newRectEnd = 0;
            for (int i = 0; i < processCount; i++)
            {
                Rectangle newRect = (new Rectangle(columnWidth * i, 0, columnWidth, image.Height));
                newRect.Intersect(new Rectangle(0, 0, image.Width, image.Height));

                Bitmap partial = image.Clone(newRect, image.PixelFormat);

                Console.WriteLine(partial.Height + " " + partial.Width);
                tasks.Add(BlackAndWhite(partial));
            }

            tasks.ForEach(task => task.Start());
            Task.WaitAll(tasks.ToArray());
            Bitmap grayedBMP = new Bitmap(image.Width, image.Height);
            Graphics graphics = Graphics.FromImage(grayedBMP);
            for (int i = 0; i < tasks.Count; i++)
            {
                Bitmap partial = tasks[i].Result;
                graphics.DrawImage(partial, new Point(columnWidth * i, 0));
            }

            return grayedBMP;
        }

        private static Task<Bitmap> BlackAndWhite(Bitmap image)
        {
            return new Task<Bitmap>(() =>
            {
                Bitmap grayScale = new Bitmap(image.Width, image.Height);

                for (Int32 y = 0; y < grayScale.Height; y++)
                    for (Int32 x = 0; x < grayScale.Width; x++)
                    {
                        Color c = image.GetPixel(x, y);
                        int greyPower = (int)((c.R * 0.3) + (c.G * 0.59) + (c.B * 0.11));

                        grayScale.SetPixel(x, y, Color.FromArgb(c.A, greyPower, greyPower, greyPower));

                        //                        grayScale.SetPixel(x, y, greyPower, greyPower, greyPower);
                    }

                return grayScale;
            });
        }
    }
}