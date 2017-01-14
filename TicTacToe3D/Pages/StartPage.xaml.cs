using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using TicTacToe3D.Game;

namespace TicTacToe3D.Pages
{
    public partial class StartPage : Page
    {
        private readonly MainWindow _mainWinow;
        private readonly DispatcherTimer _timer;
        private readonly GameBoard _gameBoard;

        public StartPage()
        {
            InitializeComponent();

            _mainWinow = Application.Current.MainWindow as MainWindow;

            _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(30) };
            _timer.Tick += (sender, args) => _gameBoard?.RotateXY(0.25, 0.25);

            _gameBoard = new GameBoard(3, AnimationCanvas);
        }

        private void StartPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            _timer.Start();
        }

        private void StartPage_OnUnloaded(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
        }

        private void Game3p1_OnClick(object sender, RoutedEventArgs e)
        {
            _mainWinow?.NavigateToGamePage(3);
        }

        private void Game3p2_OnClick(object sender, RoutedEventArgs e)
        {
            _mainWinow?.NavigateToGamePage(3, false);
        }

        private void Game5p1_OnClick(object sender, RoutedEventArgs e)
        {
            _mainWinow?.NavigateToGamePage(5);
        }

        private void Game5p2_OnClick(object sender, RoutedEventArgs e)
        {
            _mainWinow?.NavigateToGamePage(5, false);
        }
    }
}
