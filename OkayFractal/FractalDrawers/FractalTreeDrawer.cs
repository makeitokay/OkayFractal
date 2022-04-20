using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace OkayFractal
{
    /// <summary>
    /// Класс, отрисовывающий фрактал "Обдуваемое ветром фрактальное дерево".
    /// </summary>
    public class FractalTreeDrawer : FractalDrawer
    {
        private (double x, double y) _startPoint;
        
        private double _startWidth;
        public override int MaxDepth => 11;

        private const double HalfPI = Math.PI / 2;

        private double _leftAngle;
        private double _rightAngle;
        public double LeftAngle
        {
            get => _leftAngle;
            set => _leftAngle = ToRadians(value);
        }
        public double RightAngle
        {
            get => _rightAngle;
            set => _rightAngle = ToRadians(value);
        }
        public double Scale { get; set; }
        
        /// <summary>
        /// Конструктор, необходимый для инициализации экземпляра класса.
        /// </summary>
        /// <param name="canvas">Холст для отрисовки фрактала.</param>
        /// <param name="depth">Глубина фрактала.</param>
        /// <param name="leftAngle">Угол левой ветки дерева.</param>
        /// <param name="rightAngle">Угол правой ветки дерева.</param>
        /// <param name="scale">Доп. коэффициент, задающий отношение между длинами отрезков соседних итераций.</param>
        public FractalTreeDrawer(Canvas canvas, int depth, double leftAngle, double rightAngle, double scale) : base(canvas, depth)
        {
            LeftAngle = leftAngle;
            RightAngle = rightAngle;
            Scale = scale;
        }

        /// <summary>
        /// Установка холста для отрисовки фрактала.
        /// </summary>
        public override Canvas Canvas
        {
            set
            {
                _canvas = value;
                _startPoint = (_canvas.ActualWidth / 2, _canvas.ActualHeight - 15);
                _startWidth = _canvas.ActualHeight / 5; 
            }
        }
        
        /// <summary>
        /// Главный метод, который вызывает отрисовку фрактала.
        /// </summary>
        public override void DrawFractal() => DrawFractalAt(_startPoint, 0);

        /// <summary>
        /// Переводит градусы в радианы.
        /// </summary>
        /// <param name="angle">Угол, который необходимо перевести.</param>
        /// <returns>Угол в радианах.</returns>
        private static double ToRadians(double angle) => Math.PI / 180 * angle;

        /// <summary>
        /// Рекурсивный метод, отрисовывающий фрактал в соответствии с глубиной.
        /// </summary>
        /// <param name="point">Текущая позиция</param>
        /// <param name="currentLevel"></param>
        /// <param name="angle"></param>
        private void DrawFractalAt((double x, double y) point, int currentLevel, double angle = HalfPI)
        {
            if (currentLevel >= Depth)
                return;
            
            var (x1, y1) = point;
            double x2, y2;
            if (currentLevel == 0)
            {
                (x2, y2) = (x1, y1 - _startWidth);
                _canvas.Children.Add(new Line { X1 = x1, Y1 = y1, X2 = x2, Y2 = y2, Stroke = Brushes.Black });
            }
            else
            {
                var scale = Scale * (Depth * 0.9 - currentLevel) / (Depth * 0.9);
                (x2, y2) = (
                    x1 + Math.Cos(angle) * _startWidth * scale,
                    y1 - Math.Sin(angle) * _startWidth * scale
                );
                _canvas.Children.Add(new Line { X1 = x1, Y1 = y1, X2 = x2, Y2 = y2, Stroke = Brushes.Black });
            }
            
            DrawFractalAt((x2, y2), currentLevel + 1, angle + LeftAngle);
            DrawFractalAt((x2, y2), currentLevel + 1, angle - RightAngle);
        }
    }
}