#include <iostream>
#include <fstream>
#include <vector>
#include <thread>
#include <atomic>

using namespace std;

atomic <int> cnt;

bool check(vector<int> configuration)
{
	if (configuration[0] % 2 == 0)
		return true;

	return true;
}

inline void subs(vector <int> c, int n, int k, int pos, int T) {
	if (pos == k) {
		if (check(c)) {
			for (auto it : c)
				cout << it << " ";
			cout << "\n";
			++cnt;
		}

		return;
	}

	int last = -1;
	if (!c.empty())
		last = c.back();

	if (T == 1) {
		for (int i = last + 1; i < n; ++i) {
			c.push_back(i);
			subs(c, n, k, pos + 1, T);
			c.pop_back();
		}
	}
	else {
		thread t([&]() {
			vector <int> newPath(c);
			for (int i = last + 1; i < n; i += 2) {
				newPath.push_back(i);
				subs(newPath, n, k, pos + 1, T / 2);
				newPath.pop_back();
			}
		});
		vector <int> aux(c);
		for (int i = last + 2; i < n; i += 2) {
			aux.push_back(i);
			subs(aux, n, k, pos + 1, T - T / 2);
			aux.pop_back();
		}
		t.join();
	}
}

int main() {
	subs(vector<int>(), 4, 3, 0, 4);
	cout << cnt << "\n";
}