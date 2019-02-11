/**
 * 单例
 */

namespace Base.Common
{
    public abstract class Singleton<T> where T : class, new()
    {
        private static T mInstance;

        public static T Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new T();
                }
                return mInstance;
            }
        }
    }
}
