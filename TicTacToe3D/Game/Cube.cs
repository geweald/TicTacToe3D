using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Media3D;
using TicTacToe3D.Drawing;

namespace TicTacToe3D.Game
{
    class Cube
    {
        private Point3D[] _vertexes;
        private readonly Face[] _faces;
        private Point3D _center;
        public double Size { get; private set; }

        public Cube(double x, double y, double z)
        {
            _faces = new[]
            {
                new Face(), new Face(), new Face(), new Face(), new Face(), new Face()
            };
            MakeCubeVertexes(x, y, z);
            SetCubeFaces();
        }

        public void Transform(Transform3DTool t3D)
        {
            t3D.TransformPoints3D(_vertexes);
            CalculateCenterAndSize();
            SetCubeFaces();
        }

        public List<Point3D[]> CubeFaces()
        {
            var result = new List<Point3D[]>();
            var faces = _faces.OrderByDescending(f => f.MaxZ).ToArray();
            foreach (var face in faces)
                result.Add(face.Points());
            return result;
        }

        private void MakeCubeVertexes(double x, double y, double z)
        {
            int v = 8;
            _vertexes = new Point3D[v];
            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 2; j++)
                    for (int k = 0; k < 2; k++)
                        _vertexes[--v] = new Point3D(x + i, y + j, z + k);


            CalculateCenterAndSize();
        }

        private void CalculateCenterAndSize()
        {
            var centerX = (_vertexes[0].X + _vertexes[7].X) / 2;
            var centerY = (_vertexes[0].Y + _vertexes[7].Y) / 2;
            var centerZ = (_vertexes[0].Z + _vertexes[7].Z) / 2;
            _center.X = centerX;
            _center.Y = centerY;
            _center.Z = centerZ;

            Size = Math.Sqrt(
                Math.Pow(_vertexes[0].X - _vertexes[1].X, 2) +
                Math.Pow(_vertexes[0].Y - _vertexes[1].Y, 2) +
                Math.Pow(_vertexes[0].Z - _vertexes[1].Z, 2));
        }

        private void SetCubeFaces()
        {
            _faces[0].SetPoints(GetVertexes(0, 1, 3, 2));
            _faces[1].SetPoints(GetVertexes(0, 1, 5, 4));
            _faces[2].SetPoints(GetVertexes(0, 2, 6, 4));
            _faces[3].SetPoints(GetVertexes(1, 3, 7, 5));
            _faces[4].SetPoints(GetVertexes(2, 3, 7, 6));
            _faces[5].SetPoints(GetVertexes(4, 5, 7, 6));
        }

        private Point3D[] GetVertexes(int a, int b, int c, int d)
        {
            return new[]
            {
                _vertexes[a], _vertexes[b], _vertexes[c], _vertexes[d]
            };
        }

        public Point3D Center()
        {
            return _center;
        }

    }
}
