using System;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace ImageApp.Directory
{
    public static class FileStructure
    {
        public static BitmapSource GetImageFromFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg; *.png; *.bmp)|*.png;*.jpeg|All files (*.*)|*.*"
            };
            try
            {
                openFileDialog.ShowDialog();
                return new BitmapImage(new Uri(openFileDialog.FileName));
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid File Exception", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }
    }
}
