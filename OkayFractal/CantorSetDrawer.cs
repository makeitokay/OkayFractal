using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace OkayFractal
{
    /// <summary>
    /// Класс, отвечающий за отрисовку фрактала "Множество Кантора".
    /// </summary>
    public class CantorSetDrawer : FractalDrawer
    {
        private double _startWidth;
        private (double x, double y) _startPoint;

        /// <summary>
        /// Конструктор, необходимый для инициализации объекта.
        /// </summary>
        /// <param name="canvas">Холст для отрисовки фрактала.</param>
        /// <param name="depth">Глубина фрактала.</param>
        public CantorSetDrawer(Canvas canvas, int depth) : base(depth) => UpdateCanvas(canvas);

        /// <summary>
        /// Метод обновления холста для отрисовки фрактала.
        /// </summary>
        /// <param name="canvas">Новый холст.</param>
        public override void UpdateCanvas(Canvas canvas)
        {
            _canvas = canvas;
            _startWidth = _canvas.ActualWidth - 50;
            _startPoint = (15, _canvas.ActualHeight / 2.0);
        }

        public override int MaxDepth => 8;
        
        /// <summary>
        /// Главный метод, вызывающий отрисовку фрактала. 
        /// </summary>
        public override void DrawFractal() => DrawFractalAt(_startPoint, 0);

        /// <summary>
        /// Рекурсивный метод, отрисовывающий фрактал в соответствии с глубиной.
        /// </summary>
        /// <param name="point">Первая точка нового отрезка.</param>
        /// <param name="currentLevel">Текущий уровень рекурсии.</param>
        private void DrawFractalAt((double x, double y) point, int currentLevel)
        {
            if (currentLevel >= Depth)
                return;

            double currentWidth = _startWidth / Math.Pow(3, currentLevel);
            _canvas.Children.Add(new Line
            {
                X1 = point.x,
                Y1 = point.y,
                X2 = point.x + currentWidth,
                Y2 = point.y,
                Stroke = Brushes.Black,
                StrokeThickness = 7
            });
            currentLevel++;
            var newY = point.y + 20; // newY не Нью-Йорк, а новый игрик, если чо ))
            DrawFractalAt((point.x, newY), currentLevel);
            DrawFractalAt((point.x + currentWidth * (2.0 / 3), newY), currentLevel);
        }
    }
}