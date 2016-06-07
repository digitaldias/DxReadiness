using DxReadinessSolution.Domain.Contracts;

namespace DxReadinessSolution.Business.Verifiers
{
    public class ByteStreamVerifier : IByteStreamVerifier
    {
        public bool IsValid(byte[] entity)
        {
            if (entity == null)
                return false;

            if (entity.Length == 0)
                return false;

            //TODO: Do we need a maximum length?

            return true;
        }
    }
}
