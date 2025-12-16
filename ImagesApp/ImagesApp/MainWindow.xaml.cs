using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace ImagesApp
{
    public partial class MainWindow : Window
    {
        private const double RotateStep = 90.0;
        private const double RotateDurationSeconds = 0.25;

        public MainWindow()
        {
            InitializeComponent();
            LoadImages();
        }

        private void LoadImages()
        {
            // Пути к 6 картинкам в папке Images
            SetImageSource(img1, "Images/img1.jpg");
            SetImageSource(img2, "Images/img2.jpg");
            SetImageSource(img3, "Images/img3.jpg");
            SetImageSource(img4, "Images/img4.jpg");
            SetImageSource(img5, "Images/img5.jpg");
            SetImageSource(img6, "Images/img6.jpg");
        }

        private void SetImageSource(Image img, string relativePath)
        {
            try
            {
                img.Source = new BitmapImage(new Uri(relativePath, UriKind.Relative));
            }
            catch
            {
                // если файл не найден, просто ничего не делаем
            }
        }

        private void RotateLeft_Click(object sender, RoutedEventArgs e)
        {
            RotateFromButton(sender, -RotateStep);
        }

        private void RotateRight_Click(object sender, RoutedEventArgs e)
        {
            RotateFromButton(sender, RotateStep);
        }

        private void RotateFromButton(object sender, double angleDelta)
        {
            // sender – кнопка; через Tag знаем номер картинки
            var btn = sender as Button;
            if (btn == null) return;

            int index;
            if (!int.TryParse(btn.Tag.ToString(), out index)) return;

            RotateTransform rotate = null;

            switch (index)
            {
                case 1:
                    rotate = GetRotateTransform(ImageHost1);
                    break;
                case 2:
                    rotate = GetRotateTransform(ImageHost2);
                    break;
                case 3:
                    rotate = GetRotateTransform(ImageHost3);
                    break;
                case 4:
                    rotate = GetRotateTransform(ImageHost4);
                    break;
                case 5:
                    rotate = GetRotateTransform(ImageHost5);
                    break;
                case 6:
                    rotate = GetRotateTransform(ImageHost6);
                    break;
            }

            if (rotate == null) return;

            double from = rotate.Angle;
            double to = from + angleDelta;

            var anim = new DoubleAnimation();
            anim.From = from;
            anim.To = to;
            anim.Duration = TimeSpan.FromSeconds(RotateDurationSeconds);
            anim.EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut };

            rotate.BeginAnimation(RotateTransform.AngleProperty, anim);
        }

        private RotateTransform GetRotateTransform(Grid host)
        {
            if (host == null) return null;

            var group = host.RenderTransform as TransformGroup;
            if (group == null || group.Children.Count < 2) return null;

            return group.Children[1] as RotateTransform;
        }
    }
}
