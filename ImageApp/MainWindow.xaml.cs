using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using ImageLib;

namespace ImageApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private Bitmap img;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ReloadImage(Bitmap input)
        {
            img = input;
            Image.Source = Imaging.CreateBitmapSourceFromHBitmap(img.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }

        private void LoadButton_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg; *.png; *.bmp)|*.png;*.jpeg|All files (*.*)|*.*"
            };
            try
            {
                openFileDialog.ShowDialog();
                string selectedFile = openFileDialog.FileName;
                ReloadImage(new Bitmap(selectedFile));
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid File Exception", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Async_OnClick(object sender, RoutedEventArgs e)
        {
            BackgroundWorker worker = new BackgroundWorker();
            ImageProcessing imageProcessing = new ImageProcessing(img);
            ProgressBar.Value = 0;
            ProgressBar.Visibility = Visibility.Visible;
            Image.Visibility = Visibility.Hidden;
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += imageProcessing.ToMainColorsAsync;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync();
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressBar.Value = e.ProgressPercentage;
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            img = (Bitmap) e.Result;
            ProgressBar.Visibility = Visibility.Hidden;
            Image.Visibility = Visibility.Visible;
            ReloadImage(img);
        }

        private void Sync_OnClick(object sender, RoutedEventArgs e)
        {
            ImageProcessing imageProcessing = new ImageProcessing(img);
            ReloadImage(imageProcessing.ToMainColors());
        }
    }
}
