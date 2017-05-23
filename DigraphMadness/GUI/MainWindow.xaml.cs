using DigraphMadness.GUI;
using DigraphMadness.Model;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DigraphMadness
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DrawGraph draw;
        public MainWindow()
        {
            InitializeComponent();
            InitializeColorPickers();
            draw = new DrawGraph(mainCanvas, GraphCreator.CreateFullGraph());
            draw.NodeClicked += Draw_NodeClicked;
        }

        private void Draw_NodeClicked(object sender, EventArgs e)
        {
            Node clickedNode = sender as Node;
            MessageBox.Show(BellmanFord.Algorithm(clickedNode.ID, draw.CurrentGraph), "Odległości dla wierzchołka " + clickedNode.ID);
        }

        private void InitializeColorPickers()
        {
            colorPickerCircle.SelectedColor = Colors.Green;
            colorPickerPoints.SelectedColor = Colors.Red;
        }

        private void btnDrawRandomGraphFromProbability_Click(object sender, RoutedEventArgs e)
        {
            draw.ClearAll();

            if (intUpDownRandomPoints.Value != null && doubleUpDownProbability.Value != null)
                draw.CurrentGraph = GraphCreator.CreateRandomGraphProbability((int)intUpDownRandomPoints.Value, (double)doubleUpDownProbability.Value);
            else
            {
                MessageBox.Show("Niepoprawna ilość wierchołków!", "Błąd!");
                return;
            }

            draw.NodeRadius = (int)sliderNodeRadius.Value;
            draw.Radius = (int)sliderRadius.Value;

            draw.DrawMainCircle();
            draw.Draw();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            draw.ClearAll();
        }

        private void colorPickerPoints_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            Resources["ColorPoints"] = new SolidColorBrush((Color)colorPickerPoints.SelectedColor);
        }

        private void colorPickerCircle_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            Resources["ColorCircle"] = new SolidColorBrush((Color)colorPickerCircle.SelectedColor);
        }

        private void btnKosaraju_Click(object sender, RoutedEventArgs e)
        { 
            int size = draw.CurrentGraph.Nodes.Count;
            int[] arrayToCheck = new int[size];
            string finalString = "";
            List<List<int>> ListOfStack = Kosaraju.KosarajuAlgorithm(draw.CurrentGraph);
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
            for (int i = 0; i < size; i++)
            {
                if (arrayToCheck[i] == 0)
                    finalString = finalString + " " + i + Environment.NewLine;
            }
            MessageBox.Show(finalString, "Silne składowe");
        }

        private void btnJohnson_Click(object sender, RoutedEventArgs e)
        {
            List<List<int>> ListOfStack = Kosaraju.KosarajuAlgorithm(draw.CurrentGraph);
            if (ListOfStack[0].Count!=draw.CurrentGraph.Nodes.Count)
            {
                MessageBox.Show("Uzyty graf musi byc silnie spojny!", "Błąd!");
                return;
            }

            if (draw.CurrentGraph.Nodes.Count != 0)
            {
                Graph CurrentGraphCopy = Graph.MakeGraphCopy(draw.CurrentGraph);
                if (Johnson.Johnson_Algorithm(ref CurrentGraphCopy))
                {
                    DistanceTableWindow distanceTableWindow = new DistanceTableWindow(Johnson.MatrixOfShortestsPaths(CurrentGraphCopy));  
                    distanceTableWindow.Show();
                }
                else
                {
                    MessageBox.Show("Na tym grafie istnieje ujemny cykl!", "Błąd!");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Probujesz wywolac algorytm bez grafu!", "Błąd!");
                return;
            }
        }
    }
}
