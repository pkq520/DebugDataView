using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;
using ScottPlot;
using ScottPlot.Plottable;
using static DebugDataViewCore.Support.DataStructures;

namespace DebugDataViewCore.Support
{
    public interface IScottPlotService
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="peakPointNum">需要显示的峰的上限</param>
        /// <param name="customPointNum">自定义点上限</param>
        void Init(int peakPointNum, int customPointNum);

        /// <summary>
        /// ScottPlot图表鼠标响应控制
        /// </summary>
        /// <param name="scottPlotParameter">图表引用</param>
        /// <param name="isEnable">控制是否使能</param>
        void Response_Control(WpfPlot scottPlotParameter, bool isEnable);

        /// <summary>
        /// 初始化所有峰点
        /// </summary>
        /// <param name="scottPlotParameter">图表引用</param>
        /// <param name="markerSize">标记点大小</param>
        /// <param name="fontSize">字体大小</param>
        void PeakPoint_Init(WpfPlot scottPlotParameter, int markerSize = 10, int fontSize = 14);

        /// <summary>
        /// 显示所有峰点
        /// </summary>
        /// <param name="scottPlotParameter">图表引用</param>
        /// <param name="pointsList">峰点集合</param>
        /// <param name="linearParameter">轴参数</param>
        void PeakPoint_Display(WpfPlot scottPlotParameter, List<PointD> pointsList, ILinearParameter linearParameter);

        /// <summary>
        /// 清除所有峰点
        /// </summary>
        /// <param name="scottPlotParameter">图表引用</param>
        void PeakPoint_Clear(WpfPlot scottPlotParameter);

        /// <summary>
        /// 初始化所有自定义点
        /// </summary>
        /// <param name="scottPlotParameter">图表引用</param>
        void CustomPoint_Init(WpfPlot scottPlotParameter);

        /// <summary>
        /// 清除所有自定义点
        /// </summary>
        /// <param name="scottPlotParameter">图表引用</param>
        void CustomPoint_Clear(WpfPlot scottPlotParameter);

        /// <summary>
        /// 添加自定义新点到指定位置
        /// </summary>
        /// <param name="scottPlotParameter">图表引用</param>
        /// <param name="targetIndex">目标位置</param>
        /// <param name="pointX">X坐标</param>
        /// <param name="pointY">Y坐标</param>
        /// <param name="linearParameter">图表轴映射信息</param>
        void CustomPoint_Add(WpfPlot scottPlotParameter, int targetIndex, double pointX, double pointY, ILinearParameter linearParameter);

        /// <summary>
        /// 移动自定义点并添加新自定义点
        /// </summary>
        /// <param name="pointX"></param>
        /// <param name="pointY"></param>
        /// <param name="linearParameter"></param>
        void CustomPoint_MoveAdd(double pointX, double pointY, ILinearParameter linearParameter);

        /// <summary>
        /// 检查自定义点是否重复
        /// </summary>
        /// <param name="pointX">x坐标</param>
        /// <param name="pointY">y坐标</param>
        /// <returns>如果有重复返回true，否则返回false</returns>
        bool CustomPointRepeat_Check(double pointX, double pointY);

