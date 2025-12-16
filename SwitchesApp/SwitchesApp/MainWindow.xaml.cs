using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace SwitchesApp
{
    public partial class MainWindow : Window
    {
        // Насколько увеличиваем кнопку при наведении
        private const double ScaleUpFactor = 1.2;

        // Длительность любой анимации (масштаб, поворот)
        private const double AnimationDurationSeconds = 0.3;

        // На сколько градусов поворачиваем при каждом клике
        private const double RotateAngleStep = 20.0;

        public MainWindow()
        {
            InitializeComponent();
        }

        // Наведение мыши: плавно увеличить переключатель
        private void Switch_MouseEnter(object sender, MouseEventArgs e)
        {
            var button = (Button)sender;

            // Достаем RootGrid из шаблона
            var rootGrid = (Grid)button.Template.FindName("RootGrid", button);
            if (rootGrid == null) return;

            var transformGroup = (TransformGroup)rootGrid.RenderTransform;
            var scale = (ScaleTransform)transformGroup.Children[0];

            var anim = new DoubleAnimation(
                ScaleUpFactor,
                TimeSpan.FromSeconds(AnimationDurationSeconds));

            scale.BeginAnimation(ScaleTransform.ScaleXProperty, anim);
            scale.BeginAnimation(ScaleTransform.ScaleYProperty, anim);
        }

        // Уход мыши: вернуть размер к 1.0
        private void Switch_MouseLeave(object sender, MouseEventArgs e)
        {
            var button = (Button)sender;

            var rootGrid = (Grid)button.Template.FindName("RootGrid", button);
            if (rootGrid == null) return;

            var transformGroup = (TransformGroup)rootGrid.RenderTransform;
            var scale = (ScaleTransform)transformGroup.Children[0];

            var anim = new DoubleAnimation(
                1.0,
                TimeSpan.FromSeconds(AnimationDurationSeconds));

            scale.BeginAnimation(ScaleTransform.ScaleXProperty, anim);
            scale.BeginAnimation(ScaleTransform.ScaleYProperty, anim);
        }

        // Клик: повернуть на +20 градусов по часовой
        private void Switch_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;

            var rootGrid = (Grid)button.Template.FindName("RootGrid", button);
            if (rootGrid == null) return;

            var transformGroup = (TransformGroup)rootGrid.RenderTransform;
            var rotate = (RotateTransform)transformGroup.Children[1];

            double currentAngle = rotate.Angle;
            double targetAngle = currentAngle + RotateAngleStep;

            var anim = new DoubleAnimation(
                targetAngle,
                TimeSpan.FromSeconds(AnimationDurationSeconds));

            rotate.BeginAnimation(RotateTransform.AngleProperty, anim);
        }
    }
}
