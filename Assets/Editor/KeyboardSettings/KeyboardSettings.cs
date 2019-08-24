/**
 * 快捷键设置
 */

using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

namespace Assets.Editor.KeyboardSettings
{
    public class KeyboardSettings
    {
        private static string START_SCENE = "Assets/Scenes/StartScene.unity";

        /// <summary>
        /// 游戏开始快捷键
        /// </summary>
        [MenuItem("Tools/KeyboardSettings/Play _F5")]
        private static void Play()
        {
            if (EditorApplication.isPlaying)
            {
                EditorApplication.isPlaying = false;
            }
            else
            {
                AssetDatabase.Refresh();
                Scene activeScene = EditorSceneManager.GetActiveScene();
                if (!string.IsNullOrEmpty(activeScene.path) && (activeScene.path != START_SCENE)) EditorSceneManager.SaveOpenScenes();
                EditorSceneManager.OpenScene(START_SCENE);
                EditorApplication.isPlaying = true;
            }
        }
    }
}
