using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    public interface IVerticeColor
    {
        void ColorVertice(int vertice);
    }
    public class VerticeColor : IVerticeColor
    {
        private ABCAlgorithm algorithm;
        public VerticeColor(ABCAlgorithm algorithm)
        {
            this.algorithm = algorithm;
        }
        //Функція перевірки допустимосці розфарбування вершини певним кольором
        private bool CheckColorWithAdjecent(int vertice, string color)
        {
            var adjecentVertices = this.algorithm.area.GetAdjacentVertices(vertice).ToList();

            foreach (var adjacentVertice in adjecentVertices)
            {
                if (this.algorithm.ColoredVertices.Contains(new KeyValuePair<int, string>(adjacentVertice, color))) 
                    return false;
            }
            return true;
        }
        //Метод розфарбування вершини
        public void ColorVertice(int vertice)
        {
            foreach (var color in this.algorithm.UsedColors)
            {
                if (CheckColorWithAdjecent(vertice, color))
                {
                    if (this.algorithm.ColoredVertices.ContainsKey(vertice)) this.algorithm.ColoredVertices[vertice] = color;
                    else this.algorithm.ColoredVertices.Add(vertice, color);
                    return;
                }
            }
            foreach (var color in this.algorithm.AllColors)
            {
                if (CheckColorWithAdjecent(vertice, color))
                {
                    if (this.algorithm.ColoredVertices.ContainsKey(vertice)) this.algorithm.ColoredVertices[vertice] = color;
                    else this.algorithm.ColoredVertices.Add(vertice, color);
                    if (!this.algorithm.UsedColors.Contains(color)) this.algorithm.UsedColors.Add(color);
                }
            }
        }
        
    }
}
