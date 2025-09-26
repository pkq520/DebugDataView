using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebugDataViewCore.Support
{
    public class DataStructures
    {
        /// <summary>
        /// 系数接口
        /// </summary>
        public interface ILinearParameter
        {
            /// <summary>
            /// 系数k
            /// </summary>
            double LinearK { get; set; }
            /// <summary>
            /// 系数b
            /// </summary>
            double LinearB { get; set; }
        }

        /// <summary>
        /// 校准系数类
        /// </summary>
        [Serializable]
        public class LinearParameterClass : ILinearParameter
        {
            /// <inheritdoc/>
            public double LinearK { get; set; } = 0.001;
            /// <inheritdoc/>
            public double LinearB { get; set; }

            /// <summary>
            /// 无参构造函数
            /// </summary>
            public LinearParameterClass() { }

            /// <summary>
            /// 有参构造函数
            /// </summary>
            /// <param name="linearK">系数k</param>
            /// <param name="linearB">系数b</param>
            public LinearParameterClass(double linearK, double linearB)
            {
                LinearK = linearK;
                LinearB = linearB;
            }
        }

        /// <summary>
        /// 主要数据结构体
        /// </summary>
        public class DebugDataStructures
        {
            /// <summary>
            /// 信息
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
}
