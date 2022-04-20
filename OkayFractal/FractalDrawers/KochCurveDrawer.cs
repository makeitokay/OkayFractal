using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace OkayFractal
{
    /// <summary>
    /// Класс, отрисовывающий фрактал "Кривая Коха".
    /// </summary>
    public class KochCurveDrawer : FractalDrawer
    {
        private (double x, double y) _startPoint;
        
        private double _startWidth;
        private const double PI3 = Math.PI / 3;

        public override int MaxDepth => 8;

        /// <summary>
        /// Конструктор, необходимый для инициализации экземпляра класса.
        /// </summary>
        /// <param name="canvas">Холст для отрисовки фрактала.</param>
        /// <param name="depth">Глубина фрактала.</param>
        public KochCurveDrawer(Canvas canvas, int depth) : base(depth) => UpdateCanvas(canvas);

        /// <summary>
        /// Метод обновления холста для отрисовки фрактала.
        /// </summary>
        /// <param name="canvas">Новый холст.</param>
        public override void UpdateCanvas(Canvas canvas)
        {
            _canvas = canvas;
            _startPoint = (30, canvas.ActualHeight / 2);
            _startWidth = canvas.ActualWidth - 40;
        }

        /// <summary>
        /// Главный метод, который вызывается для отрисовки фрактала.
        /// </summary>
        public override void DrawFractal() => DrawFractalAt(_startPoint, 0, _startWidth, 0);

        /// <summary>
        /// Рекурсивый метод отрисовки фрактала в соответствии с глубиной.
        /// </summary>
        /// <param name="startPoint">Текущая позиция.</param>
        /// <param name="currentAngle">Текущий угол поворота.</param>
        /// <param name="currentWidth">Текущая длина отрезка.</param>
        /// <param name="currentLevel">Текущий уровень рекурсии.</param>
        /// <returns>Новая точка, которая была вычислена методом.</returns>
        private (double x, double y) DrawFractalAt((double x, double y) startPoint, double currentAngle, double currentWidth, int currentLevel)
        {
            var (x1, y1) = startPoint;
            double x2, y2;
            if (currentLevel == Depth - 1)
            {
                (x2, y2) = (
                    x1 + currentWidth * Math.Cos(currentAngle), 
                    y1 - currentWidth * Math.Sin(currentAngle));
                _canvas.Children.Add(new Line
                {
                    X1 = x1, Y1 = y1, X2 = x2, Y2 = y2, Stroke = Brushes.Black
                });
                return (x2, y2);
            }

            var nextWidth = currentWidth / 3.0;
            currentLevel++;
            (x2, y2) = DrawFractalAt(startPoint, currentAngle, nextWidth, currentLevel);
            (x2, y2) = DrawFractalAt((x2, y2), currentAngle + PI3, nextWidth, currentLevel);
            (x2, y2) = DrawFractalAt((x2, y2), currentAngle - PI3, nextWidth, currentLevel);
            return DrawFractalAt((x2, y2), currentAngle, nextWidth, currentLevel);
        }
    }
}