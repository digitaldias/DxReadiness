using System;

namespace DxReadinessSolution.Domain.Contracts
{
    public interface ILogger
    {
        void LogException(Exception ex);
    }
}