# Jenkins管理打包
* [官网下载](https://jenkins.io)

## 个人建议
* 能写在shell里的尽量写在shell里面。Jenkins确实提供了很多配置，但项目久了回头来看时，东一项西一项的反而容易乱
* 不要为了拓展性，而把所有能配置的参数都暴露在外面配置。一些你认为要配置的信息，对于打包的交接人来说并不需要

## Jenkins权限问题
* Jenkins默认安装在/Users/Shared/Jenkins，并没有自己用户的权限
* 我遇到的第一个问题是Git的权限。如果要用Jenkins的Git管理仓库，可以避开这个问题，但它会默认把仓库下载到它自己的目录下面管理
* 我个人更偏向Git也是在shell中去控制，同时之后还可能遇到除了Git之外所需的权限问题
* 综上，正式解决方案
~~~
sudo launchctl unload /Library/LaunchDaemons/org.jenkins-ci.plist
sudo vim /Library/LaunchDaemons/org.jenkins-ci.plist
~~~
* 修改群组名和用户名(系统偏好设置->用户与群组)，我的默认群组用不了，所以新建了JenkinsGroup群组，加了自己和Jenkins两个成员用户
![Config](Images/001.png)
* 修改完成后，重启Jenkins
~~~
sudo chown -R 用户名:群组名 /Users/Shared/Jenkins/
sudo chown -R 用户名:群组名 /var/log/jenkins/
sudo launchctl load /Library/LaunchDaemons/org.jenkins-ci.plist
~~~