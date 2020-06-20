#include <iostream>
#include <fstream>
#include <vector>
#include <thread>

using namespace std;

inline vector<int> solve(vector<int> a, vector<int> b, int T)
{
	vector<int> res;
	int n = a.size();
	int m = 2 * n - 1;
	res.resize(m);

	vector<thread> thr;
	for (int idx = 0; idx < T; ++idx)
	{
		thr.emplace_back([&, idx]()
		{
			for (int x = idx; x < m; x += T)
			{
				for (int i = 0; i < n; ++i)
				{
					if (x - i >= n || x - i < 0) { continue; }
					res[x] += a[i] * b[x - i];
				}
			}
		});
	}
	for (auto& i : thr)
	{
		i.join();
	}

	res.insert(res.begin(), 0);

	for (int i = res.size() - 1; i >= 0; i--)
	{
		if (res[i] > 9)
		{
			res[i - 1] += res[i] / 10;
			res[i] %= 10;
		}
	}

	return res;
}

int main()
{
	for (auto it : solve({ 1,2,3 }, { 1,2,3 }, 2))
		cout << it << " ";
	cout << "\n";
}