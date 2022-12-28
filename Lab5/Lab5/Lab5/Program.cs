using System;

namespace Lab5
{ 
    public static class Program
    {
        //Змінні для знаходження піку більш оптимального розв'язку
        public static long PreviousSolution = int.MaxValue;
        public static long BestSolution = int.MaxValue;
        public static void Main()
        {
            //Генерація графу
            Graph graph = new Graph(300);
            Console.WriteLine("Generating graph...");
            graph.GenerateAdjencyMatrix();
            Console.WriteLine("\nGraph is generated!");

            int TotalVisits = 0;
            int BeesAmount = 0;
            int MaxVisits = 0;

            //=====================Перше дослідження===================================
            //Введення початкової кількості ділянок та бджіл та валдіація введених даних
            EnterDataWidthValidation(ref TotalVisits, ref BeesAmount);

            //Змінюємо кількість ділянок
            SolveWithVisits(graph, TotalVisits, BeesAmount, ref MaxVisits);
            
            //=====================Друге дослідження===================================
            //Введення початкової кількості ділянок та бджіл та валдіація введених даних
            EnterDataWidthValidation(ref TotalVisits, ref BeesAmount, MaxVisits);

            //Змінюємо кількість фуражирів
            SolveWithBeesAmount(graph, TotalVisits, ref BeesAmount);


            Console.WriteLine("\nIterations stoped!");
        }

        public static void EnterDataWidthValidation(ref int TotalVisits, ref int BeesAmount, int MaxVisits = 1000)
        {
            bool IsStringIncorrect = false;

            Console.Write("Enter start amount of visits and bees: ");
            string res = Console.ReadLine();
            if (res == null || res.Split(' ').Length != 2) IsStringIncorrect = true;

            TotalVisits = int.Parse(res.Split(' ')[0]);
            BeesAmount = int.Parse(res.Split(' ')[1]);

            while((TotalVisits < 100 || TotalVisits > MaxVisits) || (BeesAmount < 50 || BeesAmount > 100) || IsStringIncorrect)
            {
                Console.WriteLine("\nIncorrect value!");
                Console.WriteLine($"Total visits must have value beetween 100 and {MaxVisits}.");
                Console.WriteLine("Bees amount must have value beetween 50 and 100.");

                Console.Write("\nEnter values again: ");
                res = Console.ReadLine();
                if (res == null || res.Split(' ').Length != 2) IsStringIncorrect = true;
                else IsStringIncorrect = false;

                TotalVisits = int.Parse(res.Split(' ')[0]);
                BeesAmount = int.Parse(res.Split(' ')[1]);
            }
        }

        //Метод для дослідження значення цільової функції змінюючи кількість ділянок
        public static void SolveWithVisits(Graph graph, int TotalVisits, int TotalBees, ref int MaxVisits)
        {
            Console.WriteLine("\nChenging amount of visits:");
            while (true)
            {
                int ScoutAmount = TotalBees / 10;
                int ActiveAmount = TotalBees - ScoutAmount;
                MaxVisits = TotalVisits;
                SBCAlgorithm algorithm = new SBCAlgorithm(TotalVisits, ActiveAmount, ScoutAmount, graph, 5000);

                algorithm.Solve();

                PreviousSolution = BestSolution;
                BestSolution = algorithm.ObjectiveFunction();

                Console.WriteLine($"\n\tVisits:{TotalVisits} Bees:{TotalBees}(Active:{ActiveAmount} Scout:{ScoutAmount})");

                TotalVisits += 100;
                Console.WriteLine($"\tBest - {BestSolution}");
                Console.Write("\tContinue(1/0):");
                if (Console.ReadLine() == "0")
                {
                    BestSolution = PreviousSolution;
                    break;
                }
            }
        }
        //Метод дослідження значення цільової функції, змінюючи значення кількіості бджіл
        public static void SolveWithBeesAmount(Graph graph, int TotalVisits, ref int TotalBees)
        {
            Console.WriteLine("\nChenging amount of active bees:");
            while (true)
            {
                int ScoutAmount = TotalBees / 10;
                int ActiveAmount = TotalBees - ScoutAmount;
                SBCAlgorithm algorithm = new SBCAlgorithm(TotalVisits, ActiveAmount, ScoutAmount, graph, 5000);

                algorithm.Solve();

                PreviousSolution = BestSolution;
                BestSolution = algorithm.ObjectiveFunction();
                
                Console.WriteLine($"\n\tVisits:{TotalVisits} Bees:{TotalBees}(Active:{ActiveAmount} Scout:{ScoutAmount})");

                TotalBees += 10;
                Console.WriteLine($"\tBest - {BestSolution}\n");
                Console.Write("\tContinue(1/0):");
                if (Console.ReadLine() == "0")
                {
                    BestSolution = PreviousSolution;
                    break;
                }
            }
        }
    }
}