using DxReadinessSolution.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DxReadinessSolution.Fakes
{
    public class Logger : ILogger
    {
        public void LogException(Exception ex)
        {
            Debug.WriteLine("IT FRICKEN FAILED>>\n\n" + ex.Message + "\n" + ex.StackTrace.ToString());
        }
    }
}
