// Sum of first K numbers.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
#include <algorithm>
#include <thread>
#include <vector>
#include <cassert>

const int maxlg = 22;
const int maxn = 500005;
int n;
long long dp[maxlg][maxn], sum[maxn], psum[maxn];
const int T = 1;

void doIt(int idx)
{
	for (int i = idx; i < n; i += T)
	{
		int act = 0;
		int now = i + 1;
		for (int bit = 0; (1 << bit) <= now; ++bit)
			if (now & (1 << bit))
			{
				sum[i] += dp[bit][act];
				act += (1 << bit);
			}
	}
}

int main()
{
	std::cin >> n;
	int element;
	for (int i = 0; i < n; ++i)
	{
		std::cin >> element;
		dp[0][i] = element;
		psum[i] = psum[i - 1] + dp[0][i];
	}
	std::cout << std::endl << std::endl;
	for (int k = 1; (1 << k) < maxn; ++k)
	{
		for (int i = 0; i < n; ++i)
		{
			dp[k][i] = dp[k - 1][i] + dp[k - 1][i + (1 << (k - 1))];
		}
	}
	std::vector<std::thread> th;
	for (int i = 0; i < std::min(T, n); ++i)
		th.emplace_back(doIt, i);
	for (auto& i : th)
		i.join();
	for (int i = 0; i < n; ++i)
	{
		std::cout << sum[i] << '\n';
		assert(sum[i] == psum[i]);
	}
}
