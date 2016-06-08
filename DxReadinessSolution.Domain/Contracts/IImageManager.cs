using DxReadinessSolution.Domain.Entities;
using System.IO;
using System.Threading.Tasks;

namespace DxReadinessSolution.Domain.Contracts
{
    public interface IImageManager
    {
        Task<ImageResult> Process(Stream imageStream);
    }
}
