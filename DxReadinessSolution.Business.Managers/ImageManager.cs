using DxReadinessSolution.Domain.Contracts;
using DxReadinessSolution.Domain.Contracts.Converters;
using DxReadinessSolution.Domain.Entities;
using ImageAnalyzerService;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DxReadinessSolution.Business.Managers
{
    public class ImageManager : IImageManager
    {
        private readonly IByteStreamVerifier _byteStreamVerifier;
        private readonly IExceptionHandler _exceptionHandler;
        private readonly IStreamToByteConverter _streamToByteConverter;


        public ImageManager()
        {
            throw new ArgumentException("Never call this constructor");
        }


        public ImageManager(IByteStreamVerifier byteStreamVerifier, IExceptionHandler exceptionHandler, IStreamToByteConverter streamToByteConverter)
        {
            _byteStreamVerifier    = byteStreamVerifier;
            _exceptionHandler      = exceptionHandler;
            _streamToByteConverter = streamToByteConverter;
        }


        public async Task<ImageResult> Process(Stream imageStream)
        {
            MemoryStream memoryStream = ConvertReadOnlyStreamToMemoryStream(imageStream);

            if (memoryStream == null || memoryStream.Length == 0)
                return null;

            var imageBytes = _exceptionHandler.Get(() => _streamToByteConverter.Convert(memoryStream));
            if (!_byteStreamVerifier.IsValid(imageBytes))
                return null;

            return await AnalyzeImageUsingServiceProxy(imageBytes);
        }


        private MemoryStream ConvertReadOnlyStreamToMemoryStream(Stream imageStream)
        {
            return _exceptionHandler.Get(() => 
            {
                var memoryStream = new MemoryStream();
                imageStream.CopyTo(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);

                return memoryStream;
            });
        }


        private async Task<ImageResult> AnalyzeImageUsingServiceProxy(byte[] imageBytes)
        {
            var imageAnalyzerProxy = CreateImageAnalyzerProxy();
            return await _exceptionHandler.Get(() => imageAnalyzerProxy.Analyze(imageBytes));
        }


        private static IImageAnalyzerService CreateImageAnalyzerProxy()
        {
            var uri = new Uri("fabric:/ReadinessApi/ImageAnalyzerService");
            var serviceProxy = ServiceProxy.Create<IImageAnalyzerService>(uri);
            return serviceProxy;
        }
    }
}
