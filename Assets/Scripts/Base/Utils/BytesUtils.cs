/**
 * 字节数组处理工具
 */


namespace Base.Utils
{
    public class BytesUtil
    {
        /// <summary>
        /// Reads the int.
        /// </summary>
        public static int ReadInt(byte[] buffer, int start)
        {
            return (buffer[start + 0] << 24 | buffer[start + 1] << 16 | buffer[start + 2] << 08 | buffer[start + 3] << 00);
        }
    }
}