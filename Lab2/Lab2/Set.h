#pragma once

template<class T>
struct SNode
{
	T data;
	SNode<T>* next;

	SNode(T value, SNode<T>* nextptr = NULL) : data(value), next(nextptr) {}
};

template<class T>
class Set
{
private:
	SNode<T>* top;
	int length;
public:
	Set() : top(NULL), length(0) {}
	void insert(T value);
	bool contains(T value);
	int getLength() { return length; }
	~Set();
};

template<class T>
void Set<T>::insert(T value)
{
	SNode<T>* newNode = new SNode<T>(value);
	newNode->next = this->top;
	this->top = newNode;
}

template<class T>
bool Set<T>::contains(T value)
{
	for (SNode<T>* current = this->top; current != NULL; current = current->next)
	{
		if (current->data == value) return true;
	}
	return false;
}

template<class T>
Set<T>::~Set()
{
	SNode<T>* previous = NULL;
	SNode<T>* current = this->top;
	while (current != NULL)
	{
		previous = current;
		current = current->next;
		delete previous;
	}
}