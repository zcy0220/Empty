/**
 * Mono单例
 */

using UnityEngine;

namespace Base.Common
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T mInstance;

        public static T Instance
        {
            get
            {
                if (mInstance == null)
                {
                    var go = new GameObject(typeof(T).Name);
                    mInstance = go.AddComponent<T>();
                    GameObject.DontDestroyOnLoad(go);
                }
                return mInstance;
            }
        }

        public void Destroy()
        {
            Dispose();
            GameObject.Destroy(gameObject);
            mInstance = null;
        }

        public virtual void Dispose() { }
    }
}
