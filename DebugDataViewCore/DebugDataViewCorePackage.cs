
using System;
using System.Runtime.InteropServices;
using System.Threading;
using Community.VisualStudio.Toolkit;
using CommunityToolkit.Mvvm.Messaging;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using static CommunityToolkit.Mvvm.Messaging.WeakReferenceMessenger;
using static DebugDataViewCore.DataView;
using Task = System.Threading.Tasks.Task;

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
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            _cancelToken = cancellationToken;
            DataView.Initialize(this);
            await this.RegisterCommandsAsync();

            Default.Register<ExpressionItemChanged>(this, ExpressionItemChangedReceiver);
            Default.Register<DataIntervalMove>(this, DataIntervalMoveReceiver);

            if (await GetServiceAsync(typeof(DTE)) is DTE2 dte)
            {
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                _dte = dte;
                _dbgEvents = _dte.Events.DebuggerEvents;
                _dbgEvents.OnEnterBreakMode += OnBreakMode;
            }
        }
    }
}