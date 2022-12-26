using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    public class ABCAlgorithm
    {
        //Масив, що містить інформацію про кількість нектару на всіх джерелах на ділянці
        public int[] Food { get; private set; }
        //Ділянка пошуку нектару
        public Graph area { get; private set; }
        //Кількість бджіл-розвідників
        public int Scout { get; private set; }
        //Кількість бджіл-спостерігачів
        public int Onlooker { get; private set; }
        //Список усіх допустимих кольорів
        public List<string> AllColors = new List<string>();
        //Список усіх використаних кольорів
        public List<string> UsedColors = new List<string>();
        //Список розфарбованих вершин
        public Dictionary<int,string> ColoredVertices = new Dictionary<int, string>();
        //Список джерел, в яких вже незалишилось нектару
        public List<int> EmptyVertices = new List<int>();
        //Список обраних розвідниками джерел
        public List<int> ChoosenVertices = new List<int>();

        public ABCAlgorithm(Graph graph, int ScoutAmount, int OnlookerAmount, int maxDegree)
        {
            this.area = graph;
            this.Scout = ScoutAmount;
            this.Onlooker = OnlookerAmount;

            this.Food = new int[area.CountVertices];
            for (int i = 0; i < area.CountVertices; i++)
            {
                Food[i] = area.CountDegree(i);
            }
            for (int i = 1; i <=maxDegree; i++)
            {
                AllColors.Add($"color{i}");
            }
        }
        //Фаза пошуку джерел розвідниками
        public void ScoutPfase()
        {
            Random random = new Random();
            for (int i = 1; i <= this.Scout; i++)
            {
                int flower = random.Next(0, area.CountVertices - 1);
                while(EmptyVertices.Contains(flower)) flower = random.Next(0, area.CountVertices);

                ChoosenVertices.Add(flower);
                if (EmptyVertices.Count + ChoosenVertices.Count == area.CountVertices) break;
            }
        }
        //Фаза танцю бджіл та збору нектару з елітного джерела
        public void OnlookerPhase()
        {
            //Танець бджіл
            double sum = 0;
            foreach(var vertice in ChoosenVertices) sum += Food[vertice];
            
            double MaxP = (double)Food[ChoosenVertices[0]] / sum;
            int choosenVertice = ChoosenVertices[0];
            foreach(var vertice in ChoosenVertices)
            {
                double P = (double)Food[vertice] / sum;
                if(P > MaxP)
                {
                    MaxP = P;
                    choosenVertice = vertice;
                }
            }

            ChoosenVertices.Clear();

            //Збір нектару з елітного джерела та розвідка його околу
            int usedOnlookers = 0;
            foreach (var adjacentVertice in area.GetAdjacentVertices(choosenVertice))
            {
                if(usedOnlookers < Onlooker - 1)
                ColorVertice(adjacentVertice);
                usedOnlookers++;
            }
            ColorVertice(choosenVertice);
            Food[choosenVertice] = 0;
            EmptyVertices.Add(choosenVertice);
        }
        //Метод розфарбування вершини
        public void ColorVertice(int vertice)
        {
            foreach (var color in UsedColors)
            {
                if(CheckColorWithAdjecent(vertice, color))
                {
                    if (ColoredVertices.ContainsKey(vertice)) ColoredVertices[vertice] = color;
                    else ColoredVertices.Add(vertice, color);
                    return;
                }
            }
            foreach (var color in AllColors)
            {
                if (CheckColorWithAdjecent(vertice, color))
                {
                    if (ColoredVertices.ContainsKey(vertice)) ColoredVertices[vertice] = color;
                    else ColoredVertices.Add(vertice, color);
                    if (!UsedColors.Contains(color)) UsedColors.Add(color);
                }
            }
        }
        //Функція перевірки допустимосці розфарбування вершини певним кольором
        public bool CheckColorWithAdjecent(int vertice, string color)
        {
            List<int> adjecentVertices = area.GetAdjacentVertices(vertice);

            foreach(var adjacentVertice in adjecentVertices)
            {
                if (ColoredVertices.Contains(new KeyValuePair<int,string>(adjacentVertice, color))) return false;
            }
            return true;
        }
        //Цільова функція
        public int ObjectiveFunction()
        {
            while(EmptyVertices.Count != area.CountVertices)
            {
                ScoutPfase();
                OnlookerPhase();
                List<string> UsedColorsWithDublicats = ColoredVertices.Values.ToList();
                UsedColors = UsedColorsWithDublicats.Distinct().ToList();
            }
            return UsedColors.Count;
        }
    }
}
