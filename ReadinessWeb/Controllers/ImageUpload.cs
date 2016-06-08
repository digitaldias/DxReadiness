using DxReadinessSolution.Domain.Contracts;
using ImageAnalysisActor.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using ReadinessWeb.IoC;
using StructureMap;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ReadinessWeb.Controllers
{
    class ImageUpload : ApiController
    {
        private readonly IByteStreamVerifier _byteStreamVerifier;


        public ImageUpload(IByteStreamVerifier byteStreamVerifier)
        {
            _byteStreamVerifier = Startup.DiContainer.GetInstance<IByteStreamVerifier>();
        }


        [HttpPut]
        public async Task Put(byte[] imageBytes)
        {
            if (!_byteStreamVerifier.IsValid(imageBytes))
                return;

            var actorId = ActorId.CreateRandom();
            var imageAnalyzerActor = ActorProxy.Create<IImageAnalysisActor>(actorId);

            await NewMethod(imageBytes, imageAnalyzerActor);
        }


        private static async Task NewMethod(byte[] imageBytes, IImageAnalysisActor imageAnalyzerActor)
        {
            using (var memoryStream = new MemoryStream(imageBytes))
            {
                var result = await imageAnalyzerActor.AnalyzeImageStreamAsync(memoryStream);
            }
        }
    }
}
