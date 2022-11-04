#include "Field.h"

Field::Field()
{
	for (int i = 0; i < 8; i++)
	{
		Queens[i] = -1;
	}
}

Field::Field(const int Queens[8])
{
	for (int i = 0; i < 8; i++)
	{
		this->Queens[i] = Queens[i];
	}
}

Field::Field(const Field& field)
{
	for (int i = 0; i < 8; i++)
	{
		this->Queens[i] = field.Queens[i];
	}
}

void Field::mooveQueen(int QueenNumber, int position)
{
	if (QueenNumber < 0 || QueenNumber > 7) std::cout << "QueenNumber is incorrect!" << std::endl;
	else if(position < 0 || position > 7) std::cout << "Position is incorrect!" << std::endl;
	else
	{
		this->Queens[QueenNumber] = position;
	}
}

void Field::setQueensRandom()
{
	for (int i = 0; i < 8; i++)
	{
		Queens[i] = rand() % 8;
	}
}

void Field::display()
{
	std::cout << "   a  b  c  d  e  f  g  h " << std::endl;
	for (int i = 0; i < 8; i++)
	{
		std::cout << "  ";
		for (int j = 0; j < 8; j++)
		{
			std::cout << "---";
		}
		std::cout << "-" << std::endl;
		std::cout << 8 - i << " ";
		for (int j = 0; j < 8; j++)
		{
			std::cout << "| ";
			if (this->Queens[j] == i) std::cout << "Q";
			else std::cout << " ";
		}
		std::cout << "| " << 8 - i << std::endl;
	}
	std::cout << "  ";
	for (int j = 0; j < 8; j++)
	{
		std::cout << "---";
	}
	std::cout << "-" << std::endl;
	std::cout << "   a  b  c  d  e  f  g  h " << std::endl;
	std::cout << std::endl;
}

bool Field::CheckForSolution()
{
	//Check for horisontal
	int arr1[8] = { 0,0,0,0,0,0,0,0 };
	for (int i = 0; i < 8; i++) if (arr1[this->Queens[i]]++ == 1) return false;

	//Check for main diagonal
	int arr2[15] = { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 };
	for (int i = 0; i < 8; i++) if (arr2[7 + (i - this->Queens[i])]++ == 1) return false;

	//Check for secondary diagonal
	int arr3[15] = { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 };
	for (int i = 0; i < 8; i++) if (arr3[i + this->Queens[i]]++ == 1) return false;

	return true;
}

bool Field::operator==(Field field)
{
	for (int i = 0; i < 8; i++) if (this->Queens[i] != field.Queens[i]) return false;
	return true;
}

int F2(Field field)
{
	int countPairs = 0;

	//Check for horisontal
	int arr1[8] = { 0,0,0,0,0,0,0,0 };
	for (int i = 0; i < 8; i++) arr1[field.Queens[i]]++;
	countPairs += countPairsInArr(arr1, 8);

	//Check for main diagonal
	int arr2[15] = { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 };
	for (int i = 0; i < 8; i++) arr2[7 + (i - field.Queens[i])]++;
	countPairs += countPairsInArr(arr2, 15);

	//Check for secondary diagonal
	int arr3[15] = { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 };
	for (int i = 0; i < 8; i++) arr3[i + field.Queens[i]]++;
	countPairs += countPairsInArr(arr3, 15);

	return countPairs;
}

int countPairsInArr(int arr[], int n)
{
	int countPairsInArr = 0;
	for (int i = 0; i < n; i++)
	{
		if (arr[i] > 1) countPairsInArr += Combination(arr[i],2);
	}
	return countPairsInArr;
}

int Combination(int n, int m)
{
	if (m == 0 || n == m)
		return 1;
	else
		return Combination(n - 1, m - 1) + Combination(n - 1, m);
}