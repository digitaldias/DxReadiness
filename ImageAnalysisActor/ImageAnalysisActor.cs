using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors.Runtime;
using ImageAnalysisActor.Interfaces;
using DxReadinessSolution.Domain.Entities;
using System;
using System.IO;

namespace ImageAnalysisActor
{

    [StatePersistence(StatePersistence.Persisted)]
    internal class ImageAnalysisActor : Actor, IImageAnalysisActor
    {
        public  Task<ImageResult> AnalyzeImageStreamAsync(Stream imageBytes)
        {
            //TODO: Pedro implement this once Rina is done
            return Task.FromResult<ImageResult>(new ImageResult());
        }

        protected override Task OnActivateAsync()
        {
            ActorEventSource.Current.ActorMessage(this, "Actor activated.");
            return this.StateManager.TryAddStateAsync("count", 0);
        }
    }
}