        /// <summary>
        /// 检查自定义点是否有空位
        /// </summary>
        /// <returns>如果有空位返回第一个空位的索引，否则返回-1</returns>
        int CustomPointVacancy_Check();
    }
    public class ScottPlotService : IScottPlotService
    {
        /// <summary>
        /// 谱图峰显示的最大个数
        /// </summary>
        private int _peakPointNum { get; set; }
        /// <summary>
        /// 自定义点的最大个数
        /// </summary>
        private int _customPointNum { get; set; }

        /// <summary>
        /// 谱峰标记点文本
        /// </summary>
        private Text[] _peakPointText;
        /// <summary>
        /// 谱峰标记点
        /// </summary>
        private MarkerPlot[] _peakPoint;
        /// <summary>
        /// 自定义标记点文本
        /// </summary>
        private Text[] _customPointText;
        /// <summary>
        /// 自定义标记点
        /// </summary>
        private MarkerPlot[] _customPoint;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="peakPointNum">需要显示的峰的上限</param>
        /// <param name="customPointNum">自定义点上限</param>
        public void Init(int peakPointNum, int customPointNum)
        {
            _peakPointNum = peakPointNum;
            _customPointNum = customPointNum;

            _peakPointText = new Text[_peakPointNum];
            _peakPoint = new MarkerPlot[_peakPointNum];
            _customPointText = new Text[_customPointNum];
            _customPoint = new MarkerPlot[_customPointNum];
        }

        /// <inheritdoc/>
        public void Response_Control(WpfPlot scottPlotParameter, bool isEnable)
        {
            scottPlotParameter.Configuration.AltLeftClickDragZoom = isEnable;
            scottPlotParameter.Configuration.DoubleClickBenchmark = isEnable;
            scottPlotParameter.Configuration.RightClickDragZoom = isEnable;
            scottPlotParameter.Configuration.RightClickDragZoomFromMouseDown = isEnable;
            scottPlotParameter.Configuration.LeftClickDragPan = isEnable;
            scottPlotParameter.Configuration.MiddleClickAutoAxis = isEnable;
            scottPlotParameter.Configuration.MiddleClickDragZoom = isEnable;
            scottPlotParameter.Configuration.ScrollWheelZoom = isEnable;
        }

        /// <inheritdoc/>
        public void PeakPoint_Init(WpfPlot scottPlotParameter, int markerSize = 10, int fontSize = 14)
        {
            for (int i = 0; i < _peakPointNum; i++)
            {
                _peakPoint[i] = scottPlotParameter.Plot.AddPoint(-1, -1);
                _peakPoint[i].Color = Color.CadetBlue;
                _peakPoint[i].MarkerSize = markerSize;
                _peakPoint[i].MarkerShape = MarkerShape.openCircle;
                _peakPoint[i].IsVisible = false;
                _peakPointText[i] = scottPlotParameter.Plot.AddText("", -1, -1);
                _peakPointText[i].Color = Color.Black;
                _peakPointText[i].FontSize = fontSize;
                _peakPointText[i].FontName = "CESI_HT_GB13000";
                _peakPointText[i].IsVisible = false;

                scottPlotParameter.Plot.Add(_peakPoint[i]);
                scottPlotParameter.Plot.Add(_peakPointText[i]);
            }
        }

        /// <inheritdoc/>
        public void PeakPoint_Display(WpfPlot scottPlotParameter, List<PointD> pointsList, ILinearParameter linearParameter)
        {
            var pointsListNeedDisplay = pointsList.ToList();
            for (int i = 0; i < _peakPointNum; i++)//显示
            {
                if (i < pointsListNeedDisplay.Count)
                {
                    scottPlotParameter.Plot.Remove(_peakPoint[i]);
                    scottPlotParameter.Plot.Remove(_peakPointText[i]);

                    _peakPoint[i].X = pointsListNeedDisplay[i].X.Linear_Process(linearParameter);
                    _peakPoint[i].Y = pointsListNeedDisplay[i].Y;
                    _peakPoint[i].IsVisible = true;

                    _peakPointText[i].Label = "X:" + (pointsListNeedDisplay[i].X.Linear_Process(linearParameter)).ToString("0.00");
                    _peakPointText[i].X = pointsListNeedDisplay[i].X.Linear_Process(linearParameter);
                    _peakPointText[i].Y = pointsListNeedDisplay[i].Y;
                    _peakPointText[i].Alignment = Alignment.MiddleLeft;
                    _peakPointText[i].DragEnabled = true;
                    _peakPointText[i].IsVisible = true;

                    scottPlotParameter.Plot.Add(_peakPoint[i]);
                    scottPlotParameter.Plot.Add(_peakPointText[i]);
                }
                else
                {
                    scottPlotParameter.Plot.Remove(_peakPoint[i]);
                    scottPlotParameter.Plot.Remove(_peakPointText[i]);

                    _peakPoint[i].X = -1;
                    _peakPoint[i].Y = -1;
                    _peakPoint[i].IsVisible = false;
                    _peakPointText[i].X = -10000;
                    _peakPointText[i].Y = -10000;
                    _peakPointText[i].IsVisible = false;

                    scottPlotParameter.Plot.Add(_peakPoint[i]);
                    scottPlotParameter.Plot.Add(_peakPointText[i]);
                }
            }
        }

        /// <inheritdoc/>
        public void PeakPoint_Clear(WpfPlot scottPlotParameter)
        {
            for (int i = 0; i < _peakPointNum; i++)//清空所有点
            {
                _peakPoint[i].X = -1;
                _peakPoint[i].Y = -1;
                _peakPoint[i].IsVisible = false;
                _peakPointText[i].X = -10000;
                _peakPointText[i].Y = -10000;
                _peakPointText[i].IsVisible = false;

                scottPlotParameter.Plot.Remove(_peakPoint[i]);
                scottPlotParameter.Plot.Remove(_peakPointText[i]);
            }
        }

        /// <inheritdoc/>
        public void CustomPoint_Init(WpfPlot scottPlotParameter)
        {
            for (int i = 0; i < _customPointNum; i++)
            {
                _customPoint[i] = scottPlotParameter.Plot.AddPoint(-1, -1);
                _customPoint[i].Color = Color.Red;
                _customPoint[i].MarkerSize = 10;
                _customPoint[i].MarkerShape = MarkerShape.openCircle;
                _customPoint[i].IsVisible = false;
                _customPointText[i] = scottPlotParameter.Plot.AddText("", -1, -1);
                _customPointText[i].Color = Color.Black;
                _customPointText[i].FontSize = 14;
                _customPointText[i].FontName = "CESI_HT_GB13000";
                _customPointText[i].IsVisible = false;
                _customPointText[i].DragEnabled = true;

                scottPlotParameter.Plot.Add(_customPoint[i]);
                scottPlotParameter.Plot.Add(_customPointText[i]);
            }
        }

        /// <inheritdoc/>
        public void CustomPoint_Clear(WpfPlot scottPlotParameter)
        {
            for (int i = 0; i < _customPointNum; i++)//清空所有点
            {
                _customPoint[i].X = -1;
                _customPoint[i].Y = -1;
                _customPoint[i].IsVisible = false;
                _customPointText[i].X = -10000;
                _customPointText[i].Y = -10000;
                _customPointText[i].IsVisible = false;

                scottPlotParameter.Plot.Remove(_customPoint[i]);
                scottPlotParameter.Plot.Remove(_customPointText[i]);
            }
        }

        /// <inheritdoc/>
        public void CustomPoint_Add(WpfPlot scottPlotParameter, int targetIndex, 
            double pointX, double pointY, ILinearParameter linearParameter)
        {
            scottPlotParameter.Plot.Remove(_customPoint[targetIndex]);
            scottPlotParameter.Plot.Remove(_customPointText[targetIndex]);

            _customPoint[targetIndex].X = pointX;
            _customPoint[targetIndex].Y = pointY;
            _customPoint[targetIndex].IsVisible = true;

            _customPointText[targetIndex].Label = "(X:" + pointX.Linear_Process(linearParameter) + $",Y:{pointY:N2})";
            _customPointText[targetIndex].X = pointX;
            _customPointText[targetIndex].Y = pointY;
            _customPointText[targetIndex].Alignment = Alignment.LowerLeft;
            _customPointText[targetIndex].IsVisible = true;

            scottPlotParameter.Plot.Add(_customPoint[targetIndex]);
            scottPlotParameter.Plot.Add(_customPointText[targetIndex]);
        }

        /// <inheritdoc/>
        public void CustomPoint_MoveAdd(double pointX, double pointY, ILinearParameter linearParameter)
        {
            for (int i = 0; i < _customPointNum - 1; i++)
            {
                _customPoint[i].X = _customPoint[i + 1].X;
                _customPoint[i].Y = _customPoint[i + 1].Y;
                _customPoint[i].IsVisible = _customPoint[i + 1].IsVisible;

                _customPointText[i].Label = _customPointText[i + 1].Label;
                _customPointText[i].X = _customPointText[i + 1].X;
                _customPointText[i].Y = _customPointText[i + 1].Y;
                _customPointText[i].Alignment = _customPointText[i + 1].Alignment;
                _customPointText[i].IsVisible = _customPointText[i + 1].IsVisible;
            }
            // 新点添加到最后一个位置
            _customPoint[_customPointNum - 1].X = pointX;
            _customPoint[_customPointNum - 1].Y = pointY;
            _customPoint[_customPointNum - 1].IsVisible = true;

            _customPointText[_customPointNum - 1].Label = "(X:" + pointX.Linear_Process(linearParameter) + $",Y:{pointY:N0})";
            _customPointText[_customPointNum - 1].X = pointX;
            _customPointText[_customPointNum - 1].Y = pointY;
            _customPointText[_customPointNum - 1].Alignment = Alignment.LowerLeft;
            _customPointText[_customPointNum - 1].IsVisible = true;
        }

        /// <inheritdoc/>
        public bool CustomPointRepeat_Check(double pointX, double pointY)
        {
            for (int i = 0; i < _customPointNum; i++)//判断是否有重复的点
            {
                if (_customPoint[i].X.AlmostEqual(pointX) && _customPoint[i].Y.AlmostEqual(pointY)) return true;
            }
            return false;
        }

        /// <inheritdoc/>
        public int CustomPointVacancy_Check()
        {
            for (int i = 0; i < _customPointNum; i++)
            {
                if (_customPoint[i].X.AlmostEqual(-1) && _customPoint[i].Y.AlmostEqual(-1)) return i;
            }
            return -1;
        }
    }
}
