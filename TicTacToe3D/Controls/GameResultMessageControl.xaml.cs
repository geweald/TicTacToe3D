using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace TicTacToe3D.Controls
{
    public partial class GameResultMessageControl : UserControl, INotifyPropertyChanged
    {
        public GameResultMessageControl()
        {
            InitializeComponent();
            DataContext = this;
        }

        public event EventHandler YesClicked;

        private string _msg;
        public string Message
        {
            get
            {
                return _msg;
            }
            set
            {
                _msg = value;
                OnPropertyChanged();
            }
        }

        private void YesButton_OnClick(object sender, RoutedEventArgs e)
        {
            YesClicked?.Invoke(this, EventArgs.Empty);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
