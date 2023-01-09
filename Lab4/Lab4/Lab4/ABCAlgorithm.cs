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
            double maxFood = Food[ChoosenVertices[0]];
            int choosenVertice = ChoosenVertices[0];
            foreach(var vertice in ChoosenVertices)
            {
                double food = Food[vertice];
                if(food > maxFood)
                {
                    maxFood = food;
                    choosenVertice = vertice;
                }
            }

            ChoosenVertices.Clear();

            //Збір нектару з елітного джерела та розвідка його околу
            VerticeColor verticeColor = new VerticeColor(this);
            int usedOnlookers = 0;
            foreach (var adjacentVertice in area.GetAdjacentVertices(choosenVertice))
            {
                if(usedOnlookers < Onlooker - 1)
                verticeColor.ColorVertice(adjacentVertice);
                usedOnlookers++;
            }
            verticeColor.ColorVertice(choosenVertice);
            Food[choosenVertice] = 0;
            EmptyVertices.Add(choosenVertice);
        }
        public void Solve(bool IsProgressBarEnabled)
        {
            if (IsProgressBarEnabled)
            {
                Console.WriteLine("\n\nProgress: |==========|");
                Console.Write("           ");
            }
            while (EmptyVertices.Count != area.CountVertices)
            {
                ScoutPfase();
                OnlookerPhase();
                List<string> UsedColorsWithDublicats = ColoredVertices.Values.ToList();
                UsedColors = UsedColorsWithDublicats.Distinct().ToList();
                if (IsProgressBarEnabled && EmptyVertices.Count % (area.CountVertices / 10) == 0)
                    Console.Write("^");
            }
        }
        //Цільова функція
        public int ObjectiveFunction() => UsedColors.Count;
    }
}
