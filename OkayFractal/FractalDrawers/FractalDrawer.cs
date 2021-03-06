using System.Windows.Controls;

namespace OkayFractal
{
    /// <summary>
    /// Базовый абстрактный класс для всех классов, отрисовывающих фракталы.
    /// </summary>
    public abstract class FractalDrawer
    {
        public int Depth { get; set; }

        public abstract int MaxDepth { get; }
        
        /// <summary>
        /// Метод, который будет вызван для отрисовки фрактала.
        /// </summary>
        public abstract void DrawFractal();

        public abstract Canvas Canvas
        {
            set;
        }
        
        protected Canvas _canvas;

        /// <summary>
        /// Конструктор, инициализирующий экземпляр класса.
        /// </summary>
        /// <param name="depth">Глубина фрактала.</param>
        protected FractalDrawer(Canvas canvas, int depth)
        {
            Canvas = canvas;
            Depth = depth;
        }

        /// <summary>
        /// Метод обновления глубины фрактала.
        /// </summary>
        /// <param name="newDepth">Новая глубина фрактала.</param>
        public void UpdateDepth(int newDepth)
        {
            Depth = newDepth;
            DrawFractal();
        }
    }
}