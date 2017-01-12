using System;
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
        private readonly int _size;
        private int _moves;
        private bool _over;

        public Game(ushort size, Canvas canvas)
        {
            _players = new List<Player>
            {
                new Player("Janek", new SolidColorBrush(Color.FromArgb(255, 255, 255, 255))),
                new Player("Marek", new SolidColorBrush(Color.FromArgb(255, 0, 0, 0))),
            };
            _players[1].IsComputer = true;
            _moves = 0;
            _size = size;
            _fields = size * size * size;

            GameBoard = new GameBoard(size, canvas);
        }


        public void Start()
        {
            GameBoard.Clear();
            _over = false;
        }

        public void PlayWithComputer(bool? withComputer = true)
        {
            _players[1].IsComputer = withComputer == null || withComputer.Value;
        }


        public void MakeMove()
        {
            if (_over) return;
            var player = _players[_moves % _players.Count];
            if (!MakedMove(player, GameBoard.HighlightedField)) return;
            var nextPlayer = _players[_moves % _players.Count];
            if (nextPlayer.IsComputer) MakeRandomMove(nextPlayer);
        }

        private bool MakedMove(Player player, int field)
        {
            var success = GameBoard.MarkField(field, player.Color);
            if (!success) return false;
            _moves++;
            player.MarkedFields.Add(field);

            if (CheckWin(player.MarkedFields))
            {
                _over = true;
                MessageBox.Show("Wyrgywa gracz " + player.Name);
            }
            return true;
        }

        private void MakeRandomMove(Player player)
        {
            if (_over) return;
            var fields = _size * _size * _size;
            var rnd = new Random();
            var field = rnd.Next(fields);
            for (int i = 0; i < fields; i++)
            {
                if (!MakedMove(player, field)) field = (field + 1) % fields;
                else return;
            }
        }

        private bool CheckWin(List<int> playerMarkedFields)
        {
            var last = playerMarkedFields[playerMarkedFields.Count - 1];
            var layerFields = _size * _size;
            var lastLayer = last / layerFields;
            foreach (var playerMarkedField in playerMarkedFields)
            {
                Console.Write(playerMarkedField + " ");
            }
            Console.WriteLine();
            // column in layer
            var result = 0;
            for (int i = 0; i < _size; i++)
            {
                var f = last % _size + i * _size;
                f += lastLayer * layerFields;
                //Console.WriteLine("kol in layer " + f);
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
                Console.WriteLine("row in layer " + f);
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
                //Console.WriteLine("diag in layer " + f);
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
                //Console.WriteLine("antidiag in layer " + f);
                if (playerMarkedFields.Contains(f)) result++;
                else break;
            }
            if (result == _size) return true;

            // row between layers
            result = 0;
            for (int i = 0; i < _size; i++)
            {
                var f = last % layerFields + i * layerFields;
                //Console.WriteLine("row between " + f);
                if (playerMarkedFields.Contains(f)) result++;
                else break;
            }
            if (result == _size) return true;

            // antidiag between layers up
            result = 0;
            for (int i = 0; i < _size; i++)
            {
                var f = last % (layerFields + _size) + i * (layerFields + _size);
                //Console.WriteLine("antidiag between up " + f);
                if (playerMarkedFields.Contains(f)) result++;
                else break;
            }
            if (result == _size) return true;

            // diag betwwen layers up
            result = 0;
            for (int i = 0; i < _size; i++)
            {
                var f = last % (layerFields - _size) + (i + 1) * (layerFields - _size);
                //Console.WriteLine("diag between up " + f);
                if (playerMarkedFields.Contains(f)) result++;
                else break;
            }
            if (result == _size) return true;

            // antidiag between layers flat
            result = 0;
            for (int i = 0; i < _size; i++)
            {
                var f = last % (layerFields + 1) + i * (layerFields + 1);
                //Console.WriteLine("antidiag between flat " + f);
                if (playerMarkedFields.Contains(f)) result++;
                else break;
            }
            if (result == _size) return true;

            // diag between layers flat
            result = 0;
            for (int i = 0; i < _size; i++)
            {
                var f = last % (layerFields - 1) + i * (layerFields - 1);
                //Console.WriteLine("diag between flat " + f);
                if (playerMarkedFields.Contains(f)) result++;
                else break;
            }
            if (result == _size) return true;

            // cross down 1
            result = 0;
            for (int i = 0; i < _size; i++)
            {
                var f = i * (layerFields + _size + 1);
                //Console.WriteLine("cross down1 " + f);
                if (playerMarkedFields.Contains(f)) result++;
                else break;
            }
            if (result == _size) return true;

            // cross down 2
            result = 0;
            for (int i = 0; i < _size; i++)
            {
                var f = _size - 1 + i * (layerFields + _size - 1);
                //Console.WriteLine("cross down2 " + f);
                if (playerMarkedFields.Contains(f)) result++;
                else break;
            }
            if (result == _size) return true;

            // cross up 1
            result = 0;
            for (int i = 0; i < _size; i++)
            {
                var f = layerFields - _size + 1 + i * (layerFields - _size + 1);
                //Console.WriteLine("cross up1 " + f);
                if (playerMarkedFields.Contains(f)) result++;
                else break;
            }
            if (result == _size) return true;

            // cross up 2
            result = 0;
            for (int i = 0; i < _size; i++)
            {
                var f = layerFields - 1 + i * (layerFields - _size - 1);
                //Console.WriteLine("cross up2 " + f);
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

        //TODO
        public void Restart()
        {
            foreach (var p in _players)
            {
                p.MarkedFields.Clear();
            }
            _moves = 0;
            Start();
        }

    }
}
