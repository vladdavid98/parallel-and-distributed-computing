#include <iostream>
#include <thread>
#include <string>
#include <vector>
#include <cmath>
using namespace std;

vector<pair<int, int>> splitWork(int n, int t)
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

void mult(vector<int> a, vector<int> b, vector<int>& r, int T)
{
	if (T == 1)
	{
		for (int i = 0; i < a.size(); i++)
			for (int j = 0; j < b.size(); j++)
				r[i + j] += a[i] * b[j];
	}
	else
	{
		vector<thread> threads(T);
		vector<pair<int, int>> intervals = splitWork(a.size(), T);
		for (int k = 0; k < T; k++)
			threads[k] = thread([&, k]()
			{
				for (int i = intervals[k].first; i < intervals[k].second; i++)
					for (int j = 0; j < a.size(); j++)
						r[i + j] += a[i] * b[j];
			});

		for (int i = 0; i < T; i++)
			threads[i].join();
	}
}

int main()
{
	vector<int> firstP = {1, 5, 1, 3, 6, 2, 7, 9, 4, 3};
	vector<int> secondP = {3, -10, 15, 1, 7, 3, 8, 5, 4, 2};
	vector<int> r1(firstP.size() + secondP.size() - 1, 0);
	vector<int> r2(firstP.size() + secondP.size() - 1, 0);

	mult(firstP, secondP, r1, 1);
	mult(firstP, secondP, r2, 6);

	for (auto it : r1)
		cout << it << " ";
	cout << "\n";
	for (auto it : r2)
		cout << it << " ";
	cout << "\n";

	return 0;
}
