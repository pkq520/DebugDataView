namespace DataSturctures
{
    /// <summary>
    /// 线性系数接口
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
    /// 线性系数实现
    /// </summary>
    public class LinearParameter : ILinearParameter
    {
        /// <inheritdoc/>
        public double LinearK { get; set; } = 0.001;
        /// <inheritdoc/>
        public double LinearB { get; set; }

        /// <summary>
        /// 无参构造函数
        /// </summary>
        public LinearParameter() { }

        /// <summary>
        /// 有参构造函数
        /// </summary>
        /// <param name="linearK">系数k</param>
        /// <param name="linearB">系数b</param>
        public LinearParameter(double linearK, double linearB)
        {
            LinearK = linearK;
            LinearB = linearB;
        }
    }
}
