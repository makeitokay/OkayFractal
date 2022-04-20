using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace OkayFractal
{
    /// <summary>
    /// Класс, отрисовывающий фрактал "Треугольник Серпинского".
    /// </summary>
    public class SerpinskyTriangleDrawer : FractalDrawer
    {
        public override int MaxDepth => 9;

        private List<Point> _startTriangle;
        private double _triangleMargin;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="depth"></param>
        public SerpinskyTriangleDrawer(Canvas canvas, int depth) : base(canvas, depth) {}

        /// <summary>
        /// Установка холста для отрисовки фрактала.
        /// </summary>
        public override Canvas Canvas
        {
            set
            {
                _canvas = value;
                _triangleMargin = _canvas.ActualWidth / 5;
                _startTriangle = new List<Point>
                {
                    new() { X = _triangleMargin, Y = _canvas.ActualHeight - 15 },
                    new()
                    {
                        X = _triangleMargin + (_canvas.ActualWidth - _triangleMargin * 2) / 2.0, 
                        Y = _canvas.ActualHeight - 15 - Math.Sqrt(0.75 * Math.Pow(_canvas.ActualWidth - _triangleMargin * 2, 2))
                    },
                    new() { X = _canvas.ActualWidth - _triangleMargin, Y = _canvas.ActualHeight - 15 }
                };
            }
        }

        /// <summary>
        /// Метод, вычислящий точку - середину отрезка двух точек.
        /// </summary>
        /// <param name="firstPoint">Первая точка.</param>
        /// <param name="secondPoint">Вторая точка.</param>
        /// <returns>Точка - середина отрезка из двух точек.</returns>
        private static Point GetMiddlePoint(Point firstPoint, Point secondPoint) => new() 
            {X = (firstPoint.X + secondPoint.X) / 2, Y = (firstPoint.Y + secondPoint.Y) / 2};

        /// <summary>
        /// Метод, отрисовывающий фрактал.
        /// </summary>
        public override void DrawFractal() => DrawFractalAt(_startTriangle, 0);

        /// <summary>
        /// Рекурсивный метод, отрисовывающий фрактал в соответствии с глубиной.
        /// </summary>
        /// <param name="currentTriangle">Коллекция точек текущего треугольника.</param>
        /// <param name="currentLevel">Текущий уровень рекурсии.</param>
        private void DrawFractalAt(List<Point> currentTriangle, int currentLevel)
        {
            _canvas.Children.Add(new Polygon { Points = new PointCollection(currentTriangle), Stroke = Brushes.Black});
            if (currentLevel == Depth - 1)
                return;
            currentLevel++;
            var firstMiddle = GetMiddlePoint(currentTriangle[0], currentTriangle[1]);
            var secondMiddle = GetMiddlePoint(currentTriangle[0], currentTriangle[2]);
            var thirdMiddle = GetMiddlePoint(currentTriangle[1], currentTriangle[2]);
            DrawFractalAt(new List<Point> { currentTriangle[0], firstMiddle, secondMiddle }, currentLevel);
            DrawFractalAt(new List<Point> { currentTriangle[1], firstMiddle, thirdMiddle }, currentLevel);
            DrawFractalAt(new List<Point> { currentTriangle[2], secondMiddle, thirdMiddle }, currentLevel);
        }
    }
}