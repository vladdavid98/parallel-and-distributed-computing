#include <mpi.h>
#include <cstdio>
#include <vector>
#include <iostream>

int calcSum(std::vector<int> vect)
{
	int s = 0;
	for (int elem : vect)
	{
		s += elem;
	}
	return s;
}

int main(int argc, char** argv)
{
	// Initialize the MPI environment
	MPI_Init(NULL, NULL);

	// Get the number of processes
	int world_size;
	MPI_Comm_size(MPI_COMM_WORLD, &world_size);


	// Get the rank of the process
	int world_rank;
	MPI_Comm_rank(MPI_COMM_WORLD, &world_rank);


	std::vector<int> inputVector = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 1};
	int chunkSize = inputVector.size() / world_size;
	int rest = inputVector.size() % world_size;


	if (world_rank == 0) // master process
	{
		int totalSum = 0;

		int indexInVector = 0;
		std::vector<int> masterVect;
		for (int i = 0; i < world_size; i++)
		{
			std::vector<int> currentVect;
			if (i == 0) // give chunk and rest to master
			{
				for (int j = 0; j < chunkSize + rest; j++)
				{
					masterVect.push_back(inputVector[indexInVector]);
					indexInVector++;
				}
			}
			else // give chunk to slaves
			{
				for (int j = 0; j < chunkSize; j++)
				{
					currentVect.push_back(inputVector[indexInVector]);
					indexInVector++;
				}
				// send chunk with MPI
				MPI_Send(&currentVect[0], chunkSize, MPI_INT, i, 123, MPI_COMM_WORLD);
			}
		}
		totalSum += calcSum(masterVect);

		for (int i = 1; i < world_size; i++) // receive sums from slaves
		{
			int partialSum = 0;
			MPI_Recv(&partialSum, 1, MPI_INT, MPI_ANY_SOURCE, MPI_ANY_TAG, MPI_COMM_WORLD, MPI_STATUS_IGNORE);
			totalSum += partialSum;
		}
		std::cerr << "total sum: " << totalSum;
	}
	else // slaves
	{
		std::vector<int> inputVector;
		inputVector.resize(chunkSize);
		MPI_Recv(&inputVector[0], chunkSize, MPI_INT, MPI_ANY_SOURCE, MPI_ANY_TAG, MPI_COMM_WORLD, MPI_STATUS_IGNORE);

		int partialSum = calcSum(inputVector);

		MPI_Send(&partialSum, 1, MPI_INT, 0, 123, MPI_COMM_WORLD);
	}


	// // Get the name of the processor
	// char processor_name[MPI_MAX_PROCESSOR_NAME];
	// int name_len;
	// MPI_Get_processor_name(processor_name, &name_len);
	//
	// // Print off a hello world message
	// printf("Hello world from processor %s, rank %d out of %d processors\n",
	//     processor_name, world_rank, world_size);

	// Finalize the MPI environment.
	MPI_Finalize();
}
