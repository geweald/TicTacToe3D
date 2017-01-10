using System.Windows.Media;

namespace TicTacToe3D.Game
{
    class GameField
    {
        public Cube Cube { get; }
        public int Layer { get; }
        public int FieldNr { get; }
        public bool Marked { get; private set; }
        public SolidColorBrush PlayerColor { get; private set; }

        public GameField(double[] cubePoints, int layer, int field)
        {
            Cube = new Cube(cubePoints[0], cubePoints[1], cubePoints[2]);
            Layer = layer;
            FieldNr = field;
        }

        public bool Mark(SolidColorBrush playerColor)
        {
            if (Marked) return false;
            Marked = true;
            PlayerColor = playerColor;
            return true;
        }

        public void Clear()
        {
            Marked = false;
            PlayerColor = null;
        }
    }
}
