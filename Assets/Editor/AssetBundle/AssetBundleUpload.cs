/**
 * 上传AssetBundle
 * 上传热更包跟实际项目关联性比较大
 * 这里只是复制到Server/public下面
 */

using UnityEditor;
using System.IO;
using UnityEngine;

namespace Assets.Editor.AssetBundle
{
    public class AssetBundleUpload
    {
        /// <summary>
        /// 上传路径
        /// </summary>
        private static string UPLOADPATH = "Server/public/AssetBundles";
        /// <summary>
        /// AssetBundles原路径
        /// </summary>
        private static string SOURCEPATH = Application.streamingAssetsPath + "/AssetBundles";
        
        [MenuItem("Tools/AssetBundle/Upload")]
        public static void Upload()
        {
            if (Directory.Exists(UPLOADPATH))
            {
                FileUtil.DeleteFileOrDirectory(UPLOADPATH);
            }
            FileUtil.CopyFileOrDirectory(SOURCEPATH, UPLOADPATH);
        }
    }
}
