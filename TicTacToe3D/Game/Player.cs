using System.Collections.Generic;
using System.Windows.Media;

namespace TicTacToe3D.Game
{
    class Player
    {
        public string Name { get; private set; }
        public SolidColorBrush Color { get; private set; }
        public List<int> MarkedFields;

        public Player(string name, SolidColorBrush color)
        {
            Name = name;
            Color = color;
            color.Freeze();
            MarkedFields = new List<int>();
        }


        public void MarkedField(int field)
        {
            MarkedFields.Add(field);
        }

        public void ClearFields()
        {
            MarkedFields.Clear();
        }
    }
}
