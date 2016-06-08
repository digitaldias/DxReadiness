using DxReadinessSolution.Domain.Contracts;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Http;

namespace ReadinessWeb.Controllers
{
    public class ImagesController : ApiController
    {
        private readonly IByteStreamVerifier _byteStreamVerifier;
        private readonly IExceptionHandler _exceptionHandler;


        public ImagesController()
        {
            _byteStreamVerifier = Startup.DiContainer.GetInstance<IByteStreamVerifier>();
            _exceptionHandler   = Startup.DiContainer.GetInstance<IExceptionHandler>();
        }


        [HttpGet]
        public async Task<string> Get()
        {
            return await Task.FromResult("Go Away!");
        }


        [HttpPut]
        public async Task Put()
        {
            var imageStream = await Request.Content.ReadAsStreamAsync();
            var memoryStream = new MemoryStream();
            imageStream.CopyTo(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            var serviceProxy = CreateImageAnalyzerProxy();

            var buffer = ReadFully(memoryStream);
            

            var result = await _exceptionHandler.Get(() => serviceProxy.Analyze(buffer));
        }


        private static ImageAnalyzerService.IImageAnalyzerService CreateImageAnalyzerProxy()
        {
            var uri = new Uri("fabric:/ReadinessApi/ImageAnalyzerService");
            var serviceProxy = ServiceProxy.Create<ImageAnalyzerService.IImageAnalyzerService>(uri);
            return serviceProxy;
        }


        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}
