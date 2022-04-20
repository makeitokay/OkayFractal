using System.Windows;
using System.Windows.Controls;

namespace OkayFractal
{
    /// <summary>
    /// Класс графического окна программы.
    /// </summary>
    public partial class MainWindow
    {
        private int _currentFractalDepth = 1;

        private FractalDrawer _currentFractal;
        
        /// <summary>
        /// Конструктор.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Обработчик события изменения глубины фрактала на слайдере.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void FractalDepthSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int newFractalDepth = (int) e.NewValue;
            
            ((Slider)sender).SelectionEnd = newFractalDepth;
            if (_currentFractalDepth == newFractalDepth)
                return;
            _currentFractalDepth = newFractalDepth;

            FractalCanvas?.Children.Clear();
            _currentFractal?.UpdateDepth(_currentFractalDepth);
        }

        /// <summary>
        /// Обработчик события изменения глубины фрактала в текстбоксе.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void FractalDepthTextBoxValue_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (FractalCanvas == null)
                return;
            
            TextBox textBox = (TextBox) sender;
            if (string.IsNullOrEmpty(textBox.Text))
                return;
            
            if (!int.TryParse(textBox.Text, out int newFractalDepth) || !(newFractalDepth >= 1 && newFractalDepth <= _currentFractal.MaxDepth))
            {
                MessageBox.Show($"Вводите число от 1 до {_currentFractal.MaxDepth}");
                textBox.Text = _currentFractalDepth.ToString();
                return;
            }

            if (_currentFractalDepth == newFractalDepth)
                return;
            
            _currentFractalDepth = newFractalDepth;
            FractalCanvas?.Children.Clear();
            _currentFractal?.UpdateDepth(_currentFractalDepth);
        }

        /// <summary>
        /// Обработчик события выбора фрактала.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void FractalRadioButton_OnChecked(object sender, RoutedEventArgs e)
        {
            if (FractalCanvas == null)
                return;
            RadioButton pressedButton = (RadioButton) sender;

            _currentFractal = pressedButton.Name switch
            {
                "CantorSetButton" => new CantorSetDrawer(FractalCanvas, _currentFractalDepth),
                "SerpinskyTriangleButton" => new SerpinskyTriangleDrawer(FractalCanvas, _currentFractalDepth), 
                "KochCurveButton" => new KochCurveDrawer(FractalCanvas, _currentFractalDepth),
                "SerpinskyCarpetButton" => new SerpinskyCarpetButton(FractalCanvas, _currentFractalDepth),
                "FractalTreeButton" or _ => new FractalTreeDrawer(
                    FractalCanvas, 
                    _currentFractalDepth, 
                    LeftTreeAngleSlider.Value, 
                    RightTreeAngleSlider.Value,
                    ScaleSlider.Value)
            };
            
            FractalDepthSlider.Maximum = _currentFractal.MaxDepth;
            if (_currentFractalDepth > _currentFractal.MaxDepth)
            {
                _currentFractalDepth = _currentFractal.MaxDepth;
                FractalDepthSlider.Value = _currentFractalDepth;
                FractalDepthTextBoxValue.Text = _currentFractalDepth.ToString();
            }

            FractalCanvas.Children.Clear();
            _currentFractal.DrawFractal();

        }

        /// <summary>
        /// Обработчик события полной загрузки окна.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            _currentFractal = new FractalTreeDrawer(
                FractalCanvas, 
                _currentFractalDepth, 
                LeftTreeAngleSlider.Value, 
                RightTreeAngleSlider.Value,
                ScaleSlider.Value);
            _currentFractal.DrawFractal();
        }

        /// <summary>
        /// Обработчик события изменения угла ветки фрактального дерева.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void TreeAngleSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int newAngle = (int) e.NewValue;
            
            ((Slider)sender).SelectionEnd = newAngle;

            if (_currentFractal is not FractalTreeDrawer drawer) 
                return;
            _currentFractal = drawer;
            drawer.LeftAngle = LeftTreeAngleSlider.Value;
            drawer.RightAngle = RightTreeAngleSlider.Value;
            FractalCanvas?.Children.Clear();
            drawer.DrawFractal();
        }

        /// <summary>
        /// Обработчик события изменения коэффициента уменьшения длин отрезков фрактального дерева.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void ScaleSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double newScale = e.NewValue;
            
            ((Slider)sender).SelectionEnd = newScale;
            
            if (_currentFractal is not FractalTreeDrawer drawer) 
                return;
            _currentFractal = drawer;
            drawer.Scale = newScale;
            FractalCanvas?.Children.Clear();
            drawer.DrawFractal();
        }

        /// <summary>
        /// Обработчик события изменения размеров окна.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void MainWindow_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (FractalCanvas == null)
                return;
            
            FractalCanvas.Children.Clear();
            _currentFractal?.UpdateCanvas(FractalCanvas);
            _currentFractal?.DrawFractal();
        }
    }
}