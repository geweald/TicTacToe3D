using System.Windows.Media.Media3D;

namespace TicTacToe3D.Game
{
    class Face
    {
        private Point3D[] _points;
        public double MaxZ { get; private set; }

        public Face(Point3D[] points = null)
        {
            SetPoints(points);
            FindMaxZ();
        }

        public void SetPoints(Point3D[] points)
        {
            _points = points;
            FindMaxZ();
        }

        public Point3D[] Points()
        {
            return _points;
        }

        private void FindMaxZ()
        {
            if (_points == null) return;
            double maxz = _points[0].Z;
            for (int i = 1; i < _points.Length; i++)
            {
                if (_points[i].Z > maxz) maxz = _points[i].Z;
            }
            MaxZ = maxz;
        }
    }
}
