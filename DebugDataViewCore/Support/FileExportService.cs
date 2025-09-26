using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DebugDataViewCore.Support.DataStructures;

namespace DebugDataViewCore.Support
{
    public interface IFileExportService
    {
        /// <summary>
        /// 导出数据到CSV文件
        /// </summary>
        /// <param name="csvFilePath">目标路径</param>
        /// <param name="debugData">需要被保存数据</param>
        void FileCsv_Export(string csvFilePath, DebugDataStructures debugData);
    }
    public class FileExportService : IFileExportService
    {
        /// <inheritdoc/>
        public void FileCsv_Export(string csvFilePath, DebugDataStructures debugData)
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
