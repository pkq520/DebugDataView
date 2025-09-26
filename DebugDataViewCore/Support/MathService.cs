using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebugDataViewCore.Support
{
    /// <summary>
    /// 点坐标结构体(Y可接受double)
    /// </summary>
    /// <remarks>
    /// 点坐标结构体构造函数
    /// </remarks>
    /// <param name="x">X坐标</param>
    /// <param name="y">Y坐标</param>
    public struct PointD(int x, double y)
    {
        /// <summary> 
        /// X坐标 
        /// </summary>
        public int X { get; set; } = x;

        /// <summary> 
        /// Y坐标 
        /// </summary>
        public double Y { get; set; } = y;
    }

    public interface IMathService
    {
        /// <summary>
        /// 查找数组中的峰,按照峰值从大到小返回
        /// </summary>
        /// <param name="data">目标数组</param>
        /// <param name="peakPointNum">峰的上限个数</param>
        /// <param name="peakInterval">峰之间的最小间隔</param>
        /// <param name="slideWidth">数据窗口滑动宽度</param>
        /// <returns>
        /// 返回根据Y值从大到小排序好的 <see cref="List{PointD}"/>，T为 <see cref="PointD"/>， <see cref="PointD"/>
        /// 的 X 代表目标峰对应数组的下标，Y 则代表对应的数值。
        /// </returns>
        List<PointD> Peaks_Calculate(double[] data, int peakPointNum, int peakInterval = 20, int slideWidth = 10);
    }
    public class MathService : IMathService
    {
        /// <inheritdoc/>
        public List<PointD> Peaks_Calculate(double[] data, int peakPointNum, int peakInterval = 20, int slideWidth = 10)
        {
            var resultPeaks = new List<PointD>();   /* 存储最终结果（找到的峰值点） */
            var valuePosition = new List<int>();   /* 存储检测到的峰值的索引 */
            int slideCount = data.Length / slideWidth;    /* 计算数据滑动的次数 */
            double[] slideAve = new double[slideCount];   /* 存储每段滑动窗口的平均值 */
            double allAve = 0;                             /* 所有滑动窗口的平均值 */
            int cnt = 0;                                    /* 计数器，遍历数据时的索引 */

            /* 计算每个滑动窗口的平均值以及整体的平均值 */
            for (int i = 0; i < slideCount; i++)
            {
                for (int j = 0; j < slideWidth; ++j)
                {
                    slideAve[i] += data[cnt];
                    cnt++;
                }
                slideAve[i] /= slideWidth;
                allAve += slideAve[i];
            }
            allAve /= slideCount;

            // 峰值检测，将不符合的窗口删去，将可能有峰的窗口保留
            for (int i = 0; i < slideCount; i++)
            {
                if (slideAve[i] >= allAve * 3)
                {
                    valuePosition.Add(i);
                }
                else
                {
                    slideAve[i] = 0;
                }
            }

            /* 如果没有可能有峰的窗口即信号是白噪声直接返回 */
            if (valuePosition.Count == 0)
            {
                return resultPeaks;
            }

            /* 特别判断第一个可能有峰的窗口是否是第一个窗口或最后一个窗口 */
            int valuePositionIndex = valuePosition[0];  /* 有效峰值位置索引 */
            if (valuePositionIndex != 0 && valuePositionIndex != slideCount - 1)
            {
                /*如果该窗口平均值大于前后两个窗口的的平均值则判断有峰，并寻找峰*/
                if (slideAve[valuePositionIndex - 1] < slideAve[valuePositionIndex] && slideAve[valuePositionIndex] > slideAve[valuePositionIndex + 1])
                {
                    int startIndex = valuePositionIndex * slideWidth;
                    int endIndex = startIndex + slideWidth;
                    int maxIndex = startIndex;
                    double maxValue = data[startIndex];

                    for (int j = startIndex; j < endIndex; j++)
                    {
                        if (data[j] > maxValue)
                        {
                            maxIndex = j;
                            maxValue = data[j];
                        }
                    }
                    resultPeaks.Add(new PointD(maxIndex, maxValue));
                }
            }

            /* 循环遍历每一个可能有峰的窗口 */
            for (int i = 1; i < valuePosition.Count - 1; i++)
            {
                valuePositionIndex = valuePosition[i];
                if (slideAve[valuePositionIndex - 1] < slideAve[valuePositionIndex] && slideAve[valuePositionIndex] > slideAve[valuePositionIndex + 1])
                {
                    int startIndex = valuePositionIndex * slideWidth;
                    int endIndex = startIndex + slideWidth;
                    int maxIndex = startIndex;
                    double maxValue = data[startIndex];

                    for (int j = startIndex; j < endIndex; j++)
                    {
                        if (data[j] > maxValue)
                        {
                            maxIndex = j;
                            maxValue = data[j];
                        }
                    }
                    // 检查峰值是否过于接近
                    if (resultPeaks.Count != 0 && (maxIndex - resultPeaks.Last().X < peakInterval))
                    {
                        if (maxValue > resultPeaks.Last().Y)
                        {
                            resultPeaks[resultPeaks.Count - 1] = new PointD(maxIndex, maxValue);
                        }
                    }
                    else
                    {
                        resultPeaks.Add(new PointD(maxIndex, maxValue));
                    }
                }
            }

            /* 判断最后一个可能有峰的窗口是否只有一个以及是否是最后一个来做特殊处理 */
            valuePositionIndex = valuePosition[valuePosition.Count - 1];
            if (valuePosition.Count != 1 && valuePositionIndex != slideCount - 1)
            {
                if (slideAve[valuePositionIndex - 1] < slideAve[valuePositionIndex] && slideAve[valuePositionIndex] > slideAve[valuePositionIndex + 1])
                {
                    int startIndex = valuePositionIndex * slideWidth;
                    int endIndex = startIndex + slideWidth;
                    int maxIndex = startIndex;
                    double maxValue = data[startIndex];

                    for (int j = startIndex; j < endIndex; j++)
                    {
                        if (data[j] > maxValue)
                        {
                            maxIndex = j;
                            maxValue = data[j];
                        }
                    }
                    // 检查峰值是否过于接近
                    if (resultPeaks.Count != 0 && (maxIndex - resultPeaks.Last().X < peakInterval))
                    {
                        if (maxValue > resultPeaks.Last().Y)
                        {
                            resultPeaks[resultPeaks.Count - 1] = new PointD(maxIndex, maxValue);
                        }
                    }
                    else
                    {
                        resultPeaks.Add(new PointD(maxIndex, maxValue));
                    }
                }
            }
            // 限制峰值数量
            resultPeaks = resultPeaks.OrderByDescending(p => p.Y).Take(Math.Min(peakPointNum, resultPeaks.Count)).ToList();
            return resultPeaks;
        }
    }
}
