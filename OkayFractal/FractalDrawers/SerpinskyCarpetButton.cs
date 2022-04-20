using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace OkayFractal
{
    /// <summary>
    /// Класс, отрисовывающий фрактал "Ковер Серпиского".
    /// </summary>
    public class SerpinskyCarpetButton : FractalDrawer
    {
        private double _startWidth;

        public override int MaxDepth => 7;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="canvas">Холст для отрисовки фрактала.</param>
        /// <param name="depth">Глубина фрактала.</param>
        public SerpinskyCarpetButton(Canvas canvas, int depth) : base(canvas, depth) {}

        /// <summary>
        /// Установка холста для отрисовки фрактала.
        /// </summary>
        public override Canvas Canvas
        {
            set
            {
                _canvas = value;
                _startWidth = _canvas.ActualWidth / 2;
            }
        }

        /// <summary>
        /// Метод, отрисовывающий фрактал.
        /// </summary>
        public override void DrawFractal() => DrawFractalAt(
            ((_canvas.ActualHeight - _startWidth) / 2, (_canvas.ActualWidth - _startWidth) / 2), 0);

        /// <summary>
        /// Рекурсивный метод, отрисовывающий фрактал в соответствии с глубиной.
        /// </summary>
        /// <param name="currentRect">Текущий квадрат.</param>
        /// <param name="currentLevel">Текущий уровень рекурсии.</param>
        private void DrawFractalAt((double x, double y) currentRect, int currentLevel)
        {
            if (currentLevel >= Depth)
                return;
            
            if (currentLevel == 0)
            {
                var mainRectangle = new Rectangle {Width = _startWidth, Height = _startWidth, Fill = Brushes.Blue};
                _canvas.Children.Add(mainRectangle);
                Canvas.SetTop(mainRectangle, currentRect.x);
                Canvas.SetRight(mainRectangle, currentRect.y);
                DrawFractalAt(currentRect, 1);
            }
            else
            {
                var newWidth = _startWidth / Math.Pow(3.0, currentLevel);
                var centerRect = new Rectangle
                {
                    Width = newWidth,
                    Height = newWidth,
                    Fill = Brushes.White
                };

                Canvas.SetTop(centerRect, currentRect.x + newWidth);
                Canvas.SetRight(centerRect, currentRect.y + newWidth);
                _canvas.Children.Add(centerRect);
                for (int i = 0; i < 3; i++)
                    for (int j = 0; j < 3; j++)
                        DrawFractalAt((currentRect.x + i * newWidth, currentRect.y + j * newWidth), currentLevel + 1);
            }
        }
    }
}