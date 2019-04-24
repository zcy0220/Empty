/**
 * 文件处理工具
 */

using System.IO;
using System.Text;
using Base.Debug;

namespace Base.Utils
{
    public class FileUtil
    {
        /// <summary>
        /// 获取文件的MD5值
        /// </summary>
        /// <param name="filePath">文件绝对路径</param>
        /// <returns></returns>
        public static string GetMD5HashFromFile(string filePath)
        {
            try
            {
                var file = new FileStream(filePath, FileMode.Open);
                var md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                var retVal = md5.ComputeHash(file);
                file.Close();
                var sb = new StringBuilder();
                for (var i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (System.Exception e)
            {
                throw new System.Exception("GetMD5HashFromFile failed! err = " + e.Message);
            }
        }

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
        
        /// <summary>
        /// Read Text
        /// </summary>
        public static string ReadAllText(string inFile)
        {
            try
            {
                if (string.IsNullOrEmpty(inFile)) return null;
                if (!File.Exists(inFile)) return null;
                File.SetAttributes(inFile, FileAttributes.Normal);
                return File.ReadAllText(inFile);
            }
            catch (System.Exception e)
            {
                Debugger.LogError("ReadAllText failed! path = {0} with err = {1}", inFile, e.Message);
                return null;
            }
        }

        /// <summary>
        /// Read Bytes
        /// </summary>
        public static byte[] ReadAllBytes(string inFile)
        {
            try
            {
                if (string.IsNullOrEmpty(inFile)) return null;
                if (!File.Exists(inFile)) return null;
                File.SetAttributes(inFile, FileAttributes.Normal);
                return File.ReadAllBytes(inFile);
            }
            catch (System.Exception e)
            {
                Debugger.LogError("ReadAllBytes failed! path = {0} with err = {1}", inFile, e.Message);
                return null;
            }
        }
        
        /// <summary>
        /// 检测文件路径
        /// </summary>
        public static bool Exists(string path)
        {
            return File.Exists(path);
        }
    }
}
