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

        public String mTimer = "None";
        public String Timer
        {
            get { return "Last process time: " + mTimer; }
            set
            {
                if (mTimer == value)
                    return;
                mTimer = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Timer)));
            }
        }

        public BitmapSource mImage;
        public BitmapSource Image
        {
            get { return mImage; }
            set
            {
                if (mImage == value)
                    return;
                mImage = value;
                SetCanExecuteToProcessCommands(true);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Image)));
            }
        }

        public ICommand SyncProcessImageCommand { get; set; }
        public ICommand AsyncProcessImageCommand { get; set; }
        public ICommand LoadImageFromFile { get; set; }
        public ICommand SaveImageToFile { get; set; }

        public ImageViewModel()
        {
            SyncProcessImageCommand = new RelayCommand(SyncProcess);
            AsyncProcessImageCommand = new RelayCommand(AsyncProcess);
            LoadImageFromFile = new RelayCommand(GetImageFromFile);
            SaveImageToFile = new RelayCommand(PostImageToFile);
            SetCanExecuteToProcessCommands(false);
        }

        private void AsyncProcess()
        {
            var temp = new ImageProcessing(Image);
            SetCanExecuteToAllCommands(false);
            Timer = "Processing...";
            Task.Run(async () =>
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                await temp.ToMainColorsAsync();
                watch.Stop();
                Image.Dispatcher.Invoke(() =>
                {
                    Timer = watch.ElapsedMilliseconds + " ms";
                    Image = temp.GetImage();
                    SetCanExecuteToAllCommands(true);
                });
            });
        }

        private void SyncProcess()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            Image = new ImageProcessing(Image).ToMainColors();
            watch.Stop();
            Timer = watch.ElapsedMilliseconds + " ms";
        }

        private void GetImageFromFile()
        {
            BitmapSource temp = FileStructure.GetImageFromFile();
            if (temp != null)
                Image = temp;
        }

        private void PostImageToFile()
        {
            FileStructure.SaveImageToFile(Image);
        }

        private void SetCanExecuteToAllCommands(bool setTo)
        {
            SetCanExecuteToProcessCommands(setTo);
            LoadImageFromFile.Execute(setTo);
        }

        private void SetCanExecuteToProcessCommands(bool setTo)
        {
            AsyncProcessImageCommand.Execute(setTo);
            SyncProcessImageCommand.Execute(setTo);
        }
    }
}
