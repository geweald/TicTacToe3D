using System.Windows;
using System.Windows.Controls;

namespace TicTacToe3D.Controls
{
    public partial class InstructionControl : UserControl
    {
        public InstructionControl()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Collapsed;
        }
    }
}
