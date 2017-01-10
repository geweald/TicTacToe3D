using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TicTacToe3D.Game
{
    class Game
    {
        private readonly List<Player> _players;
        public GameBoard GameBoard { get; }
        private readonly int _fields;
        private int _moves;

        public Game(ushort size, Canvas canvas)
        {
            _players = new List<Player>
            {
                new Player("Janek", new SolidColorBrush(Color.FromArgb(255, 255, 255, 255))),
                new Player("Marek", new SolidColorBrush(Color.FromArgb(255, 0, 0, 0)))
            };
            _moves = 0;
            _fields = size * size * size;
            GameBoard = new GameBoard(size, canvas);
        }


        public void Start()
        {
            GameBoard.DrawGameBoard();
        }


        public void MakeMove()
        {
            var player = _players[_moves % _players.Count];
            bool success = GameBoard.MarkHighlighted(player.Color);
            if (!success) return;

            _moves++;
            player.MarkedField(GameBoard.HighlightedField);

            if (CheckWin())
            {
                MessageBox.Show("remis");
                GameBoard.Clear();
                foreach (var player1 in _players)
                {
                    player1.ClearFields();
                }
                //TODO
            }
        }

        //TODO
        private bool CheckWin()
        {
            if (_moves >= _fields)
            {
                return true;
            }
            return false;
        }

        //TODO
        public void Restart()
        {

        }


    }
}
