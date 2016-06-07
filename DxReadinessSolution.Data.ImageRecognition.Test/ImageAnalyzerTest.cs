using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.IO;
using Should;

namespace DxReadinessSolution.Data.ImageRecognition.Test
{
    [TestClass]
    public class ImageAnalyzerTest
    {
        [TestMethod]
        public async Task  TestMethod1()
        {
            // Arrange 
            var imageAnalyzer = new ImageAnalyzer();

            string path = @"C:\temp\sample.jpg";
            var imageStream = File.OpenRead(path);
            
            // Act
            var result = await imageAnalyzer.AnalyzeImage(imageStream);

            // Assert
            result.ShouldNotBeNull();
        }
    }
}
