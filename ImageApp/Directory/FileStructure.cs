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
                MessageBox.Show("Invalid File Type", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }
        public static void SaveImageToFile(BitmapSource file)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Image files (*.jpg; *.png; *.bmp)|*.png;*.jpeg|All files (*.*)|*.*"
            };
            saveFileDialog.ShowDialog();
            var encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(file));
            using (var stream = saveFileDialog.OpenFile())
            {
                encoder.Save(stream);
            }
        }
    }
}
