// CPP program to generate all unique
// partitions of an integer
#include<iostream>
#include <vector>
using namespace std;

void printArray(vector<int> p, int n)
{
	for (int i = 0; i < n; i++)
		cout << p[i] << " ";
	cout << endl;
}

void printAllUniqueParts(vector<int>input)
{
}

// Driver program to test above functions
int main()
{
	printAllUniqueParts({ 2 });

	return 0;
}