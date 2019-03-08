/**
 * AssetBundle异步加载器
 */

using System;

namespace Resource
{
    public class AssetBundleAsyncLoader : AsyncLoader
    {
        public override float Progress()
        {
            return 1.0f;
        }

        public override void Update()
        {
        }
    }
}
