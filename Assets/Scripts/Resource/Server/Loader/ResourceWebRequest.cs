/**
 * 普通资源请求， 
 */

using UnityEngine;
using Base.Debug;

namespace Resource
{
    public class ResourceWebRequest : WebRequest
    {
        private WWW www;
        public string URL { get; private set; }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init(string assetName, string url)
        {
            AssetName = assetName;
            URL = url;
        }

        /// <summary>
        /// 开始加载
        /// </summary>
        public override void Start()
        {
            www = new WWW(URL);
            Debugger.Log(www == null, string.Format("Unity web request failed, url: {0}", URL));
        }
    }
}
