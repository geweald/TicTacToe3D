using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace TicTacToe3D.Game
{
    class Cube
    {
        private Point3D[] _vertexes;
        private readonly Face[] _faces;

        public Point3D Center { get; private set; }


        public Cube(double x, double y, double z)
        {
            _faces = new[]
            {
                new Face(), new Face(), new Face(), new Face(), new Face(), new Face()
            };
            MakeCubeVertexes(x, y, z);
            SetCubeFaces();
        }

        public void Transform(Point3D[] transformedVertexes)
        {
            _vertexes = transformedVertexes;
            CalculateCenter();
            SetCubeFaces();
        }

        public Point3D[] CubeVertexes()
        {
            return _vertexes;
        }

        public List<Face> CubeFaces()
        {
            var result = new List<Face>();
            foreach (var face in _faces)
                result.Add(face);
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

            CalculateCenter();
        }

        private void CalculateCenter()
        {
            var centerX = (_vertexes[0].X + _vertexes[7].X) / 2;
            var centerY = (_vertexes[0].Y + _vertexes[7].Y) / 2;
            var centerZ = (_vertexes[0].Z + _vertexes[7].Z) / 2;
            Center = new Point3D
            {
                X = centerX,
                Y = centerY,
                Z = centerZ
            };
        }

        private void SetCubeFaces()
        {
            _faces[0].SetPoints(Vertexes(0, 1, 3, 2), Center);
            _faces[1].SetPoints(Vertexes(0, 1, 5, 4), Center);
            _faces[2].SetPoints(Vertexes(0, 2, 6, 4), Center);
            _faces[3].SetPoints(Vertexes(1, 3, 7, 5), Center);
            _faces[4].SetPoints(Vertexes(2, 3, 7, 6), Center);
            _faces[5].SetPoints(Vertexes(4, 5, 7, 6), Center);
        }

        private Point3D[] Vertexes(int a, int b, int c, int d)
        {
            return new[]
            {
                _vertexes[a], _vertexes[b], _vertexes[c], _vertexes[d]
            };
        }
    }
}
