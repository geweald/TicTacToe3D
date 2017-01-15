using System;
using System.Windows.Media.Media3D;

namespace TicTacToe3D.Drawing
{
    class Rotation
    {
        public Matrix3D Matrix { get; private set; }

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
            Matrix = Matrix3D.Multiply(Matrix, rot);
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
            Matrix = Matrix3D.Multiply(Matrix, rot);
        }

        public void RotateXY(double x, double y)
        {
            RotateX(x);
            RotateY(y);
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
            Matrix = Matrix3D.Multiply(Matrix, rot);
        }

        public void Reset()
        {
            Matrix = new Matrix3D();
        }
    }
}
