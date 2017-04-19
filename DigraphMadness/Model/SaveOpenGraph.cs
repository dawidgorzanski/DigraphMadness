using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigraphMadness.Model
{
    public static class SaveOpenGraph
    {
        public static void SaveToFile(string FilePath, string Graph)
        {
            File.WriteAllText(FilePath, Graph);
            // return true;
        }

        public static bool ReadFromFile(string FilePath, out int[,] MatrixInt)
        {
            string text = null;
            MatrixInt = null;
            using (TextReader reader = File.OpenText(FilePath))
            {
                //Variable- zmienna używana do otrzymywania wartości z funkcji TryParse 
                int Variable;
                text = reader.ReadToEnd();

                //lines- tablica liń naszej macierzy
                string[] lines = text.Split('\n');

                //Dimenion to wymiar naszej macierzy
                int Dimension = lines.Length;

                //Sprawdzenie czy ostania linia nie jest pusta jeśli tak to zmiejszam rozmiar
                foreach (string line in lines)
                {
                    string[] numbers = line.Split(' ');

                    if (numbers.Length == 1)
                        Dimension--;
                }

                //Counter liczy abyśmy nie przeszli przypadkiem do pustej lini i tam coś robili
                int Counter = 0;
                //Sprawdzenie czy macierz jest odpowiedniego wymiaru

                foreach (string line in lines)
                {

                    string[] numbers = line.Split(' ');

                    if (Dimension == numbers.Length - 1)
                    {
                        if (Int32.TryParse(numbers[numbers.Length - 1], out Variable))
                        {
                            return false;
                        }
                    }
                    else if (Dimension == numbers.Length)
                    {
                        if (Int32.TryParse(numbers[numbers.Length - 1], out Variable) == false)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }

                    Counter++;
                    if (Counter == Dimension)
                        break;
                }
                //Inicjalizacja MatrixInt

                MatrixInt = new int[Dimension, Dimension];

                //Sprawdzenie czy w macierzy są tylko 0 i 1
                for (int i = 0; i < Dimension; i++)
                {
                    string[] numbers = lines[i].Split(' ');

                    for (int j = 0; j < Dimension; j++)
                    {
                        if (int.TryParse(numbers[j], out Variable) != true)
                        {
                            return false;
                        }
                        if (Variable != 1 && Variable != 0)
                        {
                            return false;
                        }
                        MatrixInt[i, j] = Variable;
                    }
                }
                //sprawdzanie czy macierz jest symetryczna
                for (int i = 0; i < Dimension; i++)
                {
                    for (int j = 0; j < Dimension; j++)
                    {
                        if (MatrixInt[i, j] != MatrixInt[j, i])
                        {
                            return false;
                        }
                    }
                }
                //sprawdzanie czy diagonalna ma same jedynki
                for (int i = 0; i < Dimension; i++)
                {
                    if (MatrixInt[i, i] == 1)
                        return false;
                }

            }
            return true;
        }
    }
}
