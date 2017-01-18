using System.Windows.Media.Media3D;

namespace TicTacToe3D.Game
{
    internal class Face
    {
        private Point3D[] _points;
        private Point3D _center;
        private Vector3D _faceFront;

        public void SetPoints(Point3D[] points, Point3D cubeCenter)
        {
            _points = points;

            CalculateCenter();
            CalculateFaceFront(cubeCenter);
        }

        public Point3D[] Points()
        {
            return _points;
        }

        public bool IsVisible(double cameraZ)
        {
            var camFaceCenterVector = new Vector3D(-_center.X, -_center.Y, -cameraZ - _center.Z);
            var angle = Vector3D.AngleBetween(camFaceCenterVector, _faceFront);
            return angle < 90.0;
        }


        private void CalculateFaceFront(Point3D cubeCenter)
        {
            _faceFront.X = _center.X - cubeCenter.X;
            _faceFront.Y = _center.Y - cubeCenter.Y;
            _faceFront.Z = _center.Z - cubeCenter.Z;
        }

        private void CalculateCenter()
        {
            var centerX = (_points[0].X + _points[2].X) / 2;
            var centerY = (_points[0].Y + _points[2].Y) / 2;
            var centerZ = (_points[0].Z + _points[2].Z) / 2;
            _center.X = centerX;
            _center.Y = centerY;
            _center.Z = centerZ;
        }
    }
}
