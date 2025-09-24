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
        public async Task DataViewItemAdd(string expressionString)
        {
            var expression = _dte.Debugger.GetExpression(expressionString, true);
            if (expression is { IsValidValue: true })
            {
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                _dataViewWindow.AddExpression(expressionString);
            }
        }

        private async Task DataViewRefresh()
        {
            if (int.TryParse(_dte.Debugger.GetExpression(_expressionString + ".Length", true).Value, out _dataLength))
            {
                if (_dataLength > _perPlotMaxNum) await ShowInfo("Data volume exceeds the upper limit,therefore paginated display is required");
                int startIndex = _currentIndex * _perPlotMaxNum;
                int stopIndex = _dataLength > (_currentIndex + 1) * _perPlotMaxNum ? (_currentIndex + 1) * _perPlotMaxNum - 1 : _dataLength - 1;
                await ShowIntervalinfo($"({_currentIndex + 1}/{Math.Ceiling(_dataLength / (double)_perPlotMaxNum)})[{startIndex}~{stopIndex}]");
                await GetPerData(startIndex, stopIndex);
            }
        }

        private async Task GetPerData(int startIndex, int stopIndex)
        {
            string dataInterval = $"[{startIndex}..{stopIndex}]";
            var expression = _dte.Debugger.GetExpression(_expressionString + dataInterval, true);
            double[] tempData = new double[stopIndex - startIndex + 1]; ;
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
            if (tempData.Length > 0) _dataViewWindow.AddPlotData(tempData);
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            if (tempData.Length > 0) _dataViewWindow.Refresh();
            else _dataViewWindow.PlotClear();
        }
    }
}
