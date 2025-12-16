using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ImagesApp
{
    /// <summary>
    /// Логика взаимодействия для ImageCard.xaml
    /// </summary>
    public partial class ImageCard : UserControl
    {
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(string), typeof(ImageCard),
                new PropertyMetadata("", OnSourceChanged));

        public string Source
        {
            get { return (string)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var card = (ImageCard)d;
            string path = e.NewValue?.ToString();

            if (string.IsNullOrWhiteSpace(path))
            {
                card.imageElement.Source = null;
                return;
            }

            try
            {
                var uri = new Uri(path, UriKind.RelativeOrAbsolute);
                card.imageElement.Source = new System.Windows.Media.Imaging.BitmapImage(uri);
            }
            catch (Exception ex)
            {
                // Логирование ошибки (опционально)
                System.Diagnostics.Debug.WriteLine($"Ошибка загрузки изображения: {path}, Ошибка: {ex.Message}");
                card.imageElement.Source = null;
            }
        }

        public static readonly DependencyProperty CardIdProperty =
            DependencyProperty.Register("CardId", typeof(int), typeof(ImageCard));

        public int CardId
        {
            get { return (int)GetValue(CardIdProperty); }
            set { SetValue(CardIdProperty, value); }
        }

        public event EventHandler<int> RotateRequested;

        public ImageCard()
        {
            InitializeComponent();

            btnRotateLeft.Click += (s, e) => RotateRequested?.Invoke(this, CardId);
            btnRotateRight.Click += (s, e) => RotateRequested?.Invoke(this, -CardId);
        }

        // Метод, который вызывается извне (из MainWindow), чтобы повернуть изображение на указанный угол
        public void ApplyRotation(double angle)
        {
            // Создаем анимацию
            var storyboard = new Storyboard();
            var rotationAnimation = new DoubleAnimation
            {
                To = angle,
                Duration = TimeSpan.FromMilliseconds(250), // Соответствует 0.25 секунды из старого кода
                EasingFunction = new SineEase() { EasingMode = EasingMode.EaseInOut }
            };

            // Устанавливаем цель и свойство для анимации
            // imgRotate — это имя элемента, определённое в XAML
            Storyboard.SetTarget(rotationAnimation, imgRotate);
            Storyboard.SetTargetProperty(rotationAnimation, new PropertyPath(RotateTransform.AngleProperty));

            storyboard.Children.Add(rotationAnimation);
            storyboard.Begin();
        }
    }
}