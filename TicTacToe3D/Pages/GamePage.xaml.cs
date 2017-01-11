using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TicTacToe3D.Pages
{
    /// <summary>
    /// Interaction logic for GamePage.xaml
    /// </summary>
    public partial class GamePage : Page
    {
        private Game.Game _game;
        private Point _mousePos;

        public GamePage()
        {
            InitializeComponent();
        }

        private void GamePage_OnLoaded(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.KeyDown += GamePage_OnKeyDown;
            Application.Current.MainWindow.SizeChanged += MainWindowOnSizeChanged;

            _game = new Game.Game(5, GameCanvas);
            _game.GameBoard.Zoom(-7);
            _game.Start();
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
                    _game.GameBoard.Zoom(0.1);
                    break;
                case Key.Subtract:
                    _game.GameBoard.Zoom(-0.1);
                    break;
                case Key.X:
                    _game.GameBoard.RotateX(2);
                    break;
                case Key.Z:
                    _game.GameBoard.RotateX(-2);
                    break;
                case Key.C:
                    _game.GameBoard.RotateY(2);
                    break;
                case Key.V:
                    _game.GameBoard.RotateY(-2);
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

        private void StartButton_OnClick(object sender, RoutedEventArgs e)
        {
            _game.PlayWithComputer(ComputerCheckBox.IsChecked);
            _game.Restart();
        }

        private void RotationZLeftButton_OnClick(object sender, RoutedEventArgs e)
        {
            _game.GameBoard.RotateZ(2);
        }

        private void RotationZRightButton_OnClick(object sender, RoutedEventArgs e)
        {
            _game.GameBoard.RotateZ(-2);
        }

        private void RotationXTopButton_OnClick(object sender, RoutedEventArgs e)
        {
            _game.GameBoard.RotateX(2);
        }

        private void RotationYLeftButton_OnClick(object sender, RoutedEventArgs e)
        {
            _game.GameBoard.RotateY(-2);
        }

        private void RotationXDownButton_OnClick(object sender, RoutedEventArgs e)
        {
            _game.GameBoard.RotateX(-2);
        }

        private void RotationYRightRightButton_OnClick(object sender, RoutedEventArgs e)
        {
            _game.GameBoard.RotateY(2);
        }

        private void GameCanvas_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed) return;

            var point = e.GetPosition(GameCanvas);
            _game.GameBoard.RotateY((point.X - _mousePos.X) / 8);
            _game.GameBoard.RotateX((point.Y - _mousePos.Y) / -8);

            _mousePos = point;
        }

        private void GameCanvas_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            _mousePos = e.GetPosition(GameCanvas);
        }

        private void GameCanvas_OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            _game.GameBoard.Zoom(e.Delta / 100.0);
        }
    }
}
