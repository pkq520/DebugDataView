namespace DataSturctures.Extensions
{
    /// <summary>
    /// 结构体扩展
    /// </summary>
    public class StructExtensions
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
    }
}
