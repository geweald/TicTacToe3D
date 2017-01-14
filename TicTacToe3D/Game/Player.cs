using System.Collections.Generic;
using System.Windows.Media;

namespace TicTacToe3D.Game
{
    class Player
    {
        private bool _isComputer;
        private readonly List<int> _markedFields;

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

        public Player(string name, SolidColorBrush color)
        {
            Name = name;
            Color = color;
            color.Freeze();
            _markedFields = new List<int>();
        }

        public int[] MarkedFields()
        {
            return _markedFields.ToArray();
        }

        public void MarkField(int nr)
        {
            _markedFields.Add(nr);
        }

        public void ClearFields()
        {
            _markedFields.Clear();
        }
    }
}
