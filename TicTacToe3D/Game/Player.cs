using System.Collections.Generic;
using System.Windows.Media;

namespace TicTacToe3D.Game
{
    class Player
    {
        private bool _isComputer;
        public string Name { get; private set; }
        public SolidColorBrush Color { get; private set; }

        public bool IsComputer
        {
            get { return _isComputer; }
            set
            {
                Name = value ? "Computer" : "Black Player";
                _isComputer = value;
            }
        }

        public List<int> MarkedFields;

        public Player(string name, SolidColorBrush color)
        {
            Name = name;
            Color = color;
            color.Freeze();
            MarkedFields = new List<int>();
        }
    }
}
