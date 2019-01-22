#if UNITY_2017_2_OR_NEWER

//-----------------------------------------------------------------------
// <copyright file="VectorIntDrawer.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Sirenix.OdinInspector.Editor.Drawers
{
    using Utilities.Editor;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Vector2Int proprety drawer.
    /// </summary>
    [OdinDrawer]
    public sealed class Vector2IntDrawer : OdinValueDrawer<Vector2Int>, IDefinesGenericMenuItems
    {
        /// <summary>
        /// Draws the property.
        /// </summary>
        protected override void DrawPropertyLayout(IPropertyValueEntry<Vector2Int> entry, GUIContent label)
        {
            GUILayout.BeginHorizontal();

            if (label != null)
            {
                EditorGUI.BeginChangeCheck();
                var value = SirenixEditorFields.VectorPrefixLabel(label, (Vector2)entry.SmartValue);
                if (EditorGUI.EndChangeCheck())
                {
                    entry.SmartValue = new Vector2Int((int)value.x, (int)value.y);
                }
            }

            var r = GUIHelper.GetCurrentLayoutRect();
            bool showLabels = !(SirenixEditorFields.ResponsiveVectorComponentFields && (label != null ? r.width - EditorGUIUtility.labelWidth : r.width) < 185);

            GUIHelper.PushLabelWidth(SirenixEditorFields.SingleLetterStructLabelWidth);
            GUIHelper.PushIndentLevel(0);
            entry.Property.Children[0].Draw(showLabels ? GUIHelper.TempContent("X") : null);
            entry.Property.Children[1].Draw(showLabels ? GUIHelper.TempContent("Y") : null);
            GUIHelper.PopIndentLevel();
            GUIHelper.PopLabelWidth();
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// Populates the generic menu for the property.
        /// </summary>
        public void PopulateGenericMenu(InspectorProperty property, GenericMenu genericMenu)
        {
            Vector2Int value = (Vector2Int)property.ValueEntry.WeakSmartValue;

            if (genericMenu.GetItemCount() > 0)
            {
                genericMenu.AddSeparator("");
            }
            genericMenu.AddItem(new GUIContent("Zero", "Set the vector to (0, 0)"), value == Vector2Int.zero, () => SetVector(property, Vector2Int.zero));
            genericMenu.AddItem(new GUIContent("One", "Set the vector to (1, 1)"), value == Vector2Int.one, () => SetVector(property, Vector2Int.one));
            genericMenu.AddSeparator("");
            genericMenu.AddItem(new GUIContent("Right", "Set the vector to (1, 0)"), value == Vector2Int.right, () => SetVector(property, Vector2Int.right));
            genericMenu.AddItem(new GUIContent("Left", "Set the vector to (-1, 0)"), value == Vector2Int.left, () => SetVector(property, Vector2Int.left));
            genericMenu.AddItem(new GUIContent("Up", "Set the vector to (0, 1)"), value == Vector2Int.up, () => SetVector(property, Vector2Int.up));
            genericMenu.AddItem(new GUIContent("Down", "Set the vector to (0, -1)"), value == Vector2Int.down, () => SetVector(property, Vector2Int.down));
        }

        private void SetVector(InspectorProperty property, Vector2Int value)
        {
            property.Tree.DelayActionUntilRepaint(() =>
            {
                for (int i = 0; i < property.ValueEntry.ValueCount; i++)
                {
                    property.ValueEntry.WeakValues[i] = value;
                }
            });
        }
    }

    /// <summary>
    /// Vector3Int property drawer.
    /// </summary>
    [OdinDrawer]
    public sealed class Vector3IntDrawer : OdinValueDrawer<Vector3Int>, IDefinesGenericMenuItems
    {
        /// <summary>
        /// Draws the property.
        /// </summary>
        protected override void DrawPropertyLayout(IPropertyValueEntry<Vector3Int> entry, GUIContent label)
        {
            if (label == null)
            {
                SirenixEditorGUI.BeginIndentedHorizontal(EditorGUI.indentLevel == 0 ? GUIStyle.none : SirenixGUIStyles.PropertyMargin);
            }
            else
            {
                GUILayout.BeginHorizontal();
            }

            if (label != null)
            {
                EditorGUI.BeginChangeCheck();
                var value = SirenixEditorFields.VectorPrefixLabel(label, (Vector3)entry.SmartValue);
                if (EditorGUI.EndChangeCheck())
                {
                    entry.SmartValue = new Vector3Int((int)value.x, (int)value.y, (int)value.z);
                }
            }

            var r = GUIHelper.GetCurrentLayoutRect();
            bool showLabels = !(SirenixEditorFields.ResponsiveVectorComponentFields && (label != null ? r.width - EditorGUIUtility.labelWidth : r.width) < 185);

            GUIHelper.PushLabelWidth(SirenixEditorFields.SingleLetterStructLabelWidth);
            GUIHelper.PushIndentLevel(0);
            entry.Property.Children[0].Draw(showLabels ? GUIHelper.TempContent("X") : null);
            entry.Property.Children[1].Draw(showLabels ? GUIHelper.TempContent("Y") : null);
            entry.Property.Children[2].Draw(showLabels ? GUIHelper.TempContent("Z") : null);
            GUIHelper.PopIndentLevel();
            GUIHelper.PopLabelWidth();

            if (label == null)
            {
                SirenixEditorGUI.EndIndentedHorizontal();
            }
            else
            {
                GUILayout.EndHorizontal();
            }
        }

        /// <summary>
        /// Populates the generic menu for the property.
        /// </summary>
        public void PopulateGenericMenu(InspectorProperty property, GenericMenu genericMenu)
        {
            Vector3Int value = (Vector3Int)property.ValueEntry.WeakSmartValue;

            if (genericMenu.GetItemCount() > 0)
            {
                genericMenu.AddSeparator("");
            }
            
            genericMenu.AddItem(new GUIContent("Zero", "Set the vector to (0, 0, 0)"), value == Vector3Int.zero, () => SetVector(property, Vector3Int.zero));
            genericMenu.AddItem(new GUIContent("One", "Set the vector to (1, 1, 1)"), value == Vector3Int.one, () => SetVector(property, Vector3Int.one));
            genericMenu.AddSeparator("");
            genericMenu.AddItem(new GUIContent("Right", "Set the vector to (1, 0, 0)"), value == Vector3Int.right, () => SetVector(property, Vector3Int.right));
            genericMenu.AddItem(new GUIContent("Left", "Set the vector to (-1, 0, 0)"), value == Vector3Int.left, () => SetVector(property, Vector3Int.left));
            genericMenu.AddItem(new GUIContent("Up", "Set the vector to (0, 1, 0)"), value == Vector3Int.up, () => SetVector(property, Vector3Int.up));
            genericMenu.AddItem(new GUIContent("Down", "Set the vector to (0, -1, 0)"), value == Vector3Int.down, () => SetVector(property, Vector3Int.down));
            genericMenu.AddItem(new GUIContent("Forward", "Set the vector property to (0, 0, 1)"), value == new Vector3Int(0, 0, 1), () => SetVector(property, new Vector3Int(0, 0, 1)));
            genericMenu.AddItem(new GUIContent("Back", "Set the vector property to (0, 0, -1)"), value == new Vector3Int(0, 0, -1), () => SetVector(property, new Vector3Int(0, 0, -1)));
        }

        private void SetVector(InspectorProperty property, Vector3Int value)
        {
            property.Tree.DelayActionUntilRepaint(() =>
            {
                property.ValueEntry.WeakSmartValue = value;
            });
        }
    }
}

#endif