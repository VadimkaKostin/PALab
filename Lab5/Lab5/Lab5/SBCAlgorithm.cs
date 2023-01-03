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
        private int TotalNumberBees;    //Загальна кількість бджіл
        private int NumberInactive;     //Кількість неактивних фуражирів
        private int NumberActive;       //Кількість активних фуражирів
        private int NumberScout;        //Кількість розвідників

        private Graph graph;            //Досліджувальний граф

        private int TotalIterations;                //Кількість ітерацій алгоритму
        private int MaxNumberOfVisits;              //Максимально допустима кількість ділянок для активних фуражирів

        /*Вірогіднісні атрибути. Використовуються для більш наближеної поведінки бджіл в
        коді до реальної поведінки бджіл.*/
        //Вірогідність того, що неактивний фуражир прийме нове рішення під час танцю бджіл
        private double probPersuasion = 0.90;
        //Вірогідність допущення помилки активним фуражиром при порівнянні свого розв'язку із суміжним
        private double probMistake = 0.01;

        private Bee[] Bees;             //Колекція бджіл

        private int[] BestSolution;     //Найкращий розв'язок
        private int BestDistance;       //Дистанція найкращого знайденого розв'язку

        private int[] indexesOfInactiveBees;    //Масив з індексами неактивних фуражирів в колекції Bees

        public SBCAlgorithm(int totalVisits, int inactiveAmount, int activeAmount, int scoutAmount, 
            int maxNumberOfVisits, Graph graph, int totalIterations)
        {
            this.TotalVisits = totalVisits;
            this.TotalNumberBees = inactiveAmount + activeAmount + scoutAmount;
            this.NumberInactive = inactiveAmount;
            this.NumberActive = activeAmount;
            this.NumberScout = scoutAmount;
            this.graph = graph;
            this.TotalIterations = totalIterations;
            this.MaxNumberOfVisits = maxNumberOfVisits;

            Bees = new Bee[TotalNumberBees];

            this.BestSolution = this.GenerateRandomSolution();
            this.BestDistance = GetDistance(this.BestSolution);

            this.indexesOfInactiveBees = new int[this.NumberInactive];

            for (int i = 0; i < TotalNumberBees; ++i)
            {
                BeeStatus currStatus;
                if (i < NumberInactive)
                {
                    currStatus = BeeStatus.Inactive; //Неактивний фуражир
                    indexesOfInactiveBees[i] = i;
                }
                else if (i < NumberInactive + NumberScout)
                    currStatus = BeeStatus.Scout;    //Розвідник
                else
                    currStatus = BeeStatus.Active;   //Активний фуражир

                //Генерація початкового розв'язку жадібним алгоритмом для кожної бджоли
                int[] randomSolution = GenerateRandomSolution();
                int mq = GetDistance(randomSolution);
                int numberOfVisits = 0;

                Bees[i] = new Bee(currStatus,
                  randomSolution, mq, numberOfVisits);

                if (Bees[i].Distance < this.BestDistance)
                {
                    Array.Copy(Bees[i].Solution, this.BestSolution,
                      Bees[i].Solution.Length);
                    this.BestDistance = Bees[i].Distance;
                }
            }
        }
        //Метод генерації рандомного розв'язку задачі жадібним алгоритмом
        private int[] GenerateRandomSolution()
        {
            int[] Solution = new int[graph.CountVertices];

            Solution[0] = new Random().Next(0, graph.CountVertices);

            var CheckedVertices = new List<int>();
            CheckedVertices.Add(Solution[0]);

            for (int i = 0; i < graph.CountVertices - 1; i++)
            {
                Solution[i + 1] = graph.GetNearestVertice(Solution[i], CheckedVertices);

                CheckedVertices.Add(Solution[i + 1]);
            }

            return Solution;
        }
        //Метод генерації сусіднього рішення для заданого
        private int[] GenerateNeighborSolution(int[] Solution)
        {
            int[] NeighborSolution = new int[Solution.Length];
            Array.Copy(Solution, NeighborSolution, Solution.Length);

            int ranIndex = random.Next(Solution.Length/2, Solution.Length);
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
        private int GetDistance(int[] BestSolution)
        {
            int distance = 0;
            for (int i = 0; i < BestSolution.Length - 1; i++)
            {
                distance += graph.GetDistanceBetweenVertices(BestSolution[i], BestSolution[i + 1]);
            }
            return distance;
        }
        //Головний метод алгоритму - пошук розв'язку
        public void Solve()
        {
            int VisitsCount = 0;

            for(int count = 0; count < this.TotalIterations; count++)
            {
                while (VisitsCount < this.TotalVisits)
                {
                    for (int i = 0; i < TotalNumberBees; ++i)
                    {
                        if (this.Bees[i].Status == BeeStatus.Active)
                        {
                            ProcessActiveBee(i);
                            VisitsCount++;
                        }
                        else if (this.Bees[i].Status == BeeStatus.Scout)
                        {
                            ProcessScoutBee(i);
                            VisitsCount++;
                        }
                        else if (this.Bees[i].Status == BeeStatus.Inactive)
                            ProcessInactiveBee(i);
                    }
                }
                VisitsCount = 0;
            }
        }

        /*Наступні методи, що імітують поведінку бджіл, на вхід приймають індекс 
         конкретної бджоли, імітація поведінки якої буде відбуватися в методі */

        //Метод, що імітує поведінку активного фуражира
        private void ProcessActiveBee(int i)
        {
            //Генерація суміжного розв'язку
            int[] neighbor = GenerateNeighborSolution(Bees[i].Solution);
            int neighborQuality = GetDistance(neighbor);

            //Ймовірність допущення помилки бджолою при порівнянні розв'язків
            double prob = random.NextDouble();

            bool memoryWasUpdated = false;
            bool numberOfVisitsOverLimit = false;

            if (neighborQuality < Bees[i].Distance)     //Суміжна ділянка дає кращий розв'язок
            { 
                if (prob < probMistake)                 //Бджола допустила помилку із порівнянням свого і суміжнього розв'язку
                {
                    ++Bees[i].NumberOfVisits;
                    if (Bees[i].NumberOfVisits > this.MaxNumberOfVisits)
                        numberOfVisitsOverLimit = true;
                }
                else                                    //Бджола не допустила помилку із порівнянням свого і суміжнього розв'язку
                {
                    Array.Copy(neighbor, Bees[i].Solution, neighbor.Length);
                    Bees[i].Distance = neighborQuality;
                    Bees[i].NumberOfVisits = 0;
                    memoryWasUpdated = true;
                }
            }
            else
            {
                if (prob < probMistake)
                {
                    Array.Copy(neighbor, Bees[i].Solution, neighbor.Length);
                    Bees[i].Distance = neighborQuality;
                    Bees[i].NumberOfVisits = 0;
                    memoryWasUpdated = true;
                }
                else
                {
                    ++Bees[i].NumberOfVisits;
                    if (Bees[i].NumberOfVisits > this.MaxNumberOfVisits)
                        numberOfVisitsOverLimit = true;
                }
            }

            //Якщо кількість перевірених суміжних ділянок досягла максимально можливої, то активний фуражир стає неактивним
            //Ця процедура виконується для уникнення заціклювання однієї бджоли на одному неефективному розв'язку.
            if (numberOfVisitsOverLimit == true)
            {
                Bees[i].Status = BeeStatus.Inactive;
                Bees[i].NumberOfVisits = 0;
                int x = random.Next(NumberInactive);
                Bees[indexesOfInactiveBees[x]].Status = BeeStatus.Active;
                indexesOfInactiveBees[x] = i;
            }
            else if (memoryWasUpdated == true)     
            {
                //Якщо бджола знайшла в суміжній ділянці кращий розв'язок(новий елітний розв'язок), він порівнюється із найкращим розв'язком за весь час роботи алгоритму,
                //Активний фуражир також повідомляє про знаходження нової елітної ділянки неактивного фуражира
                if (Bees[i].Distance < this.BestDistance)
                {
                    Array.Copy(Bees[i].Solution, this.BestSolution,
                      Bees[i].Solution.Length);
                    this.BestDistance = Bees[i].Distance;
                }
                DoWaggleDance(i);
            }
            else
            {
                return;
            }
        }
        //Метод що імітує поведінку розвідника
        private void ProcessScoutBee(int i)
        {
            //Розвідка нової ділянки(розв'язку)
            int[] randomFoodSource = GenerateRandomSolution();
            int randomFoodSourceQuality = GetDistance(randomFoodSource);

            //Якщо нова розвідана ділянка краща за попередню розвідану цією бджолою ділянку,
            //то цей розвідник запам'ятовує цю ділянку да при поверненні у вулик розповідає
            //про неї неактивному фуражиру через танець бджіл
            if (randomFoodSourceQuality < Bees[i].Distance)
            {
                Array.Copy(randomFoodSource, Bees[i].Solution, randomFoodSource.Length);
                Bees[i].Distance = randomFoodSourceQuality;
                if (Bees[i].Distance < this.BestDistance)
                {
                    Array.Copy(Bees[i].Solution, this.BestSolution,
                      Bees[i].Solution.Length);
                    this.BestDistance = Bees[i].Distance;
                }
                DoWaggleDance(i);
            }
        }
        //Метод що імітує поведінку неактивного фуражира, задачею якого є просто чекати в вулику на повернення активних фуражирів
        //або розвідників і обмін інформації через танець бджоли(тобто вони ніяк не задіяні в пошуку нових ділянок)
        private void ProcessInactiveBee(int i)
        {
            return;
        }

        //Метод що імітує танець бджіл, через який розвідники або активні фуражири повідомляють неактивних фуражирів
        //про знаходження нових кращих розв'язків
        private void DoWaggleDance(int i)
        {
            for (int j = 0; j < NumberInactive; ++j)
            {
                int b = this.indexesOfInactiveBees[j];
                if (Bees[i].Distance < Bees[b].Distance)
                {
                    double p = random.NextDouble();
                    if (this.probPersuasion > p)
                    {
                        Array.Copy(Bees[i].Solution, Bees[b].Solution,
                          Bees[i].Solution.Length);
                        Bees[b].Distance = Bees[i].Distance;
                    }
                }
            }
        }
        //Метод порявняння певного розв'язку алгоритму з найкращим
        private void CheckForBest(int[] Solution, int Distance)
        {
            if (Distance < this.BestDistance)
            {
                this.BestSolution = Solution;
                this.BestDistance = Distance;
            }
        }
        //Метод що повертає значення цільової функції, яке було знайденно пілсфі виклику методу Solve()
        public int ObjectiveFunction() => this.BestDistance;
    }
}