using DxReadinessSolution.Business;
using DxReadinessSolution.Data.ImageRecognition;
using DxReadinessSolution.Domain.Entities;
using DxReadinessSolution.Fakes;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.FabricTransport.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using System.Collections.Generic;
using System.Fabric;
using System.IO;
using System.Threading.Tasks;

namespace ImageAnalyzerService
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class ImageAnalyzerService : StatelessService, IImageAnalyzerService
    {
        private readonly ExceptionHandler _exceptionHandler;

        public ImageAnalyzerService(StatelessServiceContext context)
            : base(context)
        {
            var logger = new Logger();
            _exceptionHandler = new ExceptionHandler(logger);
        }

        public async Task<ImageResult> Analyze(byte[] imageStream)
        {
            var analyzer = new ImageAnalyzer();

            var memoryStream = new MemoryStream(imageStream);

            var result =  await  _exceptionHandler.Get(() => analyzer.AnalyzeImage(memoryStream));

            return result;
        }


        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new[] 
            {
               new ServiceInstanceListener(context =>
                new FabricTransportServiceRemotingListener(context, this))
            };
        }
    }
}
