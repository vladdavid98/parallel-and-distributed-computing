using System;
using System.Drawing;
using MPI;

namespace PDP_Project_MPI
{
    internal class MPIController
    {
        public MPIController()
        {
        }

        // GRAY SCALE
        public void grayScaleMaster(String oldPath, String newPath)
        {
            DateTime start = DateTime.Now;
            Bitmap image = new Bitmap(oldPath);

            int n = Communicator.world.Size;

            int begin = 0;
            int end = 0;
            int length = image.Height / (n - 1);

            for (int i = 1; i < n; i++)
            {
                begin = end;
                end = end + length;
                if (i == n - 1)
                {
                    end = image.Height;
                }

                Communicator.world.Send(image, i, 0);
                Console.WriteLine("Sent image");
                Communicator.world.Send(begin, i, 0);
                Console.WriteLine("Sent index 1: " + begin);
                Communicator.world.Send(end, i, 0);
                Console.WriteLine("Sent index 2: " + end);
            }

            int height = image.Height;
            int width = image.Width;
            int[,] results = new int[height, width];

            for (int i = 1; i < n; i++)
            {
                int beginIndex = Communicator.world.Receive<int>(i, 0);
                int endIndex = Communicator.world.Receive<int>(i, 0);
                int[,] partialResult = Communicator.world.Receive<int[,]>(i, 0);

                for (int heightIndex = beginIndex; heightIndex < endIndex; heightIndex++)
                {
                    for (int widthIndex = 0; widthIndex < width; widthIndex++)
                    {
                        results[heightIndex, widthIndex] = partialResult[heightIndex, widthIndex];
                    }
                }
            }

            ImageOperations imageOperations = new ImageOperations(image);
            imageOperations.exportGrayScaleImage(results, oldPath, newPath);

            Console.WriteLine("Exported file " + newPath + " " + (DateTime.Now - start));
        }

        public void grayScaleWorker()
        {
            Console.WriteLine("Child " + Communicator.world.Rank);
            Bitmap image = Communicator.world.Receive<Bitmap>(0, 0);
            Console.WriteLine("Received image");
            int begin = Communicator.world.Receive<int>(0, 0);
            Console.WriteLine("Received index 1");
            int end = Communicator.world.Receive<int>(0, 0);
            Console.WriteLine("Received index 2");

            ImageOperations imageOperations = new ImageOperations(image);
            int[,] result = imageOperations.MPIgrayScale(image, begin, end);

            Communicator.world.Send(begin, 0, 0);
            Communicator.world.Send(end, 0, 0);
            Communicator.world.Send(result, 0, 0);
            Console.WriteLine("Sent result from child " + Communicator.world.Rank);
        }
    }
}