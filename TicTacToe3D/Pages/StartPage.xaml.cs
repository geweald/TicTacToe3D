using System.Windows;
using System.Windows.Controls;

namespace TicTacToe3D.Pages
{
    /// <summary>
    /// Interaction logic for StartPage.xaml
    /// </summary>
    public partial class StartPage : Page
    {
        private readonly MainWindow _mainWinow;

        public StartPage()
        {
            _mainWinow = Application.Current.MainWindow as MainWindow;
            InitializeComponent();
        }

        private void Game3_OnClick(object sender, RoutedEventArgs e)
        {
            _mainWinow?.NavigateToGamePage(2, 3);
        }

        private void Game5_OnClick(object sender, RoutedEventArgs e)
        {
            _mainWinow?.NavigateToGamePage(2, 5);
        }
    }
}
