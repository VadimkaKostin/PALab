using NUnit.Framework;
using Lab4;

namespace TestLab4
{
    public class Tests
    {
        int[,] AdjancyMatrix =
        {
            {0, 1, 1, 0, 0 },
            {1, 0, 0, 1, 0 },
            {1, 0, 0, 1, 0 },
            {0, 1, 1, 0, 1 },
            {0, 0, 0, 1, 0 }
        };
        [Test]
        public void TestCountVetisesOfGraph()       
        {
            Graph graph = new Graph(AdjancyMatrix);

            Assert.AreEqual(5,graph.CountVertices);
        }
        [Test]
        public void TestCountDegreeOfGraph()        
        {
            Graph graph = new Graph(AdjancyMatrix);

            Assert.AreEqual(3, graph.CountDegree(3));
        }
        [Test]
        public void TestGetAdjecentVetrticesOfGraph()
        {
            Graph graph = new Graph(AdjancyMatrix);

            var adjecentVertices = graph.GetAdjacentVertices(3);
            Assert.IsTrue(adjecentVertices[0] == 1 && adjecentVertices[1] == 2 && adjecentVertices[2] == 4);
        }
        [Test]
        public void TestScoutPhase()
        {
            Graph graph = new Graph(AdjancyMatrix);

            ABCAlgorithm algorithm = new ABCAlgorithm(graph, 3, 7, 3);

            algorithm.ScoutPfase();

            Assert.AreEqual(algorithm.Scout, algorithm.ChoosenVertices.Count);
        }
        [Test]
        public void TestOnlookerPhase()
        {
            Graph graph = new Graph(AdjancyMatrix);

            ABCAlgorithm algorithm = new ABCAlgorithm(graph, 3, 7, 3);

            algorithm.ScoutPfase();
            algorithm.OnlookerPhase();

            Assert.AreEqual(1, algorithm.EmptyVertices.Count);
        }
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