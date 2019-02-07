/**
 * 程序主入口
 */

using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class AppMain : MonoBehaviour
{
    /// <summary>
    /// 开始游戏，初始化配置文件
    /// </summary>
    private void Awake()
    {
        //SheetTest.Test();
        //Test();
    }

    private void Test()
    {
        //var abConfig = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/assetbundleconfig");
        //var textAsset = abConfig.LoadAsset<TextAsset>("AssetBundleConfig");
        //var stream = new MemoryStream(textAsset.bytes);
        //var bf = new BinaryFormatter();
        //var assetBundleConfig = (AssetBundleConfig)bf.Deserialize(stream);
        //stream.Close();
        //string path = "Assets/GameAssets/Prefabs/ExamplePrefab1.prefab";
        //var md5 = Base.Utils.FileUtil.GetMD5HashFromFile(path);
        //AssetBundleBase abBase = null;
        //for (var i = 0; i < assetBundleConfig.AssetBundleList.Count; i++)
        //{
        //    if (assetBundleConfig.AssetBundleList[i].MD5 == md5)
        //    {
        //        abBase = assetBundleConfig.AssetBundleList[i];
        //    }
        //}
        //for (var i = 0; i < abBase.ABDependList.Count; i++)
        //{
        //    AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + abBase.ABDependList[i]);
        //}
        //var assetBundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + abBase.ABName);
        //GameObject.Instantiate(assetBundle.LoadAsset<GameObject>(abBase.AssetName));
    }
}
