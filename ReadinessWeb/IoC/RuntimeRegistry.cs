using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadinessWeb.IoC
{
    public class RuntimeRegistry : Registry
    {
        public RuntimeRegistry()
        {
            Scan(x =>
            {
                x.Assembly("DxReadinessSolution.Domain");
                x.Assembly("DxReadinessSolution.Business");
                x.Assembly("DxReadinessSolution.Fakes");

                x.WithDefaultConventions();
            });
        }
    }
}
