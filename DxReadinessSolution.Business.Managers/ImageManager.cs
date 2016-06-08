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
            if (imageStream == null || imageStream.Length == 0)
                return null;

            var imageBytes = _streamToByteConverter.Convert(imageStream);
            if (!_byteStreamVerifier.IsValid(imageBytes))
                return null;

            return await AnalyzeImageUsingServiceProxy(imageBytes);
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
