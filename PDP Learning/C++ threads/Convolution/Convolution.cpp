#include <iostream>
#include <thread>
#include <string>
#include <vector>
#include <cmath>
using namespace std;

void solve(vector<int> a, vector<int> b, int T)
{
	vector<thread> threads;
	int n = a.size();
	vector<int> sol(n, 0);
	for (int idx = 0; idx < T; ++idx)
	{
		threads.push_back(thread([a, b, idx, n, &sol, T]()
		{
			for (int i = idx; i < n; i += T)
			{
				for (int j = 0; j < n; ++j)
				{
					sol[i] += a[j] * b[(i - j + n) % n];
				}
			}
		}));
	}
	for (int i = 0; i < threads.size(); ++i)
	{
		threads[i].join();
	}
	for (auto it : sol)
	{
		cout << it << "\n";
	}
}

int main() {
	solve({ 1,2,3 }, { 1,2,3 }, 3);
}