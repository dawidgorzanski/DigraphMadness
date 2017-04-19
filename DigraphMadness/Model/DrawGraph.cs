using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace DigraphMadness.Model
{
    public class DrawGraph
    {
        private List<Line> _lines;
        private Canvas _canvas;

        public List<Line> Lines
        {
            get
            {
                return _lines;
            }
        }
        public Graph CurrentGraph { get; set; }
        public int Radius { get; set; }
        public int NodeRadius { get; set; }

        public DrawGraph(Canvas canvas, Graph graph)
        {
            this.CurrentGraph = graph;
            this._canvas = canvas;
            InitializeLists();
        }

        //inicjalizacja _nodesPoints oraz _lines
        private void InitializeLists()
        {
            _lines = new List<Line>();
        }

        //rysowanie głównego koła
        public void DrawMainCircle()
        {
            Ellipse mainEllipse = new Ellipse();
            mainEllipse.SetResourceReference(Ellipse.StrokeProperty, "ColorCircle");
            mainEllipse.StrokeThickness = 1;
            mainEllipse.Height = mainEllipse.Width = 2 * Radius;

            //Ustawiane w ten sposób, gdyz punkt (0,0) elementu to lewy górny róg, a nie jego środek
            Canvas.SetLeft(mainEllipse, (_canvas.ActualWidth / 2) - Radius + (NodeRadius / 2));
            Canvas.SetTop(mainEllipse, (_canvas.ActualHeight / 2) - Radius + (NodeRadius / 2));

            _canvas.Children.Insert(0, mainEllipse);
        }

        //rysowanie punktów
        private void DrawNodes()
        {
            double a = _canvas.ActualWidth / 2;
            double b = _canvas.ActualHeight / 2;

            for (int i = 0; i < CurrentGraph.Nodes.Count; i++)
            {
                double t = 2 * Math.PI * i / CurrentGraph.Nodes.Count;
                int x = (int)Math.Round(a + Radius * Math.Cos(t));
                int y = (int)Math.Round(b + Radius * Math.Sin(t));

                CurrentGraph.Nodes[i].PointOnScreen = new Point(x, y);

                Ellipse ellipse = new Ellipse();
                ellipse.SetResourceReference(Ellipse.StrokeProperty, "ColorPoints");
                ellipse.SetResourceReference(Ellipse.FillProperty, "ColorPoints");
                ellipse.Height = NodeRadius;
                ellipse.Width = NodeRadius;
                Canvas.SetLeft(ellipse, x);
                Canvas.SetTop(ellipse, y);
                _canvas.Children.Add(ellipse);
            }
        }

        //rysowanie pełnego grafu
        public bool Draw()
        {
            if (CurrentGraph.Nodes.Count == 0)
                return false;

            //Rysowanie punktów
            DrawNodes();

            //Rysowanie linii
            foreach (Connection connection in CurrentGraph.Connections)
            {
                DrawLine(connection.Node1.PointOnScreen, connection.Node2.PointOnScreen);
            }

            return true;
        }

        //rysowanie linii od punktu node1 do punktu node2
        private void DrawLine(Point node1, Point node2)
        {
            Line line = new Line();
            line.StrokeThickness = 1;
            line.SetResourceReference(Line.StrokeProperty, "ColorLines");
            line.X1 = node1.X + NodeRadius / 2;
            line.X2 = node2.X + NodeRadius / 2;
            line.Y1 = node1.Y + NodeRadius / 2;
            line.Y2 = node2.Y + NodeRadius / 2;
            //Insert() zamiast Add(), aby linie były "pod spodem" - liczy się kolejność dodawania, im dalej na liście tym "wyżej"
            _canvas.Children.Insert(0, line);
        }

        public void ClearAll()
        {
            //żeby nie było null
            CurrentGraph = GraphCreator.CreateFullGraph();
            _lines.Clear();
            _canvas.Children.Clear();
        }
    }
}
