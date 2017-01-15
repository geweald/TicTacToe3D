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
            Instruction.Visibility = Visibility.Collapsed;
            _timer.Start();
        }

        private void StartPage_OnUnloaded(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
        }

        private void InstructionButton_OnClick(object sender, RoutedEventArgs e)
        {
            Instruction.Visibility = Visibility.Visible;
        }

        private void Game1PlayerButton_OnClick(object sender, RoutedEventArgs e)
        {
            StartGame();
        }

        private void Game2PlayersButton_OnClick(object sender, RoutedEventArgs e)
        {
            StartGame(false);
        }

        private void StartGame(bool withComputer = true)
        {
            if (Cube4.IsChecked.HasValue && Cube4.IsChecked.Value)
            {
                _mainWinow?.NavigateToGamePage(4, withComputer);
            }
            else if (Cube5.IsChecked.HasValue && Cube5.IsChecked.Value)
            {
                _mainWinow?.NavigateToGamePage(5, withComputer);
            }
            else
            {
                _mainWinow?.NavigateToGamePage(3, withComputer);
            }
        }
    }
}
