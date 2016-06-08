using DxReadinessSolution.Domain.Entities;
using Microsoft.ServiceFabric.Services.Remoting;
using System.IO;
using System.Threading.Tasks;

namespace ImageAnalyzerService
{
    public interface IImageAnalyzerService : IService
    {
        Task<ImageResult> Analyze(byte[] imageStream);
    }
}