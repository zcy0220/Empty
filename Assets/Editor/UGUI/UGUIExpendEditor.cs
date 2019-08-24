/**
 * UGUI的拓展编辑
 */

using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Editor.UGUI
{
    public class UGUIExpendEditor
    {
        /// <summary>
        /// 修改创建UIText时的一些默认值
        /// </summary>
        [MenuItem("GameObject/UI/TextEx")]
        public static void CreateText()
        {
            if (Selection.activeObject && Selection.activeTransform.GetComponentInParent<Canvas>())
            {
                GameObject go = new GameObject("Text", typeof(Text));
                Text text = go.GetComponent<Text>();
                text.raycastTarget = false;
                text.fontSize = 24;
                text.alignment = TextAnchor.MiddleCenter;
                go.transform.SetParent(Selection.activeTransform);
                go.transform.localScale = Vector3.one;
            }
        }
    }
}
