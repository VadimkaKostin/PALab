using System;

namespace Lab5
{ 
    public static class Program
    {
        public static void Main()
        {
            //Генерація графу
            Graph graph = new Graph(300);
            Console.WriteLine("Generating graph...");
            graph.GenerateAdjencyMatrix();
            Console.WriteLine("\nGraph is generated!");

            int TotalVisits = 0;
            int ActiveAmount = 0;
            int ScoutAmount = 0;

            //=====================Перше дослідження===================================
            //Введення початкової кількості ділянок та бджіл та валдіація введених даних
            EnterDataWidthValidation(ref TotalVisits, ref ActiveAmount, ref ScoutAmount);

            //Змінюємо кількість ділянок
            SolveWithVisits(graph, TotalVisits, ActiveAmount, ScoutAmount);

            //=====================Друге дослідження===================================
            //Введення початкової кількості ділянок та бджіл та валдіація введених даних
            EnterDataWidthValidation(ref TotalVisits, ref ActiveAmount, ref ScoutAmount);

            //Змінюємо кількість фуражирів
            SolveWithActiveAmount(graph, TotalVisits, ActiveAmount, ScoutAmount);

            //=====================Третє дослідження===================================
            //Введення початкової кількості ділянок та бджіл та валдіація введених даних
            EnterDataWidthValidation(ref TotalVisits, ref ActiveAmount, ref ScoutAmount);

            //Змінюємо кількість фуражирів
            SolveWithScoutAmount(graph, TotalVisits, ActiveAmount, ScoutAmount);

            Console.WriteLine("\nIterations stoped!");
        }

        public static void EnterDataWidthValidation(ref int TotalVisits, ref int ActiveAmount, ref int ScoutAmount, int MaxVisits = 1000)
        {
            bool IsStringIncorrect = false;

            Console.Write("Enter start amount of visits and bees(active and scout): ");
            string res = Console.ReadLine();
            if (res == null || res.Split(' ').Length != 3) IsStringIncorrect = true;

            TotalVisits = int.Parse(res.Split(' ')[0]);
            ActiveAmount = int.Parse(res.Split(' ')[1]);
            ScoutAmount = int.Parse(res.Split(' ')[2]);

            while (TotalVisits < 10 || ActiveAmount < 5 || (ScoutAmount < 2 || ScoutAmount > ActiveAmount) || IsStringIncorrect)
            {
                Console.WriteLine("\nIncorrect value!");
                Console.WriteLine("Total visits must have value more than 10.");
                Console.WriteLine("Active amount must have value more than 5.");
                Console.WriteLine("Scout amount must have value between 2 and ActiveAmount.");

                Console.Write("\nEnter values again: ");
                res = Console.ReadLine();
                if (res == null || res.Split(' ').Length != 2) IsStringIncorrect = true;
                else IsStringIncorrect = false;

                TotalVisits = int.Parse(res.Split(' ')[0]);
                ActiveAmount = int.Parse(res.Split(' ')[1]);
                ScoutAmount = int.Parse(res.Split(' ')[2]);
            }
        }

        //Метод для дослідження значення цільової функції змінюючи кількість ділянок
        public static void SolveWithVisits(Graph graph, int TotalVisits, int ActiveAmount, int ScoutAmount)
        {
            Console.WriteLine("\nChenging amount of visits:");
            while (true)
            {
                //Загалом кількість неактивних фуражирів у 2.5 менша за кількісь активних фуражирів
                int InactiveAmount = (int)Math.Round(ActiveAmount / 2.5);
                int maxNumberOfVisits = 15;
                SBCAlgorithm algorithm = new SBCAlgorithm(TotalVisits, InactiveAmount, ActiveAmount, ScoutAmount, 
                    maxNumberOfVisits, graph, 10);

                algorithm.Solve();

                int result = algorithm.ObjectiveFunction();

                Console.WriteLine($"\n\tVisits:{TotalVisits} Bees:{ActiveAmount+ScoutAmount}(Active:{ActiveAmount} Scout:{ScoutAmount})");

                TotalVisits += 10;

                Console.WriteLine($"\tBest - {result}");
                Console.Write("\tContinue(1/0):");

                if (Console.ReadLine() == "0") break;
            }
        }
        //Метод дослідження значення цільової функції, змінюючи значення кількіості активних фуражирів
        public static void SolveWithActiveAmount(Graph graph, int TotalVisits, int ActiveAmount, int ScoutAmount)
        {
            Console.WriteLine("\nChenging amount of active bees:");
            int TotalBees = ActiveAmount + ScoutAmount;
            while (TotalBees <= TotalVisits)
            {
                int InactiveAmount = (int)Math.Round(ActiveAmount / 2.5);
                int maxNumberOfVisits = 15;
                SBCAlgorithm algorithm = new SBCAlgorithm(TotalVisits, InactiveAmount, ActiveAmount, ScoutAmount,
                    maxNumberOfVisits, graph, 10);

                algorithm.Solve();

                int result = algorithm.ObjectiveFunction();

                Console.WriteLine($"\n\tVisits:{TotalVisits} Bees:{ActiveAmount + ScoutAmount}(Active:{ActiveAmount} Scout:{ScoutAmount})");

                ActiveAmount += 1;

                Console.WriteLine($"\tBest - {result}");
                Console.Write("\tContinue(1/0):");

                if (Console.ReadLine() == "0") break;
            }
        }
        //Метод дослідження значення цільової функції, змінюючи значення кількіості розвідників
        public static void SolveWithScoutAmount(Graph graph, int TotalVisits, int ActiveAmount, int ScoutAmount)
        {
            Console.WriteLine("\nChenging amount of scout bees:");
            int TotalBees = ActiveAmount + ScoutAmount;
            while (TotalBees <= TotalVisits)
            {
                int InactiveAmount = (int)Math.Round(ActiveAmount / 2.5);
                int maxNumberOfVisits = 15;
                SBCAlgorithm algorithm = new SBCAlgorithm(TotalVisits, InactiveAmount, ActiveAmount, ScoutAmount,
                    maxNumberOfVisits, graph, 10);

                algorithm.Solve();

                int result = algorithm.ObjectiveFunction();

                Console.WriteLine($"\n\tVisits:{TotalVisits} Bees:{ActiveAmount + ScoutAmount}(Active:{ActiveAmount} Scout:{ScoutAmount})");

                ScoutAmount += 1;

                Console.WriteLine($"\tBest - {result}");
                Console.Write("\tContinue(1/0):");

                if (Console.ReadLine() == "0") break;
            }
        }
    }
}