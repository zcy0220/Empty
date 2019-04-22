/**
 * 构建打包
 */

using UnityEditor;
using System.Collections.Generic;
using Assets.Editor.AssetBundle;

public class BuildScript
{
    /// <summary>
    /// 打包的场景列表
    /// </summary>
    private static string[] GetBuildScenes()
    {
        List<string> names = new List<string>();
        foreach (var scene in EditorBuildSettings.scenes)
        {
            if (scene == null) continue;
            if (scene.enabled) names.Add(scene.path);
        }
        return names.ToArray();
    }

    /// <summary>
    /// 获得shell传入的参数
    /// </summary>
    /// <returns>The arguments.</returns>
    private static Dictionary<string, string> GetArgs()
    {
        var args = new Dictionary<string, string>();
        foreach (string arg in System.Environment.GetCommandLineArgs())
        {
            if (arg.StartsWith("@"))
            {
                var splitIndex = arg.IndexOf("=");
                args.Add(arg.Substring(1, splitIndex - 1), arg.Substring(splitIndex + 1));
            }
        }
        return args;
    }

    /// <summary>
    /// Builds for android.
    /// </summary>
    public static void BuildForAndroid()
    {
        var args = GetArgs();
        PlayerSettings.productName = args["name"];
        PlayerSettings.bundleVersion = args["version"];
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
        AssetBundleBuilder.Build();
        BuildPipeline.BuildPlayer(GetBuildScenes(), args["out"], BuildTarget.Android, BuildOptions.None);
    }

    /// <summary>
    /// Builds for ios.
    /// </summary>
    public static void BuildForIOS()
    {
        var args = GetArgs();
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, BuildTarget.iOS);
        AssetBundleBuilder.Build();
        BuildPipeline.BuildPlayer(GetBuildScenes(), args["out"], BuildTarget.iOS, BuildOptions.None);
    }
}
