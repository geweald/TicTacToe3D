using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using TicTacToe3D.Pages;

namespace TicTacToe3D.Game
{
    enum GameState
    {
        Running, Win, Draw
    }

    class Game
    {
        private readonly List<Player> _players;
        private readonly int _fields;
        private readonly int _size;
        private int _moves;

        public GameState GameState { get; private set; }
        public GameBoard GameBoard { get; }
        private readonly GamePage.MakeMoveUpdateDelegate _moveUpdateDelegate;

        public Game(ushort size, Canvas canvas, GamePage.MakeMoveUpdateDelegate moveUpdate)
        {
            _players = new List<Player>
            {
                new Player("White Player", new SolidColorBrush(Color.FromArgb(175, 255, 255, 255))),
                new Player("Red Player", new SolidColorBrush(Color.FromArgb(175, 0, 0, 0))),
            };
            _moves = 0;
            _size = size;
            _fields = size * size * size;

            GameState = GameState.Running;
            GameBoard = new GameBoard(size, canvas, this);
            _moveUpdateDelegate = moveUpdate;
        }


        public void Start()
        {
            _moveUpdateDelegate();
            GameState = GameState.Running;
            GameBoard.DrawGameBoard();
        }

        public void Restart()
        {
            foreach (var p in _players)
            {
                p.ClearFields();
            }
            _moves = 0;
            GameBoard.Clear();
            Start();
        }

        public void PlayWithComputer(bool withComputer = true)
        {
            _players[1].IsComputer = withComputer;
        }

        public void MakeMove()
        {
            if (GameState != GameState.Running) return;

            var player = NextPlayer();
            if (!MakeNextMove(player, GameBoard.HighlightedField))
                return;

            var nextPlayer = NextPlayer();

            if (GameState == GameState.Running && nextPlayer.IsComputer)
                RandomMove(nextPlayer);
        }

        public Player NextPlayer()
        {
            return _players[_moves % _players.Count];
        }


        private bool MakeNextMove(Player player, int field)
        {
            var success = GameBoard.MarkField(field, player.Color);
            if (!success) return false;
            player.MarkField(field);

            if (CheckWin(player.MarkedFields()))
            {
                GameState = _moves >= _fields ? GameState.Draw : GameState.Win;
            }
            else
            {
                _moves++;
            }
            _moveUpdateDelegate();
            return true;
        }

        private void RandomMove(Player player)
        {
            var fields = _size * _size * _size;
            var rnd = new Random();
            var field = rnd.Next(fields);
            for (int i = 0; i < fields; i++)
            {
                if (!MakeNextMove(player, field)) field = (field + 1) % fields;
                else return;
            }
        }

        private bool CheckWin(IList<int> playerMarkedFields)
        {
            var last = playerMarkedFields[playerMarkedFields.Count - 1];
            var layerFields = _size * _size;
            var lastLayer = last / layerFields;

            // column in layer
            var result = 0;
            for (int i = 0; i < _size; i++)
            {
                var f = last % _size + i * _size;
                f += lastLayer * layerFields;
                if (playerMarkedFields.Contains(f)) result++;
                else break;
            }
            if (result == _size) return true;

            // row in layer
            result = 0;
            for (int i = 0; i < _size; i++)
            {
                var f = last / _size % _size * _size + i;
                f += lastLayer * layerFields;
                if (playerMarkedFields.Contains(f)) result++;
                else break;
            }
            if (result == _size) return true;

            // diagonal in layer
            result = 0;
            for (int i = 0; i < _size; i++)
            {
                var f = i * (_size + 1);
                f += lastLayer * layerFields;
                if (playerMarkedFields.Contains(f)) result++;
                else break;
            }
            if (result == _size) return true;

            // antidiag in layer
            result = 0;
            for (int i = 0; i < _size; i++)
            {
                var f = _size - 1 + i * (_size - 1);
                f += lastLayer * layerFields;
                if (playerMarkedFields.Contains(f)) result++;
                else break;
            }
            if (result == _size) return true;

            // row between layers
            result = 0;
            for (int i = 0; i < _size; i++)
            {
                var f = last % layerFields + i * layerFields;
                if (playerMarkedFields.Contains(f)) result++;
                else break;
            }
            if (result == _size) return true;

            // antidiag between layers up
            result = 0;
            for (int i = 0; i < _size; i++)
            {
                var f = last % (layerFields + _size) + i * (layerFields + _size);
                if (playerMarkedFields.Contains(f)) result++;
                else break;
            }
            if (result == _size) return true;

            // diag betwwen layers up
            result = 0;
            for (int i = 0; i < _size; i++)
            {
                var f = last % (layerFields - _size) + (i + 1) * (layerFields - _size);
                if (playerMarkedFields.Contains(f)) result++;
                else break;
            }
            if (result == _size) return true;

            // antidiag between layers flat
            result = 0;
            for (int i = 0; i < _size; i++)
            {
                var f = last % (layerFields + 1) + i * (layerFields + 1);
                if (playerMarkedFields.Contains(f)) result++;
                else break;
            }
            if (result == _size) return true;

            // diag between layers flat
            result = 0;
            for (int i = 0; i < _size; i++)
            {
                var f = last % (layerFields - 1) + i * (layerFields - 1);
                if (playerMarkedFields.Contains(f)) result++;
                else break;
            }
            if (result == _size) return true;

            // cross down 1
            result = 0;
            for (int i = 0; i < _size; i++)
            {
                var f = i * (layerFields + _size + 1);
                if (playerMarkedFields.Contains(f)) result++;
                else break;
            }
            if (result == _size) return true;

            // cross down 2
            result = 0;
            for (int i = 0; i < _size; i++)
            {
                var f = _size - 1 + i * (layerFields + _size - 1);
                if (playerMarkedFields.Contains(f)) result++;
                else break;
            }
            if (result == _size) return true;

            // cross up 1
            result = 0;
            for (int i = 0; i < _size; i++)
            {
                var f = layerFields - _size + 1 + i * (layerFields - _size + 1);
                if (playerMarkedFields.Contains(f)) result++;
                else break;
            }
            if (result == _size) return true;

            // cross up 2
            result = 0;
            for (int i = 0; i < _size; i++)
            {
                var f = layerFields - 1 + i * (layerFields - _size - 1);
                if (playerMarkedFields.Contains(f)) result++;
                else break;
            }
            if (result == _size) return true;


            if (_moves >= _fields)
            {
                return true;
            }
            return false;
        }


    }
}
