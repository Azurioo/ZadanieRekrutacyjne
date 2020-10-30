using System.Drawing;
using NUnit.Framework;
using ImageLib;

namespace ImageLibTests
{
    public class Tests
    {
        private ImageProcessing _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new ImageProcessing();
        }

        [Test]
        public void FromPinkToRedTest()
        {
            Assert.AreEqual( ColorTranslator.FromHtml("#FF0000"), _sut.GetMainColor(ColorTranslator.FromHtml("#FFCCE5")));
        }
        [Test]
        public void FromLimeToGreenTest()
        {
            Assert.AreEqual(ColorTranslator.FromHtml("#00FF00"), _sut.GetMainColor(ColorTranslator.FromHtml("#99FF33")));
        }
        [Test]
        public void FromSkyBlueToBlueTest()
        {
            Assert.AreEqual(ColorTranslator.FromHtml("#0000FF"), _sut.GetMainColor(ColorTranslator.FromHtml("#99CCFF")));
        }
    }
}