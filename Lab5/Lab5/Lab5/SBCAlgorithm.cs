using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab5
{
    internal class SBCAlgorithm
    {
        Random random = new Random();

        private int TotalVisits;        //Загальна кількість ділянок
        private int ActiveAmount;       //Кількість фуражирів
        private int ScoutAmount;        //Кількість розвідників

        private Graph graph;            //Досліджувальний граф

        private int TotalIterations;    //Кількість ітерацій

        private Bee[] Bees;             //Масив бджіл

        private int[] BestSolution;     //Найкращий знайдений розв'язок, з яким на кожній ітерації порівнюються інші розв'язки
        private long BestDistance;      //Дистанція найкращого знайденого розв'язок

        public SBCAlgorithm(int totalVisits, int activeAmount, int scoutAmount, Graph graph, int totalIterations)
        {
            this.TotalVisits = totalVisits;
            this.ActiveAmount = activeAmount;
            this.ScoutAmount = scoutAmount;
            this.graph = graph;
            this.TotalIterations = totalIterations;

            int totalNumberOfBees = ActiveAmount + ScoutAmount;

            Bees = new Bee[totalNumberOfBees];

            this.BestSolution = this.GenerateRandomSolution();
            this.BestDistance = GetDistance(this.BestSolution);

            for (int i = 1; i <= totalNumberOfBees; i++)
            {
                BeeStatus status;
                if (i <= ScoutAmount) status = BeeStatus.Scout;
                else status = BeeStatus.Active;

                int[] randomSolution = GenerateRandomSolution();
                long Distance = GetDistance(randomSolution);

                Bees[i - 1] = new Bee(status, randomSolution, Distance);

                if (Bees[i - 1].Distance < this.BestDistance)
                {
                    Array.Copy(Bees[i - 1].Solution, this.BestSolution, Bees[i - 1].Solution.Length);
                    this.BestDistance = Bees[i - 1].Distance;
                }
            }
        }
        //Метод генерації рандомного розв'язку задачі.
        //Метод генерації заключається в рандомній перестановці верщин графу
        private int[] GenerateRandomSolution()
        {
            int[] Solution = new int[graph.CountVertices];

            for (int i = 0; i < graph.CountVertices; i++)
                Solution[i] = i;

            for (int i = 0; i < graph.CountVertices; i++)
            {
                int r = random.Next(i, graph.CountVertices);
                int temp = Solution[r];
                Solution[r] = Solution[i];
                Solution[i] = temp;
            }

            return Solution;
        }
        //Метод пошуку сусіднього рішення для заданого
        private int[] GenerateNeighborSolution(int[] Solution)
        {
            int[] NeighborSolution = new int[Solution.Length];
            Array.Copy(Solution, NeighborSolution, Solution.Length);

            int ranIndex = random.Next(0, Solution.Length);
            int adjIndex;
            if (ranIndex == Solution.Length - 1)
                adjIndex = 0;
            else
                adjIndex = ranIndex + 1;

            int tmp = NeighborSolution[ranIndex];
            NeighborSolution[ranIndex] = NeighborSolution[adjIndex];
            NeighborSolution[adjIndex] = tmp;

            return NeighborSolution;
        }
        //Метод обрахунку дистнції за розв'язком
        private long GetDistance(int[] BestSolution)
        {
            long distance = 0;
            for (int i = 0; i < BestSolution.Length - 1; i++)
            {
                distance += graph.GetDistanceBetweenVertices(BestSolution[i], BestSolution[i + 1]);
            }
            return distance;
        }
        public void Solve()
        {
            int totalBees = this.ActiveAmount + this.ScoutAmount;
            int count = 0;

            while (count < this.TotalIterations)
            {
                int countVisits = 0;
                while (countVisits < this.TotalVisits)
                {
                    int indexBest = -1;
                    long best = int.MaxValue;

                    int i = 0;
                    for (; i < this.ScoutAmount; i++)
                    {
                        Bees[i].Solution = GenerateRandomSolution();
                        Bees[i].Distance = GetDistance(Bees[i].Solution);
                        if (Bees[i].Distance < best)
                        {
                            best = Bees[i].Distance;
                            indexBest = i;
                        }
                        countVisits++;
                    }
                    Bees[i].Solution = Bees[indexBest].Solution;
                    Bees[i].Distance = best;
                    CheckForBest(Bees[indexBest].Solution, best);
                    i++;
                    countVisits++;
                    for (; i < totalBees; i++)
                    {
                        Bees[i].Solution = GenerateNeighborSolution(Bees[indexBest].Solution);
                        Bees[i].Distance = GetDistance(Bees[i].Solution);
                        CheckForBest(Bees[i].Solution, Bees[i].Distance);
                        countVisits++;
                    }
                }
                count++;
            }
        }
        //Метод порявняння певного розв'язку алгоритму з найкращим
        private void CheckForBest(int[] Solution, long Distance)
        {
            if (Distance < this.BestDistance)
            {
                this.BestSolution = Solution;
                this.BestDistance = Distance;
            }
        }
        public long ObjectiveFunction() => this.BestDistance;
    }
}
