global using System;
global using Community.VisualStudio.Toolkit;
global using Microsoft.VisualStudio.Shell;
global using Task = System.Threading.Tasks.Task;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Markup;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using DebugDataViewCore.Commands;
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

            WeakReferenceMessenger.Default.Register<SelectedItemChangedMessage>(this, (r, m) =>
            {
                _expressionString = m.Value;
                DataViewRefresh(m.Value);
            });

            _cancellationToken = cancellationToken;
            var dte = await GetServiceAsync(typeof(DTE)) as DTE2;
            _dte = dte;
            _dbgEvents = _dte.Events.DebuggerEvents;
            _dbgEvents.OnEnterBreakMode += OnBreakMode;

            // 补偿：初始化时立即同步状态
            //switch (_dte.Debugger.CurrentMode)
            //{
            //    case dbgDebugMode.dbgBreakMode:
            //        dbgExecutionAction reason = dbgExecutionAction.dbgExecutionActionDefault;
            //        OnBreakMode(dbgEventReason.dbgEventReasonBreakpoint, ref reason);
            //        break;
            //}
        }

        private void OnBreakMode(dbgEventReason reason, ref dbgExecutionAction ExecutionAction)
        {
            DataViewRefresh(_expressionString);
        }

        public void DataViewAddItem(string expressionString)
        {
            try
            {
                ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
                {
                    if (Expression_Check(expressionString))
                    {
                        ToolWindowPane window = await ShowToolWindowAsync(typeof(Pane), 0, true, _cancellationToken);
                        if (window?.Content is DataViewControl control) control.AddExpression(expressionString);
                    }
                }).FireAndForget();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in DataViewAddItem: {ex.Message}");
            }
        }

        private void DataViewRefresh(string expressionString)
        {
            try
            {
                var expression = _dte.Debugger.GetExpression(expressionString, true);
                var tempData = new List<double>();
                if (expression is { IsValidValue: true })
                {
                    foreach (Expression item in expression.DataMembers)
                    {
                        var name = item.Name;      // 元素名 (如 [0], [1] ...)
                        var value = item.Value;    // 元素值 (字符串)
                        var type = item.Type;      // 元素类型
                        if (double.TryParse(value, out var parsedValue)) tempData.Add(parsedValue);
                    }
                }
                ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
                {
                    ToolWindowPane window = await ShowToolWindowAsync(typeof(Pane), 0, true, _cancellationToken);
                    if (window?.Content is DataViewControl control)
                    {
                        if (tempData.Count > 0) control.Refresh(tempData.ToArray());
                        else control.PlotClear();
                    }
                }).FireAndForget();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in DataViewRefresh: {ex.Message}");
            }
        }

        private bool Expression_Check(string expressionString)
        {
            var expression = _dte.Debugger.GetExpression(expressionString, true);
            if (expression is { IsValidValue: true }) return true;
            else return false;  
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                WeakReferenceMessenger.Default.Unregister<SelectedItemChangedMessage>(this);
                // 取消事件绑定
                if (_dte != null)
                {
                    _dbgEvents.OnEnterBreakMode -= OnBreakMode;
                }
            }
            base.Dispose(disposing);
        }
    }
}