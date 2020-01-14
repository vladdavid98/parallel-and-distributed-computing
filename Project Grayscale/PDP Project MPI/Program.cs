using MPI;
using PDP_Project_MPI;

internal class Pi
{
    private static void Main(string[] args)
    {
        MPIController mpiController = new MPIController();

        using (new MPI.Environment(ref args))
        {
            if (Communicator.world.Rank == 0)
            {
                //master process

                mpiController.grayScaleMaster(@"C:\Users\Vlad\Desktop\parallel-and-distributed-computing\Project Grayscale\PDP Project MPI\Images\mclaren.jpg", @"C:\Users\Vlad\Desktop\parallel-and-distributed-computing\Project Grayscale\PDP Project MPI\Images\mclarenG1.jpg");
                mpiController.grayScaleMaster(@"C:\Users\Vlad\Desktop\parallel-and-distributed-computing\Project Grayscale\PDP Project MPI\Images\mclaren.jpg", @"C:\Users\Vlad\Desktop\parallel-and-distributed-computing\Project Grayscale\PDP Project MPI\Images\mclarenG2.jpg");
                mpiController.grayScaleMaster(@"C:\Users\Vlad\Desktop\parallel-and-distributed-computing\Project Grayscale\PDP Project MPI\Images\mclaren.jpg", @"C:\Users\Vlad\Desktop\parallel-and-distributed-computing\Project Grayscale\PDP Project MPI\Images\mclarenG3.jpg");
            }
            else
            {
                //child process

                mpiController.grayScaleWorker();
                mpiController.grayScaleWorker();
                mpiController.grayScaleWorker();
            }
        }
    }
}