#include <mpi.h>
#include <vector>
#include <algorithm>
#include <iostream>
#include <chrono> 

// quicksort with mpi


int partition(std::vector<int> arr, int low, int high)
{
	int pivot = arr[high]; // pivot  
	int i = (low - 1); // Index of smaller element  

	for (int j = low; j <= high - 1; j++)
	{
		// If current element is smaller than the pivot  
		if (arr[j] < pivot)
		{
			i++; // increment index of smaller element  
			int c = arr[i];
			arr[i] = arr[j];
			arr[j] = c;
		}
	}
	int c = arr[i+1];
	arr[i+1] = arr[high];
	arr[high] = c;
	return (i + 1);
}



void quickSort(std::vector<int> arr, int low, int high)
{
	if (low < high)
	{
		/* pi is partitioning index, arr[p] is now
		at right place */
		int pi = partition(arr, low, high);

		// Separately sort elements before  
		// partition and after partition  
		quickSort(arr, low, pi - 1);
		quickSort(arr, pi + 1, high);
	}
}

std::vector<int> my_sort(std::vector<int> to_sort)
{
	quickSort(to_sort, 0, to_sort.size()-1);
	return to_sort;
}


int main(int argc, char** argv)
{
	// Initialize the MPI environment
	MPI_Init(nullptr, nullptr);

	// Get the number of processes
	int world_size;
	MPI_Comm_size(MPI_COMM_WORLD, &world_size);

	// Get the rank of the process
	int world_rank;
	MPI_Comm_rank(MPI_COMM_WORLD, &world_rank);


	if (world_rank == 0) // master
	{
		
		std::vector<int> numbers; // this vector has to be sorted.
		numbers.resize(100000);
		for(int i = 0;i<100000;i++)
		{
			numbers[i] = (rand() % 1000);
		}

		auto start = std::chrono::high_resolution_clock::now();
		
		int slice_size = numbers.size() / world_size;
		int rest_size = numbers.size() % world_size;

		// master will sort the first slice_size+rest_size numbers, and each slave will sort slice_size numbers.

		for(int slave_rank = 1;slave_rank<world_size;slave_rank++)
		{
			std::vector<int> slave_work = std::vector<int>(numbers.begin()+rest_size+(slice_size*slave_rank), numbers.begin() + rest_size + (slice_size * (slave_rank+1)));
			MPI_Send(&slice_size, 1, MPI_INT, slave_rank, 0, MPI_COMM_WORLD);
			MPI_Send(&slave_work[0], slice_size, MPI_INT, slave_rank, 0, MPI_COMM_WORLD);
		}

		std::vector<int> master_work = std::vector<int>(numbers.begin(), numbers.begin() + slice_size + rest_size);
		master_work = my_sort(master_work);

		int index_in_numbers_vector = 0;
		for (index_in_numbers_vector = 0; index_in_numbers_vector < master_work.size(); index_in_numbers_vector++)
		{
			numbers[index_in_numbers_vector] = master_work[index_in_numbers_vector];
		}

		for (int slave_rank = 1; slave_rank < world_size; slave_rank++)
		{
			std::vector<int> slave_work;
			slave_work.resize(slice_size);
			MPI_Recv(&slave_work[0], slice_size, MPI_INT, slave_rank, 0, MPI_COMM_WORLD, MPI_STATUS_IGNORE);

			for(int e: slave_work)
			{
				numbers[index_in_numbers_vector] = e;
				index_in_numbers_vector++;
			}
		}



		auto stop = std::chrono::high_resolution_clock::now();
		auto duration = std::chrono::duration_cast<std::chrono::microseconds>(stop - start);
		std::cout << duration.count() << std::endl;

		//finally, print numbers.
		// for (int i : numbers)
		// {
		// 	std::cout << i << " ";
		// }
		std::cout << "How many numbers: " << numbers.size();
		std::cout << std::endl;
	}
	else // slaves
	{
		int slice_size = 0;
		MPI_Recv(&slice_size, 1, MPI_INT, 0, 0, MPI_COMM_WORLD, MPI_STATUS_IGNORE);
		
		std::vector<int> slave_work;
		slave_work.resize(slice_size);
		MPI_Recv(&slave_work[0], slice_size, MPI_INT, 0, 0, MPI_COMM_WORLD, MPI_STATUS_IGNORE);

		// sort then send back
		slave_work = my_sort(slave_work);
		MPI_Send(&slave_work[0], slice_size, MPI_INT, 0, 0, MPI_COMM_WORLD);
	}


	// Finalize the MPI environment.
	MPI_Finalize();
}
