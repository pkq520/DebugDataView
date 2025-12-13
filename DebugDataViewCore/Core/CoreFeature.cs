using System;
using System.Threading.Tasks;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using static DataSturctures.StatusEnum;
using Task = System.Threading.Tasks.Task;

namespace DebugDataViewCore
{
    public partial class DebugDataViewCorePackage
    {
        /// <summary>
        /// 添加表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <returns>返回异步任务</returns>
        public async Task DataViewItemAddAsync(string expression)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            Expression expressionResult = _dte.Debugger.GetExpression(expression, true);
            if (expressionResult is { IsValidValue: true }) _dataViewWindow.AddExpression(expression);
        }

        /// <summary>
        /// 数据刷新
        /// </summary>
        /// <returns>返回异步任务</returns>
        private async Task DataViewRefreshAsync()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            if (int.TryParse(_dte.Debugger.GetExpression(_debugData.CurrentExpression + ".Length", true).Value, out _debugData.DataLength))
            {
                if (_debugData.DataLength > _debugData.PerPlotMaxNum)
                    await ShowRunInfoAsync("Data volume exceeds the upper limit,therefore paginated display is required");
                await ShowDataIntervalinfoAsync(_debugData.GetDataIntervalinfo());
                await GetViewDataAsync();
            }
        }

        /// <summary>
        /// 获取需要显示的数据
        /// </summary>
        /// <returns>返回异步任务</returns>
        private async Task GetViewDataAsync()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            string dataIntervalString = $"[{_debugData.StartIndex}..{_debugData.StopIndex}]";
            Expression expression = _dte.Debugger.GetExpression(_debugData.CurrentExpression + dataIntervalString, true);
            double[] tempData = new double[_debugData.StopIndex - _debugData.StartIndex + 1];
            if (expression is { IsValidValue: true })
            {
                Expressions dataMembers = expression.DataMembers;
                int index = 0;
                foreach (Expression item in dataMembers)
                {
                    tempData[index] = double.TryParse(item.Value, out double parsedValue) ? parsedValue : 0;
                    index++;
                }
            }
            if (tempData.Length > 0)
            {
                _dataViewWindow.AddData(tempData);
                _dataViewWindow.Refresh();
            }
            else _dataViewWindow.PlotClear();
        }

        /// <summary>
        /// debug进程语言判断
        /// </summary>
        /// <returns>支持时返回true，否则返回false</returns>
        public bool ProgramLanguageDetection()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (_dte.ActiveSolutionProjects is not Array projects) return false;
            if(projects.GetValue(0) is not Project project) return false;
            if (project.FileName.EndsWith(ProgramLanguageEnum.Csharp.GetDescription())) 
            { 
                return true;
            }
            return false;
        }

        /// <summary>
        /// 变量类型判断
        /// </summary>
        /// <param name="expression">需要判断的变量名称</param>
        /// <returns>支持时返回true，否则返回false</returns>
        public async Task<bool> ValueTypeDetectionAsync(string expression)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            Expression expressionResult = _dte.Debugger.GetExpression(expression, true);
            if (expressionResult is { IsValidValue: true })
            {
                string valueType = expressionResult.Type;
                if (valueType == SupportValueTypeEnum.ShortArrary.GetDescription() ||
                    valueType == SupportValueTypeEnum.IntArrary.GetDescription() ||
                    valueType == SupportValueTypeEnum.FloatArrary.GetDescription() ||
                    valueType == SupportValueTypeEnum.DoubleArrary.GetDescription())
                {
                    return true;
                }
            }
            return false;
        }
    }
}
