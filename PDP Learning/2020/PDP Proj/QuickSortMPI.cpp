// #include <iostream>
// #include <mpi.h>
// #include <vector>
// #include <time.h>
// #include <chrono>
// #include <stdint.h>
// #include <string>
//
// using namespace std;
//
// vector<int> vectFromArr(int src[])
// {
// 	int n = sizeof(src) / sizeof(src[0]);
//
// 	std::vector<int> dest(src, src + n);
// 	return dest;
// }
//
// int sumVect(vector<int> ipt)
// {
// 	int s = 0;
// 	for (int e : ipt)
// 	{
// 		s += e;
// 	}
// 	return s;
// }
//
// int main(int argc, char** argv)
// {
// 	MPI_Init(nullptr, nullptr);
// 	int rank;
// 	int size;
// 	MPI_Comm_size(MPI_COMM_WORLD, &size);
// 	MPI_Comm_rank(MPI_COMM_WORLD, &rank);
//
// 	if (rank == 0) // master 
// 	{
// 		vector<int> toCalc = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
//
// 		int fullRes = 0;
//
// 		int sliceSize = toCalc.size() / (size - 1);
// 		int sliceRest = toCalc.size() % (size - 1);
//
// 		int destRank = 1;
//
// 		cout << "abababab";
//
// 		vector<int> currSlice;
// 		int i = 0;
// 		for (i; i < sliceSize + sliceRest; i++)
// 		{
// 			currSlice.emplace_back(toCalc[i]);
// 		}
// 		MPI_Send(currSlice.data(), currSlice.size(), MPI_INT, destRank, 0, MPI_COMM_WORLD);
// 		currSlice.clear();
// 		destRank++;
//
// 		for (i; i < toCalc.size(); i++)
// 		{
// 			currSlice.emplace_back(toCalc[i]);
//
// 			if (currSlice.size() == sliceSize)
// 			{
// 				MPI_Send(currSlice.data(), currSlice.size(), MPI_INT, destRank, 0, MPI_COMM_WORLD);
// 				currSlice.clear();
// 				destRank++;
// 			}
// 		}
//
// 		int partialRes = 0;
// 		for (i = 0; i < size - 1; i++)
// 		{
// 			MPI_Recv(&partialRes, 1, MPI_INT, MPI_ANY_SOURCE, MPI_ANY_TAG, MPI_COMM_WORLD, MPI_STATUS_IGNORE);
//
// 			fullRes += partialRes;
// 		}
// 		cout << endl << fullRes;
// 	}
// 	else // slave
// 	{
// 		vector<int> partialSlice;
// 		partialSlice.resize(10);
// 		MPI_Recv(partialSlice.data(), 10, MPI_INT, 0, 0, MPI_COMM_WORLD, MPI_STATUS_IGNORE);
// 		cout << "Rank " << rank << " received vector ";
// 		for (auto i = partialSlice.begin(); i != partialSlice.end(); ++i)
// 			std::cout << *i << ' ';
// 		int partialSum = sumVect(partialSlice);
// 		MPI_Send(&partialSum, 1, MPI_INT, 0, 0, MPI_COMM_WORLD);
// 	}
//
//
// 	MPI_Finalize();
// }
