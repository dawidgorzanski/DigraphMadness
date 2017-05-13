using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace DigraphMadness.Model
{
    class BellmanFord
    {
        public static string Algorithm(int source, Graph graph)
        {
            List<int> nodesPredecessors = new List<int>();
            List<int> d = new List<int>();

            var isNoCycle = BellmanFordAlgorithm(graph, source, nodesPredecessors, d);

            if (!isNoCycle)
            {
                string str = "W grafie wystepuje ujemny cykl";
                return str;
            }
            string finalString = "Koszt dojscia do kazdego z wierzcholkow:";

            for (int i = 0; i < graph.Nodes.Count; i++)
            {
                if (i == source)
                    continue;
                if (d[i] > Int32.MaxValue-100000)
                    finalString += "\n" + i + ": nieosiagalny";
                else
                {
                    finalString += "\n" + i + ": " + d[i];
                    int temp = nodesPredecessors[i];
                    if (temp == -1)
                        continue;
                    List<int> tempList = new List<int>();
                    while (true)
                    {
                        tempList.Insert(0, temp);
                        if (temp == source)
                            break;
                        temp = nodesPredecessors[temp];
                    }
                    tempList.Add(i);
                    finalString += "\n   " + String.Join("->", tempList);

                }
            }

            return finalString;
        }

        private static bool BellmanFordAlgorithm(Graph graph, int source, List<int> nodesPredecessors, List<int> d)
        {
            for (int i = 0; i < graph.Nodes.Count; i++)
            {
                d.Add(Int32.MaxValue-10000);
                nodesPredecessors.Add(-1);
            }
            d[source] = 0;
            for (int i = 0; i < graph.Nodes.Count - 1; i++)
            {
                bool test = true;
                foreach (var con in graph.Connections)
                {
                    if (d[con.Node1.ID] + con.Weight < d[con.Node2.ID])
                    {
                        d[con.Node2.ID] = d[con.Node1.ID] + con.Weight;
                        test = false;
                        nodesPredecessors[con.Node2.ID] = con.Node1.ID;
                    }
                }
                if (test)
                    return true;
            }
           
            for (int i = 0; i < graph.Nodes.Count; i++) //Check if there is negative cycle
            {
                List<Connection> find = graph.Connections.FindAll(x => x.Node1.ID == i || x.Node2.ID == i);
                foreach (var con in find)
                {
                    var neighbour = (con.Node1.ID == i) ? con.Node2.ID : con.Node1.ID;
                    if (d[neighbour] > d[i] + con.Weight)
                    {
                        return false;
                    }

                }
            }

            return true;
        }
    }
}
