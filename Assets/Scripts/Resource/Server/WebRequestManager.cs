/**
 * 网络和文件的www方式读取管理
 */

using Base.Common;
using Base.Pool;
using System.Collections.Generic;

namespace Resource
{
    public class WebRequestManager : MonoSingleton<WebRequestManager>
    {
        /// <summary>
        /// 请求加载的字典
        /// </summary>
        private Dictionary<string, WebRequest> mWebRequestDict = new Dictionary<string, WebRequest>();
        /// <summary>
        /// 请求加载队列
        /// </summary>
        private Queue<WebRequest> mWebRequestQueue = new Queue<WebRequest>();
        /// <summary>
        /// 正在处理的请求列表
        /// </summary>
        private List<WebRequest> mProsessingWebRequestList = new List<WebRequest>();
        /// <summary>
        /// 普通资源请求类对象池
        /// </summary>
        private ClassObjectPool<ResourceWebRequest> mResourceWebRequestPool = new ClassObjectPool<ResourceWebRequest>();
        
        /// <summary>
        /// 本地异步请求非AssetBundle资源 主要是版本号相关文件
        /// </summary>
        public ResourceWebRequest RequestResourceAsync(string path)
        {
            //var request = mResourceWebRequestPool.Spawn();
            //var url = PathUtil.GetStreamingAssetFilePath(path);
            //request.Init(path, url);
            //mWebRequestDict.Add(path, request);
            //mWebRequestQueue.Enqueue(request);
            //return request;
            return null;
        }
    }
}
