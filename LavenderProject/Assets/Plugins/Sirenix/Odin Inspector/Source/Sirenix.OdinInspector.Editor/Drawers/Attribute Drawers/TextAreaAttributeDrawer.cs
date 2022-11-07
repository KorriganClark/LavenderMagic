#if UNITY_EDITOR
//-----------------------------------------------------------------------
// <copyright file="TextAreaAttributeDrawer.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Sirenix.OdinInspector.Editor.Drawers
{
#pragma warning disable

    using Sirenix.Utilities;
    using Sirenix.Utilities.Editor;
    using System;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// TextArea attribute drawer.
    /// </summary>
    public class TextAreaAttributeDrawer : OdinAttributeDrawer<TextAreaAttribute, string>
    {
        private delegate string ScrollableTextAreaInternalDelegate(Rect position, string text, ref Vector2 scrollPosition, GUIStyle style);

        private static readonly ScrollableTextAreaInternalDelegate EditorGUI_ScrollableTextAreaInternal;

        static TextAreaAttributeDrawer()
        {
            var method = typeof(EditorGUI).GetMethod("ScrollableTextAreaInternal", Flags.StaticAnyVisibility);

            if (method != null)
            {
                EditorGUI_ScrollableTextAreaInternal = (ScrollableTextAreaInternalDelegate)Delegate.CreateDelegate(typeof(ScrollableTextAreaInternalDelegate), method);
            }
        }

        private Vector2 scrollPosition;

        /// <summary>
        /// Draws the property in the Rect provided. This method does not support the GUILayout, and is only called by DrawPropertyImplementation if the GUICallType is set to Rect, which is not the default.
        /// If the GUICallType is set to Rect, both GetRectHeight and DrawPropertyRect needs to be implemented.
        /// If the GUICallType is set to GUILayout, implementing DrawPropertyLayout will suffice.
        /// </summary>
        /// <param name="label">The label. This can be null, so make sure your drawer supports that.</param>
        protected override void DrawPropertyLayout(GUIContent label)
        {
            var entry = this.ValueEntry;
            var attribute = this.Attribute;

            var stringValue = entry.SmartValue;
            var num = EditorStyles.textArea.CalcHeight(GUIHelper.TempContent(stringValue), GUIHelper.ContextWidth);
            var num2 = Mathf.Clamp(Mathf.CeilToInt(num / 13f), attribute.minLines, attribute.maxLines);
            var height = 32f + (float)((num2 - 1) * 13);
            var position = EditorGUILayout.GetControlRect(label != null, height);

            if (EditorGUI_ScrollableTextAreaInternal == null)
            {
                EditorGUI.LabelField(position, label, GUIHelper.TempContent("Cannot draw TextArea because Unity's internal API has changed."));
                return;
            }

            if (label != null)
            {
                Rect labelPosition = position;
                labelPosition.height = 16f;
                position.yMin += labelPosition.height;
                GUIHelper.IndentRect(ref labelPosition);
                EditorGUI.HandlePrefixLabel(position, labelPosition, label);
            }

            entry.SmartValue = EditorGUI_ScrollableTextAreaInternal(position, entry.SmartValue, ref this.scrollPosition, EditorStyles.textArea);
        }
    }
}
#endif