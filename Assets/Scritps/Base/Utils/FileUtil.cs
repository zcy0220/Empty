/**
 * 文件处理工具
 */

using System.IO;
using Base.Common;

namespace Base.Utils
{
    public class FileUtil
    {
        public static bool SafeWriteAllText(string outFile, string text)
        {
            try
            {
                if (string.IsNullOrEmpty(outFile))
                {
                    return false;
                }

                //CheckFileAndCreateDirWhenNeeded(outFile);
                if (File.Exists(outFile))
                {
                    File.SetAttributes(outFile, FileAttributes.Normal);
                }
                File.WriteAllText(outFile, text);
                return true;
            }
            catch (System.Exception ex)
            {
                //Debugger.LogError(string.Format("SafeWriteAllText failed! path = {0} with err = {1}", outFile, ex.Message));
                return false;
            }
        }
    }
}
