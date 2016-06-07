using DxReadinessSolution.Domain.Contracts;
using System;
using System.Threading.Tasks;

namespace DxReadinessSolution.Business
{
    public class ExceptionHandler : IExceptionHandler
    {
        private readonly ILogger _logger;

        public ExceptionHandler(ILogger logger)
        {
            _logger = logger;
        }


        public void Run(Action unsafeAction)
        {
            try
            {
                unsafeAction.Invoke();
            }
            catch(Exception ex)
            {
                _logger.LogException(ex);
            }
        }


        public async Task RunAsync(Func<Task> unsafeAction)
        {
            try
            {
                await unsafeAction.Invoke();
            }
            catch(Exception ex)
            {
                _logger.LogException(ex);
            }
            
        }
    }
}
