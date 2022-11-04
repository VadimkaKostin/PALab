#include <iostream>
#include <ctime>
#include "Search.h"

void CheckBFS();

void CheckAstar();

int main()
{
	srand(time(NULL));

	CheckAstar();
	
	system("pause");
}

void CheckBFS()
{
	Field parent;
	parent.setQueensRandom();

	std::cout << "First step:" << std::endl;

	parent.display();

	std::cout << "Searching for solution..." << std::endl;

	long int start = clock();
	BFS(parent);
	long int end = clock();

	std::cout << "BFS time - " << (end - start) / 1000 << "s" << std::endl;
}

void CheckAstar()
{
	Field parent;
	parent.setQueensRandom();

	std::cout << "First step:" << std::endl;

	parent.display();

	std::cout << "Searching for solution..." << std::endl;

	long int start = clock();
	Astar(parent);
	long int end = clock();

	std::cout << "A* time - " << end - start << "ms" << std::endl;
}

void ChechF2()
{
	int Array[8];
	for (int i = 0; i < 8; i++)
	{
		std::cin >> Array[i];
	}
	Field field(Array);
	std::cout << F2(field) << std::endl;
}