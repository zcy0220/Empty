/**
 * Unity相关拓展
 */

using UnityEngine;

namespace Base.Extension
{
    public static class UnityEx
    {
        /// <summary>
        /// 获得或添加组件
        /// </summary>
        /// <param name="self">GameObject</param>
        public static T GetOrAddComponent<T>(this GameObject self) where T : Component
        {
            T component = self.GetComponent<T>();
            if (component == null) component = self.AddComponent<T>();
            return component;
        }

        /// <summary>
        /// 获得或添加组件
        /// </summary>
        /// <param name="self">Transform</param>
        public static T GetOrAddComponent<T>(this Transform self) where T : Component
        {
            return GetOrAddComponent<T>(self.gameObject);
        }
    }
}

