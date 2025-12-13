namespace DebugDataView
{
    public partial class DataViewWindow
    {
        /// <summary>
        /// 鼠标在图表上点击时触发的事件
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">发送的信息</param>
        private void ScottPlot_Basic_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (_data != null &&
                e.MiddleButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                (double mouseCoordX, double _) = ScottPlot_Basic.GetMouseCoordinates();
                (double pointX, double pointY, int _) = _signalPlotConstBasic.GetPointNearestX(mouseCoordX);

                if (_scottPlotService.CustomPointRepeat_Check(pointX, pointY) == false)//如果没有重复的点
                {
                    int vacancyIndex = _scottPlotService.CustomPointVacancy_Check();
                    if (vacancyIndex != -1) _scottPlotService.CustomPoint_Add(ScottPlot_Basic, vacancyIndex, pointX, pointY, _linearParameter);
                    else _scottPlotService.CustomPoint_MoveAdd(pointX, pointY, _linearParameter);
                }
            }
        }
    }
}
