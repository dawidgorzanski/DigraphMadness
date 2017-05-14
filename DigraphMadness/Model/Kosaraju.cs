using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigraphMadness.Model
    {
        class Kosaraju
        {
            //w tej tablicy będę przechowywał potrzebne indexy dla algorytmu kosaraju
            static int[] staticArray;
            static int staticIndex;

            static List<List<int>> ListOfStack;
            static int indexOfStack;
            static bool[] isVisited;

        public static string KosarajuAlgorithm(Graph graph)
        {
            //będę tworzył matrix na postawie grafu ale narazie otrzymuje macierz
             int[,] firstMatrix = graph.ToMatrix();
            //temp jest to kopia mojej tablicy
            int[,] transponeMatrix = TransponeMatrix(firstMatrix);
            int size = firstMatrix.GetLength(0);
            staticArray = new int[size];
            staticIndex = 0;
            isVisited = new bool[size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (firstMatrix[i, j] == 1 && !isVisited[i])
                    {
                        firstMatrix = SetTimeOfConvert(firstMatrix, i);
                        //break;
                    }
                }
            }
            //teraz zaczynam odpowiednie tworzenie mojego stosu silnych spójnych
            //ListOdStack - mój stos
            ListOfStack = new List<List<int>>();
            indexOfStack = 0;
            isVisited = new bool[size];
            for (int i = size - 1; i >= 0; i--)
            {
                if (!isVisited[staticArray[i]])
                {
                    ListOfStack.Add(new List<int>());
                    ListOfStack[indexOfStack].Add(staticArray[i]);
                    isVisited[staticArray[i]] = true;
                    transponeMatrix = CreateStack(transponeMatrix, staticArray[i]);
                    indexOfStack++;

                }
            }
            //składam mój stos w listę
            int[] arrayToCheck = new int[size];
            string finalString = "";
            foreach (List<int> l in ListOfStack)
            {
                foreach (int number in l)
                {
                    finalString = finalString + " " + number;
                    arrayToCheck[number] = 1;
                }
                finalString = finalString + Environment.NewLine;
            }
            //jeżeli jakiś wierzchołemk nie miał połączeń to go dopisuje na końcu
            for(int i=0;i<size;i++)
            {
                if(arrayToCheck[i]==0)
                    finalString = finalString + i+ Environment.NewLine;
            }
            
            return finalString;
        }
        private static int[,] SetTimeOfConvert(int[,] firstMatrix, int index)
        {

            int size = firstMatrix.GetLength(0);
            for (int j = 0; j < size; j++)
            {
                if (firstMatrix[index, j] == 1 && !isVisited[j])
                {
                    firstMatrix[index, j] = 0;
                    isVisited[index] = true;
                    firstMatrix = SetTimeOfConvert(firstMatrix, j);
                }
            }
            staticArray[staticIndex] = index;
            staticIndex++;
            isVisited[index] = true;
            return firstMatrix;
        }
        private static int[,] CreateStack(int[,] transponeMatrix, int index)
        {
            int size = transponeMatrix.GetLength(0);
            for (int j = 0; j < size; j++)
            {
                if (transponeMatrix[index, j] == 1 && !isVisited[j])
                {
                    transponeMatrix[index, j] = 0;
                    ListOfStack[indexOfStack].Add(j);
                    isVisited[j] = true;
                    transponeMatrix = CreateStack(transponeMatrix, j);
                }
            }
            return transponeMatrix;
        }
        private static int[,] TransponeMatrix(int[,] firstMatrix)
            {
                int size = firstMatrix.GetLength(0);
                int[,] finalMatrix = new int[size, size];
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                        finalMatrix[i, j] = firstMatrix[j, i];
                }
                return finalMatrix;
            }
        }
}

