using DxReadinessSolution.Domain.Entities;
using System.IO;
using System.Threading.Tasks;

namespace DxReadinessSolution.Domain.Contracts
{
    public interface IImageAnalyzerServicekjsfdøkshfhs
    {
        Task<ImageResult> Analyze(Stream imageStream);
    }
}