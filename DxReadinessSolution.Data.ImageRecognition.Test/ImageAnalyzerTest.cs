using DxReadinessSolution.CrossCutting.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;
using System.IO;
using System.Threading.Tasks;

namespace DxReadinessSolution.Data.ImageRecognition.Test
{
    [TestClass]
    public class ImageAnalyzerTest : TestsFor<ImageAnalyzer>
    {
        [TestMethod]
        public void ImageAnalyzer_CanBeConstructedByTestFramework()
        {
            Instance.ShouldNotBeNull();
        }


        [TestMethod, TestCategory("SUPERSLOW")]
        public async Task  ImageAnalyzer_WillReturnResultOfAnalysis()
        {
            // Arrange 
            string path = @"C:\temp\sample1.jpg";
            var imageStream = File.OpenRead(path);
            
            // Act
            var result = await Instance.AnalyzeImage(imageStream);

            // Assert
            result.ShouldNotBeNull();
        }
    }
}
