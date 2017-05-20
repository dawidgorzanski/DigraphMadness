using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DigraphMadness.GUI
{
    /// <summary>
    /// Interaction logic for DistanceTable.xaml
    /// </summary>
    public partial class DistanceTableWindow : Window
    {
        int[][] matrixOfShortestPaths;
        public DistanceTableWindow(int[][] MatrixOfShortestPaths)
        {
            InitializeComponent();
            this.matrixOfShortestPaths = MatrixOfShortestPaths;
            CreateDistanceTable();
        }

        private void CreateDistanceTable()
        {
            gDistanceTable.Children.Clear();
            gDistanceTable.ColumnDefinitions.Clear();
            gDistanceTable.RowDefinitions.Clear();

            gDistanceTable.Height = matrixOfShortestPaths.Count() * 20;
            for (int i = 0; i < matrixOfShortestPaths.Count(); i++)
            {
                gDistanceTable.ColumnDefinitions.Add(new ColumnDefinition());
                gDistanceTable.RowDefinitions.Add(new RowDefinition());

                for (int j = 0; j < matrixOfShortestPaths.Count(); j++)
                {
                    TextBlock textBlock = new TextBlock();

                    if (i == j)
                        textBlock.Foreground = Brushes.Red;
                    else
                        textBlock.Foreground = Brushes.Black;

                    textBlock.FontSize = 200 / matrixOfShortestPaths.Count();
                    textBlock.FontWeight = FontWeights.Bold;
                    textBlock.VerticalAlignment = VerticalAlignment.Center;
                    textBlock.HorizontalAlignment = HorizontalAlignment.Center;
                    textBlock.Text = matrixOfShortestPaths[i][j].ToString();
                    Border border = new Border();
                    border.BorderBrush = Brushes.Black;
                    border.BorderThickness = new Thickness(0.5);
                    border.Child = textBlock;

                    Grid.SetRow(border, i);
                    Grid.SetColumn(border, j);
                    gDistanceTable.Children.Add(border);
                }
            }
        }
    }
}
