using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using ImageLib;

namespace ImageApp.Directory.ViewModels
{
    public class ImageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        private string _mTimer = "None";
        public string Timer
        {
            get => "Last process time: " + _mTimer;
            set
            {
                if (_mTimer == value)
                    return;
                _mTimer = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Timer)));
            }
        }

        private BitmapSource _mImage;
        public BitmapSource Image
        {
            get => _mImage;
            set
            {
                if (_mImage == value)
                    return;
                _mImage = value;
                SetCanExecuteToProcessCommands(true);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Image)));
            }
        }

        public ICommand SyncProcessImageCommand { get; set; }
        public ICommand AsyncProcessImageCommand { get; set; }
        public ICommand LoadImageFromFileCommand { get; set; }
        public ICommand SaveImageToFileCommand { get; set; }

        public ImageViewModel()
        {
            SyncProcessImageCommand = new RelayCommand(SyncProcess);
            AsyncProcessImageCommand = new RelayCommand(AsyncProcess);
            LoadImageFromFileCommand = new RelayCommand(GetImageFromFile);
            SaveImageToFileCommand = new RelayCommand(PostImageToFile);
            SetCanExecuteToProcessCommands(false);
        }

        public async void AsyncProcess()
        {
            var temp = new ImageProcessing(Image);
            SetCanExecuteToAllCommands(false);
            Timer = "Processing...";
            var watch = System.Diagnostics.Stopwatch.StartNew();
            await temp.ToMainColorsAsync();
            watch.Stop();
            Image.Dispatcher.Invoke(() =>
            {
                Timer = watch.ElapsedMilliseconds + " ms";
                Image = temp.GetImage();
                SetCanExecuteToAllCommands(true);
            });
        }

        public void SyncProcess()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            Image = new ImageProcessing(Image).ToMainColors();
            watch.Stop();
            Timer = watch.ElapsedMilliseconds + " ms";
        }

        private void GetImageFromFile()
        {
            var temp = FileStructure.GetImageFromFile();
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
            LoadImageFromFileCommand.Execute(setTo);
        }

        private void SetCanExecuteToProcessCommands(bool setTo)
        {
            AsyncProcessImageCommand.Execute(setTo);
            SyncProcessImageCommand.Execute(setTo);
            SaveImageToFileCommand.Execute(setTo);
        }
    }
}
