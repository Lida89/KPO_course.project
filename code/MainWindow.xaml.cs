using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace WpfApp1 {
    public partial class MainWindow : Window
    {
        private bool pause = false;
        private bool start = true;
        EllipseInfo info;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Start_Click(object sender, RoutedEventArgs e)
        {
            start = true;
            info = new EllipseInfo(Purs_ball, 0.04 * Convert.ToInt32(Speed_v2.Text));
            CompositionTarget.Rendering += StartAnimation;
            CompositionTarget.Rendering += StopAnimation;
            Start_btn.Content = "...";
        }      
        private void StartAnimation(object sender, EventArgs e)
        {
            double x1 = (double)Ball.GetValue(Canvas.LeftProperty);
            double y1 = (double)Ball.GetValue(Canvas.TopProperty);
            double x2 = (double)Purs_ball.GetValue(Canvas.LeftProperty);
            double y2 = (double)Purs_ball.GetValue(Canvas.TopProperty);
            double A = x1 - x2, B = y1 - y2;
            if ((Math.Abs(A) < 50) && (Math.Abs(B) < 50))
            {
                A *= 3;
                B *= 3;
            }
            else if ((Math.Abs(A) < 80) && (Math.Abs(B) < 80))
            {
                A *= 1.5;
                B *= 1.5;
            }

            Canvas.SetTop(info.Ellipse, y2 + 1 * info.VelocityY * B);
            Canvas.SetLeft(info.Ellipse, x2 + 1 * info.VelocityY * A);
        }
        private void StopAnimation(object sender, EventArgs e)
        {
            double sx = (double)Ball.GetValue(Canvas.LeftProperty);
            double sy = (double)Ball.GetValue(Canvas.TopProperty);
            double bx = (double)Purs_ball.GetValue(Canvas.LeftProperty);
            double by = (double)Purs_ball.GetValue(Canvas.TopProperty);
            if (Math.Abs(sx - bx) <= 10 && Math.Abs(sy - by) <= 10)
            {
                Start_btn.Content = "Restart";
                start = false;
                CompositionTarget.Rendering -= StartAnimation;
                CompositionTarget.Rendering -= StopAnimation;
            }
        }
        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            if (pause && start)
            {
                Pause_btn.Content = "Pause";
                CompositionTarget.Rendering += StartAnimation;
            }
            else if (start)
            {
                Pause_btn.Content = "Resume";
                CompositionTarget.Rendering -= StartAnimation;
            }
            pause = !pause;
        }
        private void Canvas_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            double sx = (double)Ball.GetValue(Canvas.LeftProperty);
            double sy = (double)Ball.GetValue(Canvas.TopProperty);
            double bx = (double)Purs_ball.GetValue(Canvas.LeftProperty);
            double by = (double)Purs_ball.GetValue(Canvas.TopProperty);
            Point tp = e.GetPosition(Canvas_ball); // получает позицию мыши в Canvas_ball

            LineGeometry geom = new LineGeometry(new Point(sx, sy), tp);

            var animation = new DoubleAnimationUsingPath
            {
                Duration = Duration.Automatic,
                SpeedRatio = Convert.ToInt32(Speed_v1.Text),
                PathGeometry = PathGeometry.CreateFromGeometry(geom)                
            };
            animation.FillBehavior = FillBehavior.HoldEnd;
            animation.Source = PathAnimationSource.X;
            Ball.BeginAnimation(Canvas.LeftProperty, animation);
            animation.Source = PathAnimationSource.Y;
            Ball.BeginAnimation(Canvas.TopProperty, animation);
        }
    }
    public class EllipseInfo
    {
        public Ellipse Ellipse
        {
            get; set;
        }
        public double VelocityY
        {
            get; set;
        }
        public EllipseInfo(Ellipse ellipse, double velocityY)
        {
            VelocityY = velocityY;
            Ellipse = ellipse;
        }
    }
}
