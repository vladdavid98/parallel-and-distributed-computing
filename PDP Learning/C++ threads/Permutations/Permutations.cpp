#include <iostream>
#include <fstream>
#include <vector>
#include <thread>
#include <atomic>

using namespace std;

atomic <int> cnt;

bool check(vector<int> c)
{
	if (c[0] % 2 == 0)
		return true;

	return true;
}

bool contains(vector <int> v, int n) {
	for (auto it : v) {
		if (it == n) {
			return true;
		}
	}
	return false;
}

inline void perm(vector <int> c, int n, int pos, int T) {
	if (pos == n) {
		if (check(c)) {
			++cnt;
		}

		return;
	}

	if (T == 1) {
		for (int i = 0; i < n; i++) {
			if (contains(c, i)) continue;
			c.push_back(i);
			perm(c, n, pos + 1, T);
			c.pop_back();
		}
	}
	else {
		thread t([&]() {
			vector <int> newPath(c);
			for (int i = 0; i < n; i += 2) {
				if (contains(c, i)) continue;
				newPath.push_back(i);
				perm(newPath, n, pos + 1, T / 2);
				newPath.pop_back();
			}
		});
		vector <int> aux(c);
		for (int i = 1; i < n; i += 2) {
			if (contains(aux, i)) continue;
			aux.push_back(i);
			perm(aux, n, pos + 1, T - T / 2);
			aux.pop_back();
		}
		t.join();
	}
}

int main() {
	perm(vector<int>(), 4, 0, 2);
	cout << cnt << "\n";
}