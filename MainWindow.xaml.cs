using System.Windows;
using Client_WPF.ViewModels;

namespace Client_WPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = new ApplicationViewModel();
        }
    }
}
