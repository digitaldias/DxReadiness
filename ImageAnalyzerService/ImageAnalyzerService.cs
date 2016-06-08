using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using DxReadinessSolution.Domain.Contracts;
using DxReadinessSolution.Domain.Entities;
using System.IO;
using Microsoft.ServiceFabric.Services.Remoting.FabricTransport.Runtime;
using DxReadinessSolution.Data.ImageRecognition;
using DxReadinessSolution.Fakes;
using DxReadinessSolution.Business;

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

        public async Task<ImageResult> Analyze(Stream imageStream)
        {
            var analyzer = new ImageAnalyzer();

            return await  _exceptionHandler.Get(() => analyzer.AnalyzeImage(imageStream));
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
