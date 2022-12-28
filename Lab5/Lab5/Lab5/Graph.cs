using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab5
{
    public class Graph
    {
        private int[,] AdjencyMatrix;
        public int CountVertices { get; private set; }
        public Graph(int AmountOFVertices)
        {
            this.CountVertices = AmountOFVertices;
            AdjencyMatrix = new int[this.CountVertices, this.CountVertices];
        }
        public Graph(int[,] matrix)
        {
            this.CountVertices = matrix.GetLength(0);
            this.AdjencyMatrix = matrix;
        }

        public void GenerateAdjencyMatrix()
        {
            Random random = new Random();

            for (int i = 0; i < CountVertices; i++)
            {
                this.AdjencyMatrix[i, i] = 0;
            }
            for (int i = 0; i < this.CountVertices-1; i++)
            {
                for (int j = i+1; j < this.CountVertices; j++)
                {
                    this.AdjencyMatrix[i, j] = this.AdjencyMatrix[j, i] = random.Next(5, 150);
                }
            }
        }
        public int GetDistanceBetweenVertices(int vertice1, int vertice2)
        {
            if ((vertice1 < 0 || vertice1 >= this.CountVertices) || (vertice2 < 0 || vertice2 >= this.CountVertices))
                throw new ArgumentException();

            return this.AdjencyMatrix[vertice1, vertice2];
        }
    }
}
