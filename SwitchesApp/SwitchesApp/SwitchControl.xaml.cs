using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace SwitchesApp
{
    public partial class SwitchControl : UserControl
    {
        private const double ScaleUpFactor = 1.2;
        private const double AnimationDurationSeconds = 0.3;
        private const double RotateAngleStep = 20.0;

        public SwitchControl()
        {
            InitializeComponent();
        }

        private void Switch_MouseEnter(object sender, MouseEventArgs e)
        {
            var btn = (Button)sender;
            var rootGrid = (Grid)btn.Template.FindName("RootGrid", btn);
            var group = (TransformGroup)rootGrid.RenderTransform;
            var scale = (ScaleTransform)group.Children[0];

            var anim = new DoubleAnimation(ScaleUpFactor,
                TimeSpan.FromSeconds(AnimationDurationSeconds));
            scale.BeginAnimation(ScaleTransform.ScaleXProperty, anim);
            scale.BeginAnimation(ScaleTransform.ScaleYProperty, anim);
        }

        private void Switch_MouseLeave(object sender, MouseEventArgs e)
        {
            var btn = (Button)sender;
            var rootGrid = (Grid)btn.Template.FindName("RootGrid", btn);
            var group = (TransformGroup)rootGrid.RenderTransform;
            var scale = (ScaleTransform)group.Children[0];

            var anim = new DoubleAnimation(1.0,
                TimeSpan.FromSeconds(AnimationDurationSeconds));
            scale.BeginAnimation(ScaleTransform.ScaleXProperty, anim);
            scale.BeginAnimation(ScaleTransform.ScaleYProperty, anim);
        }

        private void Switch_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            var rootGrid = (Grid)btn.Template.FindName("RootGrid", btn);
            var group = (TransformGroup)rootGrid.RenderTransform;
            var rotate = (RotateTransform)group.Children[1];

            double targetAngle = rotate.Angle + RotateAngleStep;
            var anim = new DoubleAnimation(targetAngle,
                TimeSpan.FromSeconds(AnimationDurationSeconds));
            rotate.BeginAnimation(RotateTransform.AngleProperty, anim);
        }
    }
}
