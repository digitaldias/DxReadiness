using DxReadinessSolution.Domain.Contracts;
using ImageAnalysisActor.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using System.IO;
using System.Threading.Tasks;
using System.Web.Http;

namespace ReadinessWeb.Controllers
{
    class ImageUpload : ApiController
    {
        private readonly IByteStreamVerifier _byteStreamVerifier;
        private readonly IExceptionHandler _exceptionHandler;

        public ImageUpload()
        {
            _byteStreamVerifier = Startup.DiContainer.GetInstance<IByteStreamVerifier>();
            _exceptionHandler   = Startup.DiContainer.GetInstance<IExceptionHandler>();
        }


        [HttpPut]
        public async Task Put(byte[] imageBytes)
        {
            if (!_byteStreamVerifier.IsValid(imageBytes))
                return;

            await ConvertImageBytesToStreamAndAnalyzeIt(imageBytes);
        }


        private async Task ConvertImageBytesToStreamAndAnalyzeIt(byte[] imageBytes)
        {
            var actorId            = ActorId.CreateRandom();
            var imageAnalyzerActor = ActorProxy.Create<IImageAnalysisActor>(actorId);

            using (var memoryStream = new MemoryStream(imageBytes))
            {
                var result = await  _exceptionHandler.Get(() => imageAnalyzerActor.AnalyzeImageStreamAsync(memoryStream));
            }
        }
    }
}
