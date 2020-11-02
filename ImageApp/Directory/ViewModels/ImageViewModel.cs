using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using ImageLib;

namespace ImageApp.Directory.ViewModels
{
    public class ImageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        public BitmapSource mImage;
        public BitmapSource Image
        {
            get { return mImage; }
            set
            {
                if (mImage == value)
                    return;
                mImage = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Image)));
            }
        }

        public ICommand SyncProcessImageCommand { get; set; }
        public ICommand AsyncProcessImageCommand { get; set; }
        public ICommand LoadImageFromFile { get; set; }

        public ImageViewModel()
        {
            SyncProcessImageCommand = new RelayCommand(SyncProcess);
            AsyncProcessImageCommand = new RelayCommand(AsyncProcess);
            LoadImageFromFile = new RelayCommand(GetImageFromFile);
        }

        private void AsyncProcess()
        {
            var temp = new ImageProcessing(Image);
            SetCanExecuteToAllCommands(false);
            Task.Run(async () =>
            {
                await temp.ToMainColorsAsync();
                Image.Dispatcher.Invoke(() =>
                {
                    Image = temp.GetResult();
                    SetCanExecuteToAllCommands(true);
                });
            });
        }

        private void SyncProcess()
        {
            Image = new ImageProcessing(Image).ToMainColors();
        }

        private void GetImageFromFile()
        {
            Image = FileStructure.GetImageFromFile();
        }

        private void SetCanExecuteToAllCommands(bool setTo)
        {
            AsyncProcessImageCommand.Execute(setTo);
            SyncProcessImageCommand.Execute(setTo);
            LoadImageFromFile.Execute(setTo);
        }
    }
}
