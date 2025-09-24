global using System;
global using Community.VisualStudio.Toolkit;
global using Microsoft.VisualStudio.Shell;
global using Task = System.Threading.Tasks.Task;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Markup;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using DebugDataViewCore.Commands;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;
using Newtonsoft.Json.Linq;
using ScottPlot.Drawing.Colormaps;
using static CommunityToolkit.Mvvm.Messaging.WeakReferenceMessenger;
using static DebugDataViewCore.DataView;
using static Microsoft.VisualStudio.Threading.AsyncReaderWriterLock;

namespace DebugDataViewCore
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration(Vsix.Name, Vsix.Description, Vsix.Version)]
    [ProvideToolWindow(typeof(Pane), 
        Style = VsDockStyle.Tabbed,
        Orientation = ToolWindowOrientation.Bottom,
        Window = WindowGuids.Locals)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(PackageGuids.DebugDataViewCoreString)]
    public sealed partial class DebugDataViewCorePackage : ToolkitPackage
    {
        private DTE2 _dte;
        private EnvDTE.DebuggerEvents _dbgEvents;
        public CancellationToken CancelToken;
        private string _expressionString = "";
        private DataViewWindow _dataViewWindow;
        private readonly int _perPlotMaxNum = 1000;
        private int _currentIndex = 0;
        private int _dataLength = 0;

        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            CancelToken = cancellationToken;
            DataView.Initialize(this);
            await this.RegisterCommandsAsync();

            Default.Register<ItemChanged>(this, ItemChangedReceiver);
            Default.Register<DataIntervalMove>(this, DataIntervalMoveReceiver);

            if (await GetServiceAsync(typeof(DTE)) is DTE2 dte)
            {
                _dte = dte;
                _dbgEvents = _dte.Events.DebuggerEvents;
                _dbgEvents.OnEnterBreakMode += OnBreakMode;
            }
        }
    }
}