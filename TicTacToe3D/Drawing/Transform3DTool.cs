using System.Windows.Media.Media3D;
using Point = System.Windows.Point;

namespace TicTacToe3D.Drawing
{
    internal class Transform3DTool
    {
        private Matrix3D _projection;
        private readonly Rotation _rotation;

        private double _canvasHalfWidth;
        private double _canvasHalfHeight;
        private double _canvSize;
        private double _zoom;

        public double D { get; }


        public Transform3DTool()
        {
            D = 5.0;
            _zoom = D;
            _projection.OffsetZ = D;
            _rotation = new Rotation();

            RotateZ(90);
            RotateY(-90);
            RotateX(70);
            RotateY(-20);
        }

        public Point TransformPointTo2D(Point3D point3D)
        {
            point3D = _projection.Transform(point3D);
            point3D.Z += _zoom;
            var x = D * point3D.X / (point3D.Z + D) * _canvSize + _canvasHalfWidth;
            var y = D * point3D.Y / (point3D.Z + D) * _canvSize + _canvasHalfHeight;
            return new Point(x, y);
        }

        public Point[] TransformPointsTo2D(Point3D[] points3D)
        {
            var len = points3D.Length;
            var result = new Point[len];
            for (int i = 0; i < len; ++i)
                result[i] = TransformPointTo2D(points3D[i]);
            return result;
        }

        public Point3D TransformPoint3D(Point3D point3D)
        {
            return _rotation.Matrix.Transform(point3D);
        }

        public Point3D[] TransformPoints3D(Point3D[] points3D)
        {
            _rotation.Matrix.Transform(points3D);
            return points3D;
        }

        public double GetCamZ()
        {
            return _zoom + 2 * D;
        }

        public void SetProjectionCanvasSize(double width, double height)
        {
            _canvSize = width > height ? height / 2.0 : width / 2.0;
            _canvasHalfHeight = height / 2;
            _canvasHalfWidth = width / 2;
        }

        public void RotateX(double a)
        {
            _rotation.RotateX(a);
        }

        public void RotateY(double a)
        {
            _rotation.RotateY(a);
        }

        public void RotateXY(double x, double y)
        {
            _rotation.RotateX(x);
            _rotation.RotateY(y);
        }

        public void RotateZ(double a)
        {
            _rotation.RotateZ(a);
        }

        public void ResetRotation()
        {
            _rotation.Reset();
        }

        public void Zoom(double zoom)
        {
            if (_zoom - zoom > 0 && _zoom - zoom < 100)
                _zoom -= zoom;
        }

        public void SetZoom(double zoom)
        {
            _zoom = zoom;
        }
    }
}
