namespace DebugDataViewCore
{
    public partial class DataViewWindow
    {
        /// <summary>
        /// 运行信息展示
        /// </summary>
        /// <param name="info">被展示的信息</param>
        public void ShowRunInfo(string info)
        {
            _viewModel.RunInfoDiaplay = info;
        }

        /// <summary>
        /// 数据区间信息显示
        /// </summary>
        /// <param name="info">被展示的信息</param>
        public void ShowDataIntervalinfo(string info)
        {
            _viewModel.DataIntervalInfoDiaplay = info;
        }
    }
}
