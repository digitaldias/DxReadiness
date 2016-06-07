using System;
using System.Threading.Tasks;

namespace DxReadinessSolution.Domain.Contracts
{
    public interface IExceptionHandler
    {
        void Run(Action unsafeAction);

        Task RunAsync(Func<Task> unsafeAction);
    }
}
