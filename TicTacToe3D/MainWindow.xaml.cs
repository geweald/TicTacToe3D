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

        public MainWindow()
        {
            _gamePage = new GamePage();

            InitializeComponent();
        }

        private void MainFrame_OnContentRendered(object sender, EventArgs e)
        {
            MainFrame.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(_gamePage);
        }
    }
}
