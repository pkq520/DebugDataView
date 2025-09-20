global using System;
global using Community.VisualStudio.Toolkit;
global using Microsoft.VisualStudio.Shell;
global using Task = System.Threading.Tasks.Task;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Markup;
using EnvDTE;
using EnvDTE80;
using ScottPlot.Drawing.Colormaps;
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
    public sealed class DebugDataViewCorePackage : ToolkitPackage
    {
        private DTE2 _dte;
        private EnvDTE.DebuggerEvents _dbgEvents;
        private CancellationToken _cancellationToken;

        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await JoinableTaskFactory.SwitchToMainThreadAsync();
            var dte = await GetServiceAsync(typeof(DTE)) as DTE2;
            if (dte == null)
            {
                System.Diagnostics.Debug.WriteLine("Error: _dte is null!");
                return;
            }
            _dte = dte;
            _dbgEvents = _dte.Events.DebuggerEvents;
            _dbgEvents.OnEnterBreakMode += OnBreakMode;
            _dbgEvents.OnEnterDesignMode += OnDesignMode;
            _cancellationToken = cancellationToken;
            DataView.Initialize(this);
            await this.RegisterCommandsAsync();
        }

        private void OnBreakMode(dbgEventReason reason, ref dbgExecutionAction ExecutionAction)
        {
            try
            {
                ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
                {
                    await JoinableTaskFactory.SwitchToMainThreadAsync();
                    var debugger = _dte.Debugger;
                    var expr = debugger.GetExpression("arrary", true);
                    var data = new System.Collections.Generic.List<double>();
                    if (expr != null && expr.IsValidValue)
                    {
                        foreach (EnvDTE.Expression item in expr.DataMembers)
                        {
                            var name = item.Name;      // 元素名 (如 [0], [1] ...)
                            var value = item.Value;    // 元素值 (字符串)
                            var type = item.Type;      // 元素类型
                            if (double.TryParse(value, out var parsedValue))
                            {
                                data.Add(parsedValue);
                            }
                        }
                        ToolWindowPane window = await ShowToolWindowAsync(typeof(Pane), 0, true, _cancellationToken);
                        if (window?.Content is DataViewControl control)
                        {
                            control.Refresh(data.ToArray());
                        }
                    }
                   
                }).FireAndForget();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in OnBreakMode: {ex.Message}");
            }
        }

        private void OnDesignMode(dbgEventReason reason)
        {
            try
            {
                ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
                {
                    await JoinableTaskFactory.SwitchToMainThreadAsync();
                    ToolWindowPane window = await ShowToolWindowAsync(typeof(Pane), 0, true, _cancellationToken);
                    if (window?.Content is DataViewControl control)
                    {
                        control.Clear();
                    }
                }).FireAndForget();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in OnDesignMode: {ex.Message}");
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // 取消事件绑定
                if (_dte != null)
                {
                    _dbgEvents.OnEnterBreakMode -= OnBreakMode;
                    _dbgEvents.OnEnterDesignMode -= OnDesignMode;
                }
            }
            base.Dispose(disposing);
        }
    }
}