using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using ImageApp.Directory.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nito.AsyncEx;

namespace ImageAppTests
{
    [TestClass]
    public class ViewModelTests
    {
        private ImageViewModel imageViewModel = new ImageViewModel();

        [TestMethod]
        public void SyncProcessImageCommandCanNotBeExecutedAtStart()
        {
            Assert.IsFalse(imageViewModel.SyncProcessImageCommand.CanExecute(null));
        }

        [TestMethod]
        public void AsyncProcessImageCommandCanNotBeExecutedAtStart()
        {
            Assert.IsFalse(imageViewModel.AsyncProcessImageCommand.CanExecute(null));
        }

        [TestMethod]
        public void SaveImageToFileCommandCanNotBeExecutedAtStart()
        {
            Assert.IsFalse(imageViewModel.SaveImageToFileCommand.CanExecute(null));
        }

        [TestMethod]
        public void LoadImageFromFileCommandCanBeExecutedAtStart()
        {
            Assert.IsTrue(imageViewModel.LoadImageFromFileCommand.CanExecute(null));
        }

        [TestMethod]
        public void SyncProcessImageCommandCanBeExecutedAfterFileLoad()
        {
            imageViewModel.Image = new BitmapImage();
            Assert.IsTrue(imageViewModel.SyncProcessImageCommand.CanExecute(null));
        }

        [TestMethod]
        public void AsyncProcessImageCommandCanBeExecutedAfterFileLoad()
        {
            imageViewModel.Image = new BitmapImage();
            Assert.IsTrue(imageViewModel.AsyncProcessImageCommand.CanExecute(null));
        }

        [TestMethod]
        public void SaveImageToFileCommandCanBeExecutedAfterFileLoad()
        {
            imageViewModel.Image = new BitmapImage();
            Assert.IsTrue(imageViewModel.SaveImageToFileCommand.CanExecute(null));
        }

        [TestMethod]
        public void TimerMessageOnStartIsNone()
        {
            Assert.AreEqual("Last process time: None", imageViewModel.Timer);
        }

