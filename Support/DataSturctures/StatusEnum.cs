using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace DataSturctures
{
    /// <summary>
    /// 状态枚举
    /// </summary>
    public static class StatusEnum
    {
        public enum ProgramLanguageEnum
        {
            /// <summary>
            /// 无法确定
            /// </summary>
            Unknow = 0,
            /// <summary>
            /// C#
            /// </summary>
            [Description(".csproj")]
            Csharp = 1,
        }

        /// <summary>
        /// 支持的数组类型枚举
        /// </summary>
        public enum SupportValueTypeEnum
        {
            /// <summary>
            /// short数组
            /// </summary>
            [Description("short[]")]
            ShortArrary = 0,
            /// <summary>
            /// int数组
            /// </summary>
            [Description("int[]")]
            IntArrary = 1,
            /// <summary>
            /// float数组
            /// </summary>
            [Description("float[]")]
            FloatArrary = 2,
            /// <summary>
            /// double数组
            /// </summary>
            [Description("double[]")]
            DoubleArrary = 3,
        }

        /// <summary>
        /// 获取枚举的描述信息，获取失败抛出异常
        /// </summary>
        /// <param name="value">目标枚举</param>
        /// <returns>返回获取到的信息</returns>
        /// <exception cref="NullReferenceException">当未获取到描述信息时抛出该异常</exception>"
        public static string GetDescription(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            DescriptionAttribute attribute = field?.GetCustomAttribute<DescriptionAttribute>() ??
                                             throw new NullReferenceException($"{value}.{nameof(DescriptionAttribute)} is Null!");
            return attribute.Description;
        }
    }
}
