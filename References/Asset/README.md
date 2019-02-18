# 资源管理
* AssetBundle打包管理
* Resource管理

## Unity资源加载方式
* Resources：[官方不推荐](https://unity3d.com/cn/learn/tutorials/topics/best-practices/resources-folder)
* AssetBundle
* AssetDataBase

### AssetBundle打包管理
* 配置打包策略：一开始准备全自动打包，即默认每个文件打包成一个AssetBundle，但考虑到粒度太细，会导致后期AssetBundle的数量过多等问题。所以这里先给出手动配置策略的方法。到具体项目时，可以确定下打包策略后，修改手动配置相关为自动配置，省去这步。
* 配置ABConfig：Assets\Editor\AssetBundle\ABConfig.asset
* 一键打包: Tools->AssetBundle->Build


### Resource管理
* 类对象池：
    