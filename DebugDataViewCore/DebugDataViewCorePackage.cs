global using System;
global using Community.VisualStudio.Toolkit;
global using Microsoft.VisualStudio.Shell;
global using Task = System.Threading.Tasks.Task;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Markup;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell.Interop;
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
        private string _expressionString = "";

        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            DataView.Initialize(this);
            await this.RegisterCommandsAsync();

            _cancellationToken = cancellationToken;
            var dte = await GetServiceAsync(typeof(DTE)) as DTE2;
            _dte = dte;
            _dbgEvents = _dte.Events.DebuggerEvents;
            _dbgEvents.OnEnterBreakMode += OnBreakMode;
            _dbgEvents.OnEnterDesignMode += OnDesignMode;

            // 补偿：初始化时立即同步状态
            switch (_dte.Debugger.CurrentMode)
            {
                case dbgDebugMode.dbgBreakMode:
                    dbgExecutionAction reason = dbgExecutionAction.dbgExecutionActionDefault;
                    OnBreakMode(dbgEventReason.dbgEventReasonBreakpoint, ref reason);
                    break;
                case dbgDebugMode.dbgDesignMode:
                    OnDesignMode(dbgEventReason.dbgEventReasonNone);
                    break;
            }
        }

        private void OnBreakMode(dbgEventReason reason, ref dbgExecutionAction ExecutionAction)
        {
            try
            {
                ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
                {
                    if (GetExpressionData(_expressionString, out double[] data))
                    {
                        ToolWindowPane window = await ShowToolWindowAsync(typeof(Pane), 0, true, _cancellationToken);
                        if (window?.Content is DataViewControl control) control.Refresh(data);
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
            //try
            //{
            //    ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
            //    {
            //        await JoinableTaskFactory.SwitchToMainThreadAsync();
            //        ToolWindowPane window = await ShowToolWindowAsync(typeof(Pane), 0, true, _cancellationToken);
            //        if (window?.Content is DataViewControl control)
            //        {
            //            control.Clear();
            //        }
            //    }).FireAndForget();
            //}
            //catch (Exception ex)
            //{
            //    System.Diagnostics.Debug.WriteLine($"Error in OnDesignMode: {ex.Message}");
            //}
        }

        public void DataViewAddItem(string expressionString)
        {
            try
            {
                ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
                {
                    if (GetExpressionData(expressionString, out double[] data))
                    {
                        ToolWindowPane window = await ShowToolWindowAsync(typeof(Pane), 0, true, _cancellationToken);
                        if (window?.Content is DataViewControl control) control.Refresh(data);
                    }
                }).FireAndForget();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in DataViewAddItem: {ex.Message}");
            }
        }

        private bool GetExpressionData(string expressionString, out double[] data)
        {
            var expression = _dte.Debugger.GetExpression(expressionString, true);
            var tempData = new List<double>();
            if (expression != null && expression.IsValidValue)
            {
                foreach (Expression item in expression.DataMembers)
                {
                    var name = item.Name;      // 元素名 (如 [0], [1] ...)
                    var value = item.Value;    // 元素值 (字符串)
                    var type = item.Type;      // 元素类型
                    if (double.TryParse(value, out var parsedValue)) tempData.Add(parsedValue);
                }
            }
            if(tempData.Count > 0) {
                _expressionString = expressionString;
                data = tempData.ToArray();
                return true;
            }
            else
            {
                data = [];
                return false;
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