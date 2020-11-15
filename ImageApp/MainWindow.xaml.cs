using System.Windows;
using ImageApp.Directory.ViewModels;

namespace ImageApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            DataContext = new ImageViewModel();
        }
    }
}
