using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigraphMadness.Model
{
    class Johnson
    {
        public static bool Johnson_Algorithm(ref Graph myGraph) 
        {
            Node q = new Node();
            q.ID = myGraph.Nodes.Count;
            myGraph.AddNode(q); //Dodaje nowy wierzcholek q, ktory bedzie polaczony z wszystkimi innymi wierzcholkami grafu krawedziami o wadze 0
            for( int i = 0; i < myGraph.Nodes.Count - 1; i++) //Dodaje krawedzie wychodzace od q do wszystkich pozostalych wierzcholkow. Nadaje im wagi 0
            {
                Connection newConnection = new Connection();
                newConnection.Node1 = myGraph.Nodes[myGraph.Nodes.Count-1];
                newConnection.Node2 = myGraph.Nodes[i];
                newConnection.Weight = 0;
                myGraph.AddConnection(newConnection);
            }

            /* Wykorzystam teraz algorytm Bellmana-Forda do sprawdzenia, czy graf nie zawiera ujemnych sciezek, oraz do wyliczenia wspolczynnikow do przekalkulowania od nowa wag krawedzi, celem przekierowania do algorytmu Dijkstry. */
            List<int> nodesPredecessors = new List<int>(); //Lista z poprzednikami do celu (czyli przedostatnie wierzcholki zanim dostalismy sie do docelowych)
            List<int> d = new List<int>(); //Lista z dlugoscia najkrotszych sciezek do poszczegolnch wierzcholkow od wierzcholka q
            var isNoCycle = BellmanFord.BellmanFordAlgorithm(myGraph, myGraph.Nodes.Count-1, nodesPredecessors, d);

            if (!isNoCycle) //Sprawdzam, czy istnieje ujemny cykl
            {
                return false; //Okazalo sie, ze w algorytm Bellmana-Forda wykryl ujemny cykl. Algorytm wiec nie ma sensu i zwroce 0
            }

            /* Usuwam teraz dodany przedtem wierzcholek q, oraz nowo dodane krawedzie, ktore go zawieraja */
            int qID = myGraph.Connections.Count - 1;

            //Console.WriteLine("qID wynosi: " + qID);

            
             //myGraph.Connections.RemoveAll(x => (x.Node1.ID == qID));
            
            for (int i = 0; i < myGraph.Connections.Count; i++)
            {
                if (myGraph.Connections[i].Node1.ID == (myGraph.Nodes.Count - 1))
                {
                    myGraph.Connections.RemoveAt(i); //Usuwam krawedzie zawierajace wierzcholek q
                    i--;
                }
            }
            myGraph.Nodes.RemoveAt(myGraph.Nodes.Count-1); //Na koncu usuwam wierzcholek q

            /* myGraph wrocil do poczatkowej postaci myGraph, ale wiemy juz, ze nie ma ujemnego cyklu oraz mamy wspolczynniki do przekalkulowania wag krawedzi */
            for(int i = 0; i < myGraph.Connections.Count; i++)
            {
                int connection_start_node = myGraph.Connections[i].Node1.ID;
                int connection_end_node = myGraph.Connections[i].Node2.ID;
                myGraph.Connections[i].Weight = myGraph.Connections[i].Weight + d[connection_start_node] - d[connection_end_node];
            }

            /*
            for (int i = 0; i < myGraph.Connections.Count; i++)
            {
                Console.WriteLine(i + " " + myGraph.Connections[i].Node1.ID + "->" + myGraph.Connections[i].Node2.ID + ": " + myGraph.Connections[i].Weight);
            }
            */

            return true;
        }

        public static string ShortestPaths(Graph graph, Node selectedNode, out int[] d)
        {
            List<int> Q = new List<int>();  //zbior dla ktorego dla ktorego najkrotsze sciezki nie zostaly jeszcze policzone

            d = new int[graph.Nodes.Count];  // n elementowa tablica z kosztami dojscia od wierzcholka selectedNode do wierzcholka i-tego wzdluz najkrotszej sciezki
            int[] p = new int[graph.Nodes.Count]; //n elementowa talibca z poprzednikami wierzcoholkow
            for (int i = 0; i < graph.Nodes.Count; ++i)
            {
                Q.Add(i);
                d[i] = Int32.MaxValue;
                p[i] = -1;  // -1 oznacza brak poprzednika
            }

            d[selectedNode.ID] = 0;  // koszt dojścia do samego siebie jest zawsze zerowy

            while (Q.Count > 0)
            {
                //Szukam najmniejszej wartości w tablicy d
                int minValue = Int32.MaxValue;
                int minIndex = Int32.MaxValue;

                for (int i = 0; i < d.Count(); ++i) {
                    //if (minValue > d[i] && Q.Count(x => x == i) == 1)
                    if( Q.Contains(i)  && minValue > d[i] )
                    {
                        //Console.WriteLine("Minimalny indeks to wierzcholek o id: " + i);
                        minIndex = i;
                        minValue = d[i];
                    }
                }

                //Console.WriteLine("Moje Q zawiera : " + Q.Count + ", a min index to : " + minIndex);

                //Usuwam wierzcholek o najmniejszym d z Q
                Q.Remove(minIndex);

                //znalezienie sąsiadów
                List<Node> neighbours = new List<Node>();
                for (int j = 0; j < graph.Connections.Count; j++)
                {
                    if (graph.Connections[j].Node1.ID == minIndex)
                    {
                        neighbours.Add(graph.Connections[j].Node2);
                    }
                    /*
                    else if (graph.Connections[j].Node2.ID == minIndex)
                    {
                        neighbours.Add(graph.Connections[j].Node1);       wyrzucam, bo graf skierowany, wiec sasiedzi tylko w Node2;
                    }
                    */
                }

                //Console.WriteLine("Jestem tutaj!");

                for (int j = 0; j < neighbours.Count; j++)
                {
                    //sprawdzenie czy wierzcholek jest w Q
                    if (Q.Count(x => x == neighbours[j].ID) == 0)
                        continue;

                    //obliczenie wagi krawedzi
                    Connection searchedConnection = graph.Connections.Single(x => (x.Node1.ID == minIndex && x.Node2.ID == neighbours[j].ID));
                    int weight = searchedConnection.Weight;
                    // || (x.Node1.ID == neighbours[j].ID && x.Node2.ID == minIndex)).Weight;   wyrzucam, bo graf skierowany 

                    if (d[neighbours[j].ID] > d[minIndex] + weight)
                    {
                        d[neighbours[j].ID] = d[minIndex] + weight;
                        p[neighbours[j].ID] = minIndex;
                    }
                }
            }

            string result = "";
            for (int i = 0; i < d.Count(); ++i)
            {
                if (i == selectedNode.ID)
                    continue;

                result += "Odległość do " + i + ": " + d[i] + ". Trasa: ";

                int previous = p[i];
                List<int> trace = new List<int>();
                while (previous != -1)
                {
                    trace.Add(previous);
                    previous = p[previous];
                }
                trace.Reverse();

                foreach (int item in trace)
                    result += item + "->";

                result += Environment.NewLine;
            }

            return result;
        }
        public static int[][] MatrixOfShortestsPaths(Graph graph)
        {
            int[] temp;
            int[][] final = new int[graph.Nodes.Count][];
            foreach (Node node in graph.Nodes)
            {
                temp = new int[graph.Nodes.Count];
                ShortestPaths(graph, node, out temp);
                final[node.ID] = temp;
            }
            return final;
        }

        public static bool isEveryNodeConnected(Graph myGraphToCheck)  //sprawdzam, czy graf jest spojny
        {
            // rak mózgu, bladzenie losowe zrobilem (dziala i to dosc szybko o dziwo, nawet dla +20 wierzcholkow z prawdopodobienstwem polaczen >0.8 xD ), można zrobić inaczej, sprawdzając, czy największa spójna składowa zawiera wszystkie wierzchołki
            List<List<int>> neighbours = new List<List<int>>();
            for (int i = 0; i < myGraphToCheck.Nodes.Count; ++i)
            {
                List<int> nodeNeighbours = new List<int>();
                for (int j = 0; j < myGraphToCheck.Connections.Count; j++)
                {
                    if (myGraphToCheck.Connections[j].Node1.ID == i)
                    {
                        nodeNeighbours.Add(myGraphToCheck.Connections[j].Node2.ID);
                    }
                }
                if (nodeNeighbours.Count == 0)
                    return false;
                else
                    neighbours.Add(nodeNeighbours);
            }
        
            
            //for(int i = 0; i < neighbours.Count; ++i)
            //{
                //Console.WriteLine("Wiersz " + i + ": ");
                //for (int j = 0; j < neighbours[i].Count; ++j)
                //{
                    //Console.WriteLine(neighbours[i][j] + " ");
                //}
            //}
            


            List<int> visited = new List<int>();
            Random rnd = new Random();
            int which;
            int actual = 0;
            int howMany = myGraphToCheck.Connections.Count * 200;
            for (int i = 0; i < howMany; ++i)
            {
                //Console.WriteLine("Przegladany wierzcholek ma: " + neighbours[actual].Count + " sasiadow.");
                which = rnd.Next(0, neighbours[actual].Count);
                actual = neighbours[actual][which];
                //Console.WriteLine("Wchodze do: " + actual);
                if (!visited.Contains(actual))
                    visited.Add(actual);
            }

            if (visited.Count == myGraphToCheck.Nodes.Count)
                return true;
            else
                return false;

        }
    
    }
}
