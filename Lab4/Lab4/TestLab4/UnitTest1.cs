using NUnit.Framework;
using Lab4;

namespace TestLab4
{
    public class Tests
    {
        //Для тестів будемо використовувати один і той самий граф з відповідною матрицею суміжності
        int[,] AdjancyMatrix =
        {
            {0, 1, 1, 0, 0 },
            {1, 0, 0, 1, 0 },
            {1, 0, 0, 1, 0 },
            {0, 1, 1, 0, 1 },
            {0, 0, 0, 1, 0 }
        };

        //Тестування підрахунку вершин графу
        [Test]
        public void TestCountVetisesOfGraph()       
        {
            Graph graph = new Graph(AdjancyMatrix);

            Assert.AreEqual(5,graph.CountVertices);
        }

        //Тестування підрахунку степеню вершини
        [Test]
        public void TestCountDegreeOfGraph()        
        {
            Graph graph = new Graph(AdjancyMatrix);

            Assert.AreEqual(3, graph.CountDegree(3));
        }

        //Тестування методу отримання суміжних вершин певної вершини
        [Test]
        public void TestGetAdjecentVetrticesOfGraph()
        {
            Graph graph = new Graph(AdjancyMatrix);

            var adjecentVertices = graph.GetAdjacentVertices(3);
            Assert.IsTrue(adjecentVertices[0] == 1 && adjecentVertices[1] == 2 && adjecentVertices[2] == 4);
        }

        //Тестування фази розвідників. Як результат цієї фази кількість розвіданих джерел(вершин графу) в колекції ChoosenVertices
        //має дорівнювати кількості самих розвідників
        [Test]
        public void TestScoutPhase()
        {
            Graph graph = new Graph(AdjancyMatrix);

            ABCAlgorithm algorithm = new ABCAlgorithm(graph, 3, 7, 3);

            algorithm.ScoutPfase();

            Assert.AreEqual(algorithm.Scout, algorithm.ChoosenVertices.Count);
        }

        //Тестування фазу фуражирів. Результатом фази фуражирів після першої ітерації буде
        //наявність в колекції EmptyVetices тільки однієї вершини,
        //тобто тільки одне джерело нектару буде пустим
        [Test]
        public void TestOnlookerPhase()
        {
            Graph graph = new Graph(AdjancyMatrix);

            ABCAlgorithm algorithm = new ABCAlgorithm(graph, 3, 7, 3);

            algorithm.ScoutPfase();
            algorithm.OnlookerPhase();

            Assert.AreEqual(1, algorithm.EmptyVertices.Count);
        }

        //Тетсування перевірки на доступність кольору для певної вершини. Розфарбовуємо вершини 1, 2, 4,
        //потім перевіряємо на можливість розфарбувати вершину 3 у color1,
        //у який вже точно буде пофарбована одна із суміжних вершин
        [Test]
        public void TestColorVertice()
        {
            Graph graph = new Graph(AdjancyMatrix);

            ABCAlgorithm algorithm = new ABCAlgorithm(graph, 3, 7, 3);

            algorithm.ColorVertice(1);
            algorithm.ColorVertice(2);
            algorithm.ColorVertice(4);

            Assert.IsTrue(!algorithm.CheckColorWithAdjecent(3,"color1"));
        }

        //Тестування цільової функції. В результаті виклику цільової функції та виконання алгоритму для підрахунку її значення
        //хроматичне число usedColors буде меншим за кількість усіх кольорів в алгоритмі
        [Test]
        public void TestObjectiveFunction()
        {
            Graph graph = new Graph(AdjancyMatrix);

            ABCAlgorithm algorithm = new ABCAlgorithm(graph, 3, 7, 3);
            int usedColors = algorithm.ObjectiveFunction();

            Assert.IsTrue(usedColors <= algorithm.AllColors.Count);
        }
    }
}