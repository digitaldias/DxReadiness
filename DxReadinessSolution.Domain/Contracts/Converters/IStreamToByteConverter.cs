using System.IO;

namespace DxReadinessSolution.Domain.Contracts.Converters
{
    public interface IStreamToByteConverter
    {
        byte[] Convert(Stream stream);
    }
}