        [TestMethod]
        public void SyncProcessRedIsCorrect()
        {
            var temp = new Bitmap(1,1);
            temp.SetPixel(0,0, Color.DarkRed);

            imageViewModel.Image = Imaging.CreateBitmapSourceFromHBitmap(temp.GetHbitmap(), IntPtr.Zero,
                Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            imageViewModel.SyncProcess();

            var width = imageViewModel.Image.PixelWidth;
            var height = imageViewModel.Image.PixelHeight;
            var stride = width * ((imageViewModel.Image.Format.BitsPerPixel + 7) / 8);
            var memoryBlockPointer = Marshal.AllocHGlobal(height * stride);
            imageViewModel.Image.CopyPixels(new Int32Rect(0, 0, width, height), memoryBlockPointer, height * stride, stride);
            var bitmap = new Bitmap(width, height, stride, PixelFormat.Format32bppPArgb, memoryBlockPointer);

            Assert.AreEqual(Color.FromArgb(255,255,0,0), bitmap.GetPixel(0,0));
        }

        [TestMethod]
        public void SyncProcessBlueIsCorrect()
        {
            var temp = new Bitmap(1, 1);
            temp.SetPixel(0, 0, Color.LightBlue);

            imageViewModel.Image = Imaging.CreateBitmapSourceFromHBitmap(temp.GetHbitmap(), IntPtr.Zero,
                Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            imageViewModel.SyncProcess();

            var width = imageViewModel.Image.PixelWidth;
            var height = imageViewModel.Image.PixelHeight;
            var stride = width * ((imageViewModel.Image.Format.BitsPerPixel + 7) / 8);
            var memoryBlockPointer = Marshal.AllocHGlobal(height * stride);
            imageViewModel.Image.CopyPixels(new Int32Rect(0, 0, width, height), memoryBlockPointer, height * stride, stride);
            var bitmap = new Bitmap(width, height, stride, PixelFormat.Format32bppPArgb, memoryBlockPointer);

            Assert.AreEqual(Color.FromArgb(255, 0, 0, 255), bitmap.GetPixel(0, 0));
        }

        [TestMethod]
        public void SyncProcessGreenIsCorrect()
        {
            var temp = new Bitmap(1, 1);
            temp.SetPixel(0, 0, Color.DarkGreen);

            imageViewModel.Image = Imaging.CreateBitmapSourceFromHBitmap(temp.GetHbitmap(), IntPtr.Zero,
                Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            imageViewModel.SyncProcess();

            var width = imageViewModel.Image.PixelWidth;
            var height = imageViewModel.Image.PixelHeight;
            var stride = width * ((imageViewModel.Image.Format.BitsPerPixel + 7) / 8);
            var memoryBlockPointer = Marshal.AllocHGlobal(height * stride);
            imageViewModel.Image.CopyPixels(new Int32Rect(0, 0, width, height), memoryBlockPointer, height * stride, stride);
            var bitmap = new Bitmap(width, height, stride, PixelFormat.Format32bppPArgb, memoryBlockPointer);

            Assert.AreEqual(Color.FromArgb(255, 0, 255, 0), bitmap.GetPixel(0, 0));
        }

        [TestMethod]
        public void AsyncProcessRedIsCorrect()
        {
            var temp = new Bitmap(1, 1);
            temp.SetPixel(0, 0, Color.LightCoral);

            imageViewModel.Image = Imaging.CreateBitmapSourceFromHBitmap(temp.GetHbitmap(), IntPtr.Zero,
                Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            AsyncContext.Run(() => imageViewModel.AsyncProcess());

            var width = imageViewModel.Image.PixelWidth;
            var height = imageViewModel.Image.PixelHeight;
            var stride = width * ((imageViewModel.Image.Format.BitsPerPixel + 7) / 8);
            var memoryBlockPointer = Marshal.AllocHGlobal(height * stride);
            imageViewModel.Image.CopyPixels(new Int32Rect(0, 0, width, height), memoryBlockPointer, height * stride, stride);
            var bitmap = new Bitmap(width, height, stride, PixelFormat.Format32bppPArgb, memoryBlockPointer);

            Assert.AreEqual(Color.FromArgb(255, 255, 0, 0), bitmap.GetPixel(0, 0));
        }

        [TestMethod]
        public void AsyncProcessBlueIsCorrect()
        {
            var temp = new Bitmap(1, 1);
            temp.SetPixel(0, 0, Color.DarkBlue);

            imageViewModel.Image = Imaging.CreateBitmapSourceFromHBitmap(temp.GetHbitmap(), IntPtr.Zero,
                Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            AsyncContext.Run(() => imageViewModel.AsyncProcess());

            var width = imageViewModel.Image.PixelWidth;
            var height = imageViewModel.Image.PixelHeight;
            var stride = width * ((imageViewModel.Image.Format.BitsPerPixel + 7) / 8);
            var memoryBlockPointer = Marshal.AllocHGlobal(height * stride);
            imageViewModel.Image.CopyPixels(new Int32Rect(0, 0, width, height), memoryBlockPointer, height * stride, stride);
            var bitmap = new Bitmap(width, height, stride, PixelFormat.Format32bppPArgb, memoryBlockPointer);

            Assert.AreEqual(Color.FromArgb(255, 0, 0, 255), bitmap.GetPixel(0, 0));
        }

        [TestMethod]
        public void AsyncProcessGreenIsCorrect()
        {
            var temp = new Bitmap(1, 1);
            temp.SetPixel(0, 0, Color.LightGreen);

            imageViewModel.Image = Imaging.CreateBitmapSourceFromHBitmap(temp.GetHbitmap(), IntPtr.Zero,
                Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            AsyncContext.Run(() => imageViewModel.AsyncProcess());

            var width = imageViewModel.Image.PixelWidth;
            var height = imageViewModel.Image.PixelHeight;
            var stride = width * ((imageViewModel.Image.Format.BitsPerPixel + 7) / 8);
            var memoryBlockPointer = Marshal.AllocHGlobal(height * stride);
            imageViewModel.Image.CopyPixels(new Int32Rect(0, 0, width, height), memoryBlockPointer, height * stride, stride);
            var bitmap = new Bitmap(width, height, stride, PixelFormat.Format32bppPArgb, memoryBlockPointer);

            Assert.AreEqual(Color.FromArgb(255, 0, 255, 0), bitmap.GetPixel(0, 0));
        }
    }
}