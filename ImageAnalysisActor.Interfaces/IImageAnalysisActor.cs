using DxReadinessSolution.Domain.Entities;
using Microsoft.ServiceFabric.Actors;
using System.IO;
using System.Threading.Tasks;

namespace ImageAnalysisActor.Interfaces
{
    public interface IImageAnalysisActor : IActor
    {
        Task<ImageResult> AnalyzeImageStreamAsync(Stream imageBytes);
    }
}
