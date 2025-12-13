using System.IO;
using System.Text;
using DataSturctures;

namespace FunctionalService
{
    /// <summary>
    /// 文档导出服务接口
    /// </summary>
    public interface IFileExportService
    {
        /// <summary>
        /// 导出数据到CSV文件
        /// </summary>
        /// <param name="csvFilePath">目标路径</param>
        /// <param name="debugData">数据</param>
        void FileCsv_Export(string csvFilePath, DebugDataSaveStructures debugData);
    }
    /// <summary>
    /// 文档导出服务实现
    /// </summary>
    public class FileExportService : IFileExportService
    {
        /// <inheritdoc/>
        public void FileCsv_Export(string csvFilePath, DebugDataSaveStructures debugData)
        {
            using FileStream fileStream = File.Open(csvFilePath, FileMode.Create);
            using var writer = new StreamWriter(fileStream, Encoding.UTF8);
            writer.WriteLine(debugData.Information + debugData.SaveTime.ToString("yyyy-MM-dd HH-mm-ss-fff"));//第一行
            writer.WriteLine("Index, debugData");
            var stringBuilder = new StringBuilder();
            for (int i = 0; i < debugData.Data.Length; i++)
                stringBuilder.AppendLine((i + 1) + "," + debugData.Data[i].ToString("0.00"));
            writer.Write(stringBuilder.ToString());
        }
    }
}
