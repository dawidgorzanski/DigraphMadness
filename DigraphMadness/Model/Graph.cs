using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigraphMadness.Model
{
    public class Graph
    {
        private List<Node> _nodes;
        private List<Connection> _connections;

        public List<Node> Nodes
        {
            get
            {
                return _nodes;
            }
        }
        public List<Connection> Connections
        {
            get
            {
                return _connections;
            }
        }

        public Graph()
        {
            InitializeLists();
        }

        //Inicjalizuje listy _nodes oraz _connections
        private void InitializeLists()
        {
            _connections = new List<Connection>();
            _nodes = new List<Node>();
        }

        public static Graph MakeGraphCopy(Graph OriginalGraph)
        {
            Graph myGraphCopy = new Graph();
            for (int i = 0; i < OriginalGraph.Nodes.Count; i++) //Kopiuje wierzcholki grafu do jego kopii
            {
                myGraphCopy.AddNode(OriginalGraph.Nodes[i]);
            }

            for (int i = 0; i < OriginalGraph.Connections.Count; i++) //Kopiuje krawedzie grafu do jego kopii
            {
                Connection copiedConnection = new Connection();
                copiedConnection.Node1 = OriginalGraph.Connections[i].Node1;
                copiedConnection.Node2 = OriginalGraph.Connections[i].Node2;
                copiedConnection.Weight = OriginalGraph.Connections[i].Weight;
                myGraphCopy.AddConnection(copiedConnection);
            }

            return myGraphCopy;
        }

        //statyczna metoda obliczająca ile może być maksymalnie połączeń
        public static int MaxConnections(int Nodes)
        {
            return Nodes * (Nodes - 1) / 2;
        }

        public void AddNode(Node node)
        {
            if (node != null)
                _nodes.Add(node);
        }

        public void AddConnection(Connection connection)
        {
            _connections.Add(connection);
        }

        public string ToMatrixString()
        {
            int Dimension = _nodes.Count;
            string finalString = null;
            int[,] Matrix = Matrix = new int[Dimension, Dimension];
            for (int i = 0; i < _connections.Count; i++)
            {
                Matrix[_connections[i].Node1.ID, _connections[i].Node2.ID] = 1;
                //Matrix[_connections[i].Node2.ID, _connections[i].Node1.ID] = 1;
            }
            for (int i = 0; i < Dimension; i++)
            {
                for (int j = 0; j < Dimension; j++)
                {
                    finalString = finalString + Matrix[i, j].ToString() + " ";
                }
                finalString = finalString + Environment.NewLine;
            }
            return finalString;
        }

        public string ToListString()
        {
            int Dimension = _nodes.Count;
            string finalString = null;
            List<List<int>> Data = new List<List<int>>();
            for (int i = 0; i < Dimension; i++)
            {
                Data.Add(new List<int>());
            }
            foreach (Connection con in _connections)
            {
                Data[con.Node1.ID].Add(con.Node2.ID + 1);
                Data[con.Node2.ID].Add(con.Node1.ID + 1);
            }
            int Counter = 1;
            foreach (List<int> lista in Data)
            {
                finalString = finalString + Counter.ToString() + ": ";
                foreach (int Number in lista)
                {
                    finalString = finalString + Number.ToString() + "->";
                }
                finalString = finalString + Environment.NewLine;
                Counter++;
            }
            return finalString;
        }

        public string ToIncidenceMatrixString()
        {
            int Dimension = _nodes.Count;
            string finalString = null;
            for (int i = 0; i < Dimension; i++)
            {
                foreach (Connection con in _connections)
                {
                    if (con.Node1.ID == i || con.Node2.ID == i)
                    {
                        finalString = finalString + "1 ";
                    }
                    else
                    {
                        finalString = finalString + "0 ";
                    }
                }
                finalString = finalString + Environment.NewLine;
            }

            return finalString;
        }
        public int[,] ToMatrix()
        {
            int Dimension = _nodes.Count;
            int[,] Matrix = new int[Dimension, Dimension];
            for (int i = 0; i < _connections.Count; i++)
            {
                Matrix[_connections[i].Node1.ID, _connections[i].Node2.ID] = 1;
                //Matrix[_connections[i].Node2.ID, _connections[i].Node1.ID] = 1;
            }
            return Matrix;
        }

    }
}
