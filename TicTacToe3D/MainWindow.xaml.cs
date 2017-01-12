using System;
using System.Windows;
using TicTacToe3D.Pages;

namespace TicTacToe3D
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly GamePage _gamePage;
        private readonly StartPage _startPage;

        public MainWindow()
        {
            _gamePage = new GamePage();
            _startPage = new StartPage();

            InitializeComponent();
        }

        private void MainFrame_OnContentRendered(object sender, EventArgs e)
        {
            MainFrame.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(_startPage);
        }

        public void NavigateToGamePage(int players, ushort size)
        {
            _gamePage.SetGameSettings(players, size);
            MainFrame.NavigationService.Navigate(_gamePage);
        }
    }
}
