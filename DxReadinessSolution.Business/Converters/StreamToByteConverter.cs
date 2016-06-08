using DxReadinessSolution.Domain.Contracts.Converters;
using System.IO;

namespace DxReadinessSolution.Business.Converters
{
    public class StreamToByteConverter : IStreamToByteConverter
    {
        public byte[] Convert(Stream stream)
        {
            byte[] buffer = new byte[stream.Length];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}
