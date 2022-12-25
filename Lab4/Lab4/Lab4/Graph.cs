using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    public class Graph
    {
        //Матриця суміжностей графу
        private int[,] matrix;
        //Кількість вершин графу
        public int CountVertices { get; private set; }
        public Graph(int AmountOfVertexes)
        {
            this.CountVertices = AmountOfVertexes;
            matrix = new int[CountVertices, CountVertices];
            for (int i = 0; i < CountVertices; i++)
            {
                for (int j = 0; j < CountVertices; j++)
                {
                    matrix[i, j] = 0;
                }
            }
        }
        public Graph(int[,] matrix)
        {
            this.CountVertices = matrix.GetLength(0);
            this.matrix = new int[CountVertices, CountVertices];
            for (int i = 0; i < CountVertices; i++)
            {
                for (int j = 0; j < CountVertices; j++)
                {
                    this.matrix[i, j] = matrix[i, j];
                }
            }
        }
        //Рандомна генерація ребер графу із заданим максимальним степінем вершини
        public void GenerateGraphRandomly(int maxDegree)
        {
            Random random = new Random();

            for (int i = 0; i < CountVertices - 1; i++)
            {
                int degree = maxDegree;
                while (degree > 0)
                {
                    int j = random.Next(i + 1, CountVertices - 1);
                    if (this.CountDegree(j) < maxDegree && this.CountDegree(i) < maxDegree)
                    {
                        matrix[i, j] = 1;
                        matrix[j, i] = 1;
                    }
                    degree--;
                }
            }
        }
        //Метод для підрахунку степіню вершини
        public int CountDegree(int vertice)
        {
            int count = 0;
            for (int i = 0; i < CountVertices; i++)
            {
                if (matrix[vertice, i] == 1) count++;
            }
            return count;
        }
        //Метод що повертає список суміжніх вершин для вказаної вершини
        public List<int> GetAdjacentVertices(int vertice)
        {
            List<int> AdjecentVertices = new List<int> ();
            for (int i = 0; i < CountVertices; i++)
            {
                if (matrix[vertice, i] == 1) AdjecentVertices.Add(i);
            }
            return AdjecentVertices;
        }
    }
}