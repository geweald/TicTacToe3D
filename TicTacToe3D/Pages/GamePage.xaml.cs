﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TicTacToe3D.Game;

namespace TicTacToe3D.Pages
{
    public partial class GamePage : Page
    {
        public delegate void MakeMoveUpdateDelegate();

        private Game.Game _game;
        private Point _mousePos;
        private ushort _size;
        private bool _withComputer;
        private readonly MakeMoveUpdateDelegate _updateAfterMoveDelegate;


        public GamePage()
        {
            InitializeComponent();

            _updateAfterMoveDelegate += UpdateNameAndColor;
            _updateAfterMoveDelegate += UpdateResultMessage;
        }

        private void GamePage_OnLoaded(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.KeyDown += GamePage_OnKeyDown;
            Application.Current.MainWindow.SizeChanged += MainWindowOnSizeChanged;
            _game = new Game.Game(_size, GameCanvas, _updateAfterMoveDelegate);
            _game.PlayWithComputer(_withComputer);
            _game.Start();

            ResultMessage.Visibility = Visibility.Collapsed;
        }

        private void UpdateResultMessage()
        {
            if (_game.GameState == GameState.Draw)
            {
                ResultMessage.Message = "DRAW!";
                ResultMessage.Visibility = Visibility.Visible;
            }
            else if (_game.GameState == GameState.Win)
            {
                ResultMessage.Message = $"{_game.NextPlayer().Name} WON!";
                ResultMessage.Visibility = Visibility.Visible;
            }
        }

        private void UpdateNameAndColor()
        {
            if (_game.GameState == GameState.Running)
            {
                PlayerColorRectangle.Fill = _game.NextPlayer().Color;
                PlayerName.Text = _game.NextPlayer().Name;
            }

        }

        private void MainWindowOnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            _game.GameBoard.DrawGameBoard();
        }

        private void GamePage_OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.PageDown:
                    _game.GameBoard.ChangeLayer();
                    break;
                case Key.PageUp:
                    _game.GameBoard.ChangeLayer(false);
                    break;
                case Key.Up:
                    _game.GameBoard.ChangeField(1);
                    break;
                case Key.Down:
                    _game.GameBoard.ChangeField(3);
                    break;
                case Key.Left:
                    _game.GameBoard.ChangeField(0);
                    break;
                case Key.Right:
                    _game.GameBoard.ChangeField(2);
                    break;
                case Key.Space:
                    _game.MakeMove();
                    break;
                case Key.Add:
                    _game.GameBoard.Zoom(0.3);
                    break;
                case Key.Subtract:
                    _game.GameBoard.Zoom(-0.3);
                    break;
                case Key.X:
                    _game.GameBoard.RotateX(1);
                    break;
                case Key.Z:
                    _game.GameBoard.RotateX(-1);
                    break;
                case Key.C:
                    _game.GameBoard.RotateY(1);
                    break;
                case Key.V:
                    _game.GameBoard.RotateY(-1);
                    break;
                case Key.B:
                    _game.GameBoard.RotateZ(2);
                    break;
                case Key.N:
                    _game.GameBoard.RotateZ(-2);
                    break;
            }
        }

        private void GamePage_OnUnloaded(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.KeyDown -= GamePage_OnKeyDown;
            Application.Current.MainWindow.SizeChanged -= MainWindowOnSizeChanged;
        }

        private void GameCanvas_OnMouseMove(object sender, MouseEventArgs e)
        {
            var point = e.GetPosition(GameCanvas);
            if (e.RightButton != MouseButtonState.Pressed)
            {
                _mousePos = point;
                return;
            }
            var x = (point.Y - _mousePos.Y) / -8;
            var y = (point.X - _mousePos.X) / 8;
            _game.GameBoard.RotateXY(x, y);
            _mousePos = point;
        }

        private void GameCanvas_OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
                _game.GameBoard.Zoom(e.Delta / 100.0);
            else
                _game.GameBoard.ChangeLayer(e.Delta > 0);
        }

        private void GameCanvas_OnMouseEnter(object sender, MouseEventArgs e)
        {
            _mousePos = e.GetPosition(GameCanvas);
        }

        public void SetGameSettings(ushort size, bool computer = true)
        {
            _size = size;
            _withComputer = computer;
        }

        private void BackToMenu_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }

        private void ResultMessage_OnYesClicked(object sender, EventArgs e)
        {
            _game.Restart();
        }
    }
}
