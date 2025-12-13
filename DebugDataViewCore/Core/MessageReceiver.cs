namespace DebugDataViewCore
{
    public partial class DebugDataViewCorePackage
    {
        /// <summary>
        /// 选中表达式变化而触发的事件
        /// </summary>
        /// <param name="recipient">发送方</param>
        /// <param name="handler">传递的信息</param>
        private async void ExpressionItemChangedReceiver(object recipient, ExpressionItemChanged handler)
        {
            _debugData.CurrentExpression = handler.Value;
            _debugData.CurrentIndex = 0;
            await DataViewRefreshAsync();
        }

        /// <summary>
        /// 数据段切换而触发的事件
        /// </summary>
        /// <param name="recipient">发送方</param>
        /// <param name="handler">传递的信息</param>
        private async void DataIntervalMoveReceiver(object recipient, DataIntervalMove handler)
        {
            if (handler.Value == MoveEnum.LeftMove && _debugData.CurrentIndex > 0)
            {
                _debugData.CurrentIndex--;
                await DataViewRefreshAsync();
            }
            if (handler.Value == MoveEnum.RightMove && (_debugData.CurrentIndex + 1) * _debugData.PerPlotMaxNum < _debugData.DataLength)
            {
                _debugData.CurrentIndex++;
                await DataViewRefreshAsync();
            }
        }
    }
}
