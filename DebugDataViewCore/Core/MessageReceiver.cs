using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebugDataViewCore
{
    public partial class DebugDataViewCorePackage
    {
        private async void ItemChangedReceiver(object recipient, ItemChanged handler)
        {
            _expressionString = handler.Value;
            _currentIndex = 0;
            await DataViewRefresh();
        }

        private async void DataIntervalMoveReceiver(object recipient, DataIntervalMove handler)
        {
            if (handler.Value == MoveEnum.LeftMove && _currentIndex > 0)
            {
                _currentIndex--;
                await DataViewRefresh();
            }
            if (handler.Value == MoveEnum.RightMove && (_currentIndex + 1) * _perPlotMaxNum < _dataLength)
            {
                _currentIndex++;
                await DataViewRefresh();
            }
        }
    }
}
