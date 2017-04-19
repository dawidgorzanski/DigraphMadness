using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DigraphMadness.Model
{
    public class DrawGraph
    {
        private Canvas _canvas;

        public Graph CurrentGraph { get; set; }
        public int Radius { get; set; }
        public int NodeRadius { get; set; }

        public DrawGraph(Canvas canvas, Graph graph)
        {
            this.CurrentGraph = graph;
            this._canvas = canvas;
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

                Label label = new Label();
                label.Height = 50;
                label.Width = 50;
                label.FontWeight = FontWeights.Bold;
                label.Content = CurrentGraph.Nodes[i].ID;
                Canvas.SetLeft(label, x - 15);
                Canvas.SetTop(label, y - 15);
                _canvas.Children.Add(label);
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
                DrawArrow(connection);
            }

            return true;
        }

        //rysowanie linii od punktu node1 do punktu node2
        private void DrawArrow(Connection connection)
        {
            Point p1 = new Point(connection.Node1.PointOnScreen.X + NodeRadius / 2, connection.Node1.PointOnScreen.Y + NodeRadius / 2);
            Point p2 = new Point(connection.Node2.PointOnScreen.X + NodeRadius / 2, connection.Node2.PointOnScreen.Y + NodeRadius / 2);

            GeometryGroup lineGroup = new GeometryGroup();
            double theta = Math.Atan2((p2.Y - p1.Y), (p2.X - p1.X)) * 180 / Math.PI;

            PathGeometry pathGeometry = new PathGeometry();
            PathFigure pathFigure = new PathFigure();
            Point p = new Point(p1.X + ((p2.X - p1.X) / 1.35), p1.Y + ((p2.Y - p1.Y) / 1.35));
            pathFigure.StartPoint = p;

            Point lpoint = new Point(p.X + 6, p.Y + 15);
            Point rpoint = new Point(p.X - 6, p.Y + 15);
            LineSegment seg1 = new LineSegment();
            seg1.Point = lpoint;
            pathFigure.Segments.Add(seg1);

            LineSegment seg2 = new LineSegment();
            seg2.Point = rpoint;
            pathFigure.Segments.Add(seg2);

            LineSegment seg3 = new LineSegment();
            seg3.Point = p;
            pathFigure.Segments.Add(seg3);

            pathGeometry.Figures.Add(pathFigure);

            RotateTransform transform = new RotateTransform();
            transform.Angle = theta + 90;
            transform.CenterX = p.X;
            transform.CenterY = p.Y;
            pathGeometry.Transform = transform;
            lineGroup.Children.Add(pathGeometry);

            LineGeometry connectorGeometry = new LineGeometry();
            connectorGeometry.StartPoint = p1;
            connectorGeometry.EndPoint = p2;
            lineGroup.Children.Add(connectorGeometry);

            Path path = new Path();
            path.Data = lineGroup;
            path.StrokeThickness = 1.0;
            path.Stroke = path.Fill = Brushes.Blue;

            //Insert() zamiast Add(), aby linie były "pod spodem" - liczy się kolejność dodawania, im dalej na liście tym "wyżej"
            _canvas.Children.Insert(0, path);

            Label label = new Label();
            label.Foreground = Brushes.Black;
            label.Content = connection.Weight;
            Canvas.SetLeft(label, p.X);
            Canvas.SetTop(label, p.Y);
            _canvas.Children.Add(label);
        }

        public void ClearAll(bool OnlyView = true)
        {
            //OnlyView - czyści tylko "rysunek"
            //żeby nie było null
            if (!OnlyView)
                CurrentGraph = GraphCreator.CreateFullGraph();

            _canvas.Children.Clear();
        }
    }
}
