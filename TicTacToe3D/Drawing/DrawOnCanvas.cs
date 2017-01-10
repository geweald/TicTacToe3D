using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Point = System.Windows.Point;

namespace TicTacToe3D.Drawing
{
    static class DrawOnCanvas
    {
        public static Canvas DrawPointsLine(Point[] points, SolidColorBrush stroke, int thickness = 1)
        {
            var canvas = new Canvas();
            for (int i = 0; i < points.Length - 1; i++)
            {
                var line = new Line
                {
                    Stroke = stroke,
                    StrokeThickness = thickness,
                    X1 = points[i].X,
                    Y1 = points[i].Y,
                    X2 = points[i + 1].X,
                    Y2 = points[i + 1].Y
                };
                canvas.Children.Add(line);
            }
            canvas.Children.Add(new Line
            {
                Stroke = stroke,
                StrokeThickness = thickness,
                X1 = points[0].X,
                Y1 = points[0].Y,
                X2 = points[points.Length - 1].X,
                Y2 = points[points.Length - 1].Y
            });
            return canvas;
        }

        public static BitmapSource BitmapToBitmapImage(Bitmap img)
        {
            if (img == null) return null;
            var bi = new BitmapImage();
            using (var ms = new MemoryStream())
            {
                bi.BeginInit();
                img.Save(ms, ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);
                bi.StreamSource = ms;
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.EndInit();
                bi.Freeze();
            }
            return bi;
        }

    }
}
