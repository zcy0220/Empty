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

        public static string Concat(string param1, string param2, string param3)
        {
            mCache.Length = 0;
            mCache.Append(param1);
            mCache.Append(param2);
            mCache.Append(param3);
            return mCache.ToString();
        }

        public static string PathConcat(string path1, string path2)
        {
            mCache.Length = 0;
            mCache.Append(path1);
            mCache.Append("/");
            mCache.Append(path2);
            return mCache.ToString();
        }
    }
}
