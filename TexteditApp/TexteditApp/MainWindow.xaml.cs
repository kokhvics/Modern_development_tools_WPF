using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace TexteditApp
{
    public partial class MainWindow : Window
    {
        private readonly Point[] _homePositions = new Point[5];
        private readonly Point _centerPosition = new Point(425, 180); // новый центр для ширины 1050
        private Border _currentFocusedBlock = null;

        // ✅ НОВЫЕ ИСХОДНЫЕ ПЕРСПЕКТИВЫ для каждого блока
        private readonly double[] _homeScaleX = { 1.0, 1.2, 1.0, 1.2, 1.2 };
        private readonly double[] _homeScaleY = { 1.2, 1.25, 1.0, 1.25, 1.25 };
        private readonly double[] _homeSkewY = { 20, 8, 0, -8, -20 };

        private const double AnimationDuration = 0.5;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            MouseLeftButtonDown += MainWindow_MouseLeftButtonDown;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // ✅ НОВЫЕ координаты из XAML
            _homePositions[0] = new Point(30, 270);   // Block1
            _homePositions[1] = new Point(200, 300);  // Block2  
            _homePositions[2] = new Point(390, 300);  // Block3
            _homePositions[3] = new Point(630, 300);  // Block4
            _homePositions[4] = new Point(830, 250);  // Block5
        }

        private void MainWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_currentFocusedBlock != null)
            {
                int index = GetBlockIndex(_currentFocusedBlock);
                AnimateToHome(_currentFocusedBlock, index);
                _currentFocusedBlock = null;
            }
        }

        private void TextBlock_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null) return;

            var block = textBox.Parent as Border;
            if (block == null || block == _currentFocusedBlock) return;

            if (_currentFocusedBlock != null)
            {
                int prevIndex = GetBlockIndex(_currentFocusedBlock);
                AnimateToHome(_currentFocusedBlock, prevIndex);
            }

            Panel.SetZIndex(block, 100);
            _currentFocusedBlock = block;
            int blockIndex = GetBlockIndex(block);
            AnimateToCenter(block, blockIndex);
        }

        private void TextBlock_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null || _currentFocusedBlock == null) return;

            var block = textBox.Parent as Border;
            if (block != _currentFocusedBlock) return;

            Panel.SetZIndex(block, 0);
            int blockIndex = GetBlockIndex(block);
            AnimateToHome(block, blockIndex);
            _currentFocusedBlock = null;
        }

        private int GetBlockIndex(Border block)
        {
            if (block == Block1) return 0;
            if (block == Block2) return 1;
            if (block == Block3) return 2;
            if (block == Block4) return 3;
            if (block == Block5) return 4;
            return -1;
        }

        private void AnimateToCenter(Border block, int index)
        {
            var transforms = GetAllTransforms(block);
            if (transforms.Length < 3) return;

            var scale = transforms[0] as ScaleTransform;
            var skew = transforms[1] as SkewTransform;

            // ✅ НОРМАЛЬНЫЙ ВИД В ЦЕНТРЕ
            AnimateTransform(scale, ScaleTransform.ScaleXProperty, _homeScaleX[index], 1.0);
            AnimateTransform(scale, ScaleTransform.ScaleYProperty, _homeScaleY[index], 1.0);
            AnimateTransform(skew, SkewTransform.AngleXProperty, 0, 0);
            AnimateTransform(skew, SkewTransform.AngleYProperty, _homeSkewY[index], 0);
            AnimateUIElement(block, UIElement.OpacityProperty, 0.7, 1.0);

            AnimateCanvasPosition(block, _homePositions[index], _centerPosition);
        }

        private void AnimateToHome(Border block, int index)
        {
            var transforms = GetAllTransforms(block);
            if (transforms.Length < 3) return;

            var scale = transforms[0] as ScaleTransform;
            var skew = transforms[1] as SkewTransform;

            // ✅ ВОЗВРАТ К ПЕРСПЕКТИВЕ каждого блока
            AnimateTransform(scale, ScaleTransform.ScaleXProperty, 1.0, _homeScaleX[index]);
            AnimateTransform(scale, ScaleTransform.ScaleYProperty, 1.0, _homeScaleY[index]);
            AnimateTransform(skew, SkewTransform.AngleXProperty, 0, 0);
            AnimateTransform(skew, SkewTransform.AngleYProperty, 0, _homeSkewY[index]);
            AnimateUIElement(block, UIElement.OpacityProperty, 1.0, 0.7);

            AnimateCanvasPosition(block, _centerPosition, _homePositions[index]);
        }

        private Transform[] GetAllTransforms(Border block)
        {
            var group = block.RenderTransform as TransformGroup;
            return group?.Children?.ToArray() ?? new Transform[0];
        }

        private void AnimateCanvasPosition(Border block, Point from, Point to)
        {
            var leftAnim = new DoubleAnimation(from.X, to.X, TimeSpan.FromSeconds(AnimationDuration))
            {
                EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
            };
            var topAnim = new DoubleAnimation(from.Y, to.Y, TimeSpan.FromSeconds(AnimationDuration))
            {
                EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
            };

            block.BeginAnimation(Canvas.LeftProperty, leftAnim);
            block.BeginAnimation(Canvas.TopProperty, topAnim);
        }

        private void AnimateUIElement(UIElement target, DependencyProperty property, double from, double to)
        {
            if (target == null) return;
            var anim = new DoubleAnimation(from, to, TimeSpan.FromSeconds(AnimationDuration))
            {
                EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
            };
            target.BeginAnimation(property, anim);
        }

        private void AnimateTransform(Transform target, DependencyProperty property, double from, double to)
        {
            if (target == null) return;
            var anim = new DoubleAnimation(from, to, TimeSpan.FromSeconds(AnimationDuration))
            {
                EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
            };
            target.BeginAnimation(property, anim);
        }
    }
}
