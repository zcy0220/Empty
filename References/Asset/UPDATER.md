# 热更新
* [构建本地服务器](../../Server/README.md)
* 加载本地和服务器上的版本配置文件进行对比。版本有差异，则加载本地和服务器上的AssetBundleManifest，对比资源Hash, 把差异和新增资源加入到下载列表中。加载完成后写入Application.persistentDataPath。
* 资源加载，优先Application.persistentDataPath路径下的资源

### 热更新核心代码
~~~C#
/// <summary>
/// 检查更新，包括大版本换包和热更新
/// </summary>
private IEnumerator CheckUpdate()
{
    // 是否开启热更检测
    if (!AppConfig.CheckVersionUpdate)
    {
        StartGame();
        yield break;
    }
    // 加载并初始化版本信息文件
    yield return InitVersion();

    // 没有版本对比文件，直接进入游戏
    if (mLocalVersionConfig == null || mServerVersionConfig == null)
    {
        StartGame();
        yield break;
    }

    // 检测版本配置文件
    if (CheckVersion(mLocalVersionConfig.Version, mServerVersionConfig.Version))
    {
        // 对比版本资源
        yield return CompareVersion();
    }
    if (mNeedDownLoadList.Count == 0)
    {
        StartGame();
    }
    else
    {
        DownLoadResource();
    }
}

/// <summary>
/// 下载资源
/// </summary>
private void DownLoadResource()
{
    if (mNeedDownLoadList.Count == 0)
    {
        UpdateVersionConfig();
        ReplaceLocalResource(AssetBundleConfig.AssetBundlesFolder, mServerManifestData);
        StartGame();
        return;
    }
    var abName = mNeedDownLoadList[0];
    Debugger.Log(StringUtil.Concat("DownLoad ", abName));
    mNeedDownLoadList.RemoveAt(0);
    var abPath = StringUtil.PathConcat(AssetBundleConfig.AssetBundlesFolder, abName);
    var url = PathUtil.GetServerFileURL(abPath);
    StartCoroutine(DownLoad(url, (www) =>
    {
        ReplaceLocalResource(abName, www.bytes);
        DownLoadResource();
    }));
}
~~~

### 热更新结果
