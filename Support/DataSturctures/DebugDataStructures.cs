using System;

namespace DataSturctures
{
    /// <summary>
    /// DebugData数据结构体
    /// </summary>
    public class DebugDataStructures
    {
        /// <summary>
        /// 当前选中的表达式
        /// </summary>
        public string CurrentExpression = "";
        /// <summary>
        /// 数据划分区段长度
        /// </summary>
        public readonly int PerPlotMaxNum = 1000;
        /// <summary>
        /// 当前数据区段下标
        /// </summary>
        public int CurrentIndex = 0;
        /// <summary>
        /// 当前数据总长度
        /// </summary>
        public int DataLength = 0;

        /// <summary>
        /// 区间起始
        /// </summary>
        public int StartIndex => CurrentIndex * PerPlotMaxNum;
        /// <summary>
        /// 区间终止
        /// </summary>
        public int StopIndex => DataLength > (CurrentIndex + 1) * PerPlotMaxNum ?
            (CurrentIndex + 1) * PerPlotMaxNum - 1 :
            DataLength - 1;

        public string GetDataIntervalinfo()
        {
            return $"({CurrentIndex + 1}-{Math.Ceiling(DataLength / (double)PerPlotMaxNum)}) " +
                $"[{StartIndex}~{StopIndex}]";
        }
    }

    /// <summary>
    /// DebugData存储数据结构体
    /// </summary>
    public class DebugDataSaveStructures
    {
        /// <summary>
        /// 相关信息
        /// </summary>
        public string Information;
        /// <summary>
        /// 保存时间
        /// </summary>
        public DateTime SaveTime;
        /// <summary>
        /// 数据
        /// </summary>
        public double[] Data;
    }
}
