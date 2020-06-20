#include <iostream>
#include <vector>
#include <thread>

using namespace std;

bool isPrime(int n)
{
	for (int j = 2; j <= sqrt(n); j++)
	{
		if (n % j == 0)
		{
			return false;
		}
	}
	return true;
}

void solve(int N, int T)
{
	vector<thread> thr(T);
	int workPerThread = N / T;
	int remainingWork = N % T;

	for (int i = 0; i < T; i++)
	{
		thr[i] = thread([&, i, T]()
		{
			int leftSide = workPerThread * i;
			int rightSide = workPerThread * (i + 1);
			if (leftSide == 0)leftSide = 2;
			if (i == T)rightSide += remainingWork;

			for (int k = leftSide; k < rightSide; k++)
			{
				if (isPrime(k))
				{
					cout << k << " ";
				}
			}
		});
	}

	for (int i = 0; i < T; ++i)
	{
		thr[i].join();
	}
	cout << "Main finished ";
}

int main()
{
	solve(1000, 8);
	return 0;
}