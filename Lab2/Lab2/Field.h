#pragma once
#include <iostream>

//Класс поле - імітація шахмотного поля за допомогою одномірного масиву, де індех елемента
//це номер ствопця де розташований ферзь, а значення елемента по індесу - це номер рядка.
//Тоді виходить що кожен ферзь розташований по своємо стовпцю, а рядки можуть співпадати.
class Field
{
private:
	int Queens[8];
public:
	Field();
	Field(const int Queens[8]);
	Field(const Field& field);
	int GetQueenPosition(int QueenNumber) { return Queens[QueenNumber]; }
	void mooveQueen(int QueenNumber, int position);  //QueenNumber відповідає номеру стовпця, position відповідає номеру рядка
	void setQueensRandom();		//Метод що розташовує ферзів в рандомному порядку
	void display();
	bool CheckForSolution();
	bool operator==(Field field);
	friend int F2(Field field);
};

int countPairsInArr(int arr[], int n);

int Combination(int n, int m);