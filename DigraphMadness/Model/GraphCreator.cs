﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigraphMadness.Model
{
    public static class GraphCreator
    {

        public static Graph CreateFromMatrix(int[,] MatrixInt)
        {
            int Dimension = MatrixInt.GetLength(0);
            Graph fromMatrix = new Graph();

            for (int i = 0; i < Dimension; i++)
                fromMatrix.Nodes.Add(new Node() { ID = i });
            for (int i = 0; i < Dimension; i++)
            {
                for (int j = i + 1; j < Dimension; j++)
                {
                    if (MatrixInt[i, j] == 1)
                    {
                        fromMatrix.Connections.Add(new Connection { Node1 = fromMatrix.Nodes[i], Node2 = fromMatrix.Nodes[j] });
                    }
                }
            }
            return fromMatrix;
        }

        public static Graph CreateRandomGraphProbability(int Nodes, double Probability) // G(n, p)
        {
            Graph randomGraph = new Graph();
            for (int i = 0; i < Nodes; i++)
                randomGraph.Nodes.Add(new Node() { ID = i });
            Random rnd = new Random();
            double currentProbability;
            for (int i = 0; i < Nodes; i++)
            {
                for (int j = i + 1; j < Nodes; j++)
                {
                    currentProbability = rnd.NextDouble();
                    if (currentProbability < Probability)
                    {
                        Connection connection = new Connection();
                        connection.Node1 = randomGraph.Nodes.FirstOrDefault(x => x.ID == i);
                        connection.Node2 = randomGraph.Nodes.FirstOrDefault(x => x.ID == j);
                        randomGraph.Connections.Add(connection);
                    }
                }
            }
            return randomGraph;
        }

        //Zerknijcie na tworzenie połączeń :D
        public static Graph CreateFullGraph(int Nodes = 0)
        {
            Graph fullGraph = new Graph();

            //Dodanie wierzchołków
            for (int i = 0; i < Nodes; i++)
                fullGraph.Nodes.Add(new Node() { ID = i });

            //Dodanie połączeń między wierzchołkami
            for (int i = 0; i < Nodes; i++)
            {
                for (int j = i + 1; j < Nodes; j++)
                {
                    Connection connection = new Connection();
                    //Nie tworzymy nowych obiektów typu Node, tylko wyszukujemy w Nodes już istniejące - znacznie ułatwia
                    //to rysowanie grafu - każdy obiekt Node dostaje później swoje współrzędne, ktore w obu listach są takie same.
                    //Jeżeli utworzylibyśmy nowy obiekt to po dodaniu współrzędnych obiektom w Nodes, w Connection nie
                    //zostałyby one zmienione.
                    //(x => x.ID == i) jest to tzw. wyrażenie Lambda - w tym wypadku szukamy pierwszego elementu o ID równym i.
                    connection.Node1 = fullGraph.Nodes.FirstOrDefault(x => x.ID == i);
                    connection.Node2 = fullGraph.Nodes.FirstOrDefault(x => x.ID == j);
                    fullGraph.Connections.Add(connection);
                }
            }

            return fullGraph;
        }
    }
}
