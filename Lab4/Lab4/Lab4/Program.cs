using System;

namespace Lab4
{
    public class Program
    {
        public static void Main()
        {
            //Створення графу із 300 вершинами
            Console.WriteLine("Creating graph...");
            Graph graph = new Graph(300);
            graph.GenerateGraphRandomly(30);
            Console.WriteLine("Graph is created!");

            Console.Write("\nPress Enter to start iterations: ");
            Console.ReadLine();

            Console.WriteLine("\nIteration Started.");
            //Ітерація із знаходженням значення цільової функції
            int iteration = 0;
            int bestCromatickNumber = int.MaxValue;
            while (iteration <= 1000)
            {
                //Знаходження значення цільової функції
                ABCAlgorithm algorithm = new ABCAlgorithm(graph, 5, 55, 30);
                int CromatickNumber = algorithm.ObjectiveFunction();
                if (CromatickNumber < bestCromatickNumber) bestCromatickNumber = CromatickNumber;

                if (iteration % 20 == 0)
                {
                    Console.WriteLine($"\n#{iteration} iteration:");
                    Console.WriteLine($"Cromatick number - {CromatickNumber}");
                    Console.WriteLine($"Best cromatick number - {bestCromatickNumber}");
                }
                iteration++;
            }
            Console.WriteLine("\n1000 iterations are done!");
            Console.WriteLine($"The best cromatick number for 300-vertices graph is {bestCromatickNumber}");
        }
    }
}