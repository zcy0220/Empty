/**
 * 字符串处理工具类
 */

using System.Text;

namespace Base.Utils
{
    public class StringUtil
    {
        private static StringBuilder mCache = new StringBuilder(256);

        public static string Concat(string param1, int param2)
        {
            mCache.Length = 0;
            mCache.Append(param1);
            mCache.Append(param2);
            return mCache.ToString();
        }

        public static string Concat(string param1, float param2)
        {
            mCache.Length = 0;
            mCache.Append(param1);
            mCache.Append(param2);
            return mCache.ToString();
        }

        public static string Concat(string param1, string param2)
        {
            mCache.Length = 0;
            mCache.Append(param1);
            mCache.Append(param2);
            return mCache.ToString();
        }
    }
}
