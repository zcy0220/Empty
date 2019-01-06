/**
 * 文件处理工具
 */

using System.IO;
using Base.Debug;

namespace Base.Utils
{
    public class FileUtil
    {
        /// <summary>
        /// 检测文件并创建对应文件夹
        /// </summary>
        public static bool CheckFileAndCreateDirWhenNeeded(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return false;
            var fileInfo = new FileInfo(filePath);
            var dirInfo = fileInfo.Directory;
            if (!dirInfo.Exists) Directory.CreateDirectory(dirInfo.FullName);
            return true;
        }

        /// <summary>
        /// Writes all text.
        /// </summary>
        public static bool WriteAllText(string outFile, string outText)
        {
            try
            {
                if (!CheckFileAndCreateDirWhenNeeded(outFile)) return false;
                if (File.Exists(outFile)) File.SetAttributes(outFile, FileAttributes.Normal);
                File.WriteAllText(outFile, outText);
                return true;
            }
            catch (System.Exception e)
            {
                Debugger.LogError("WriteAllText failed! path = {0} with err = {1}", outFile, e.Message);
                return false;
            }
        }

        /// <summary>
        /// Writes all bytes.
        /// </summary>
        public static bool WriteAllBytes(string outFile, byte[] outBytes)
        {
            try
            {
                if (!CheckFileAndCreateDirWhenNeeded(outFile)) return false;
                if (File.Exists(outFile)) File.SetAttributes(outFile, FileAttributes.Normal);
                File.WriteAllBytes(outFile, outBytes);
                return true;
            }
            catch (System.Exception e)
            {
                Debugger.LogError("WriteAllBytes failed! path = {0} with err = {1}", outFile, e.Message);
                return false;
            }
        }
    }
}
