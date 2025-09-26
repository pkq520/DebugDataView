using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DebugDataViewCore.Support.DataStructures;

namespace DebugDataViewCore.Support
{
    public static class ToolExtensions
    {
        /// <summary>
        /// 线性处理
        /// </summary>
        /// <param name="value">待计算的数</param>
        /// <param name="param">线性参数</param>
        /// <returns>计算后的数</returns>
        public static double Linear_Process(this double value, ILinearParameter param)
        {
            return param.LinearK * value + param.LinearB;
        }

        /// <summary>
        /// 线性处理
        /// </summary>
        /// <param name="value">待计算的数</param>
        /// <param name="param">线性参数</param>
        /// <returns>计算后的数</returns>
        public static double Linear_Process(this int value, ILinearParameter param)
        {
            return param.LinearK * value + param.LinearB;
        }

        /// <summary>
        /// 线性反处理
        /// </summary>
        /// <param name="value">待计算的数</param>
        /// <param name="param">线性参数</param>
        /// <returns>反计算后的数</returns>
        public static double Linear_Unprocess(this double value, ILinearParameter param)
        {
            return (value - param.LinearB) / param.LinearK;
        }

        /// <summary>
        /// 线性反处理
        /// </summary>
        /// <param name="value">待计算的数</param>
        /// <param name="param">线性参数</param>
        /// <returns>反计算后的数</returns>
        public static double Linear_Unprocess(this int value, ILinearParameter param)
        {
            return (value - param.LinearB) / param.LinearK;
        }
    }
}
