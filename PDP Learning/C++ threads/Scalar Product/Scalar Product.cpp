#include <iostream>
#include <thread>
#include <vector>

using namespace std;

vector<pair<int, int>> splitW(int n, int t)
{
	vector<pair<int, int>> intervals;

	int index = 0;
	int step = n / t;
	int mod = n % t;

	while (index < n)
	{
		intervals.emplace_back(index, index + step + (mod > 0));
		index += step + (mod > 0);
		mod--;
	}

	return intervals;
}

int scalarProduct(vector<int> a, vector<int> b, int T)
{
	vector<int> sums(a.size(), 0);
	vector<thread> threads;
	threads.resize(T);
	int final_sum = 0;

	vector<pair<int, int>> intervals = splitW(a.size(), T);
	for (int i = 0; i < T; i++)
	{
		threads[i] = thread([&, i]()
		{
			for (int k = intervals[i].first; k < intervals[i].second; k++)
			{
				sums[i] += a[k] * b[k];
			}
		});
	}

	for (int i = 0; i < T; i++)
	{
		threads[i].join();
		final_sum += sums[i];
	}
	return final_sum;
}

int main()
{
	cout << scalarProduct({ 1, 2, 3, 4 }, { 1, 2, 1, 2 }, 2);
	return 0;
}