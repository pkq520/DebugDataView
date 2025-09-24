using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;

namespace DebugDataViewCore
{
    public partial class DebugDataViewCorePackage
    {
        private void OnBreakMode(dbgEventReason reason, ref dbgExecutionAction ExecutionAction)
        {
            Task.Run(async() => await DataViewRefresh(), CancelToken);
        }
    }
}
