using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DataSturctures
{
    /// <summary>
    /// 状态枚举
    /// </summary>
    public class StatusEnum
    {
        public enum ProgramLanguageEnum
        {
            /// <summary>
            /// 无法确定
            /// </summary>
            Unknow,
            /// <summary>
            /// C#
            /// </summary>
            Csharp
        }

        /// <summary>
        /// 支持的数组类型枚举
        /// </summary>
        public enum SupportTypeEnum
        {
            Int,
            Short,
            Float,
            Double,
        }
    }
}
