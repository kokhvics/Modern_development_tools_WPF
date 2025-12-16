using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace RoadsignApp
{
    public partial class MainWindow : Window
    {
        // Хранит информацию о состоянии каждого знака (для анимации нажатия)
        private Dictionary<UIElement, SignState> _signStates = new Dictionary<UIElement, SignState>();

        public MainWindow()
        {
            InitializeComponent();

            // Подписываемся на событие MouseDown для каждого контейнера
            SubscribeToClick(PathSign1Container);
            SubscribeToClick(PathSign2Container);
            SubscribeToClick(PathSign3Container);
            SubscribeToClick(ImgSign1Container);
            SubscribeToClick(ImgSign2Container);
            SubscribeToClick(ImgSign3Container);
        }

        private void SubscribeToClick(Border border)
        {
            border.MouseDown += OnSignClicked;
            // Инициализируем состояние
            _signStates[border] = new SignState
            {
                OriginalPosition = border.Margin,
                OriginalScale = new ScaleTransform(1, 1),
                OriginalTranslate = new TranslateTransform(0, 0)
            };
            border.RenderTransform = new TransformGroup { Children = { _signStates[border].OriginalScale, _signStates[border].OriginalTranslate } };
        }

        private void OnSignClicked(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var clickedBorder = (Border)sender;
            var state = _signStates[clickedBorder];

            // Создаём копию содержимого (это может быть Path или Image)
            UIElement contentCopy = null;

            if (clickedBorder == PathSign1Container || clickedBorder == PathSign2Container || clickedBorder == PathSign3Container)
            {
                // Для Path-знаков: создаем копию Viewbox с содержимым
                var viewbox = (Viewbox)clickedBorder.Child;
                var grid = (Grid)viewbox.Child;

                // Создаем новый Viewbox с тем же содержимым
                var newViewbox = new Viewbox { Stretch = Stretch.Uniform }; // Убедимся, что пропорции сохраняются
                var newGrid = new Grid { Width = grid.Width, Height = grid.Height };

                foreach (var child in grid.Children)
                {
                    var path = child as Path;
                    if (path != null)
                    {
                        var newPath = new Path
                        {
                            Data = path.Data,
                            Fill = path.Fill,
                            Stroke = path.Stroke,
                            StrokeThickness = path.StrokeThickness
                        };
                        newGrid.Children.Add(newPath);
                    }
                }

                newViewbox.Child = newGrid;
                contentCopy = newViewbox;
            }
            else if (clickedBorder == ImgSign1Container || clickedBorder == ImgSign2Container || clickedBorder == ImgSign3Container)
            {
                // Для Image-знаков: создаем копию Image
                var image = (Image)clickedBorder.Child;
                var newImage = new Image
                {
                    Source = image.Source,
                    Stretch = image.Stretch,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                contentCopy = newImage;
            }

            // Устанавливаем копию в правую область
            LargePreviewContent.Content = contentCopy;

            // УБИРАЕМ анимацию увеличения, т.к. Viewbox сам масштабирует
            // var scaleAnim = new DoubleAnimation(2.0, TimeSpan.FromSeconds(0.3)); // Увеличиваем в 2 раза

            // var transformGroup = new TransformGroup();
            // var scaleTransform = new ScaleTransform(1, 1);
            // var translateTransform = new TranslateTransform(0, 0);
            // transformGroup.Children.Add(scaleTransform);
            // transformGroup.Children.Add(translateTransform);

            // LargePreviewContent.RenderTransform = transformGroup;

            // scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnim);
            // scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnim);
        }

        // Класс для хранения состояния знака
        private class SignState
        {
            public Thickness OriginalPosition { get; set; }
            public ScaleTransform OriginalScale { get; set; }
            public TranslateTransform OriginalTranslate { get; set; }
        }
    }
}