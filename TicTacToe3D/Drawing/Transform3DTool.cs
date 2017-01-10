using System;
using System.Windows.Media.Media3D;
using Point = System.Windows.Point;

namespace TicTacToe3D.Drawing
{
    internal class Transform3DTool
    {
        private static Transform3DTool _instance;
        private Matrix3D _projection;
        private Matrix3D _transform;
        private double _canvasHalfWidth;
        private double _canvasHalfHeight;
        private double _zoom;
        private double _canvSize;
        public double D { get; }


        private Transform3DTool()
        {
            D = 5.0;
            _zoom = 5.0;
            _projection.OffsetZ = D;
        }

        public static Transform3DTool Instance()
        {
            return _instance ?? (_instance = new Transform3DTool());
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
            return _transform.Transform(point3D);
        }

        public Point3D[] TransformPoints3D(Point3D[] points3D)
        {
            _transform.Transform(points3D);
            return points3D;
        }


        public void SetOffset(double x, double y)
        {
            _transform.OffsetX = x;
            _transform.OffsetY = y;
        }

        public void SetCanvasSize(double width, double height)
        {
            _canvSize = width > height ? height / 2.0 : width / 2.0;
            _canvasHalfHeight = height / 2;
            _canvasHalfWidth = width / 2;
        }

        public void RotateX(double a)
        {
            var rad = a * Math.PI / 180.0;
            var sin = Math.Sin(rad);
            var cos = Math.Cos(rad);
            var rot = new Matrix3D
            {
                M22 = cos,
                M33 = cos,
                M32 = sin,
                M23 = -sin
            };
            _transform = rot;
        }

        public void RotateY(double a)
        {
            var rad = a * Math.PI / 180.0;
            var sin = Math.Sin(rad);
            var cos = Math.Cos(rad);
            var rot = new Matrix3D
            {
                M11 = cos,
                M33 = cos,
                M13 = sin,
                M31 = -sin
            };
            _transform = rot;
        }

        public void RotateZ(double a)
        {
            var rad = a * Math.PI / 180.0;
            var sin = Math.Sin(rad);
            var cos = Math.Cos(rad);
            var rot = new Matrix3D
            {
                M11 = cos,
                M22 = cos,
                M21 = sin,
                M12 = -sin
            };
            _transform = rot;
        }

        public void Zoom(double zoom)
        {
            if (_zoom - zoom > 0)
                _zoom -= zoom;
        }
    }
}
