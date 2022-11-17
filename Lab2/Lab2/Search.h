#pragma once
#include <iostream>
#include "Set.h"
#include "Queue.h"
#include "Field.h"

void BFS(Field firstStep)
{
	Queue<Field> queue;
	queue.enqueue(firstStep);
	int counter = 0;
	int totalStates = 1;
	while (true)
	{
		if (counter < 20)
		{
			std::cout << "\nState " << counter + 1 << ":" << std::endl;
			std::cout << "States in memory - " << queue.GetLength() << std::endl;
			std::cout << "Total states - " << totalStates << std::endl;
		}
		Field current = queue.dequeue();
		if (current.CheckForSolution())
		{
			std::cout << "Solution is found!" << std::endl;
			current.display();
			break;
		}
		else
		{
			for (int i = 0; i < 8; i++)
			{
				int indexArr[] = { 0,0,0,0,0,0,0,0 };
				for (int j = 0; j < 7; j++)
				{
					Field childstep = current;
					indexArr[childstep.GetQueenPosition(i)]++;
					int index = rand() % 8;
					while (indexArr[index] == 1) index = rand() % 8;
					indexArr[index]++;
					childstep.mooveQueen(i, index);
					queue.enqueue(childstep);
					totalStates++;
				}
			}
		}
		counter++;
	}
}

void Astar(Field firstStep)
{
	Set<Field> closed;
	PreorityQueue<Field> open;
	int gn = 0;
	int hn = F2(firstStep);
	int fn = gn + hn;
	open.push(firstStep, fn);

	int counter = 0;
	while (true)
	{
		if (counter < 20)
		{
			std::cout << "\nState " << counter + 1 << ":" << std::endl;
			int amount = closed.getLength() + open.GetLength();
			std::cout << "States in memory - " << amount << std::endl;
			std::cout << "Total states - " << amount << std::endl;
		}
		Field current = open.pop();
		if (!closed.contains(current))
		{
			gn++;
			hn = F2(current);
			if (hn == 0)
			{
				std::cout << "Solution is found!" << std::endl;
				current.display();
				break;
			}
			else
			{
				closed.insert(current);
				
				for (int i = 0; i < 8; i++)
				{
					for (int j = 0; j < 8; j++)
					{
						Field childStep = current;
						if (j != childStep.GetQueenPosition(i)) childStep.mooveQueen(i, j);
						hn = F2(childStep);
						fn = gn + hn;
						open.push(childStep, fn);
					}
				}
			}
		}
		counter++;
	}
}