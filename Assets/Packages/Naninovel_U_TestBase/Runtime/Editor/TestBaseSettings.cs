using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Naninovel.U.TestBase
{
    public class TestBaseSettings : ConfigurationSettings<TestBaseConfiguration>
    {
        protected override Dictionary<string, Action<SerializedProperty>> OverrideConfigurationDrawers()
        {
            var drawers = base.OverrideConfigurationDrawers();
            drawers[nameof(TestBaseConfiguration.API)] = DrawContextKeyItemsEditor;
            return drawers;
        }

        private void DrawContextKeyItemsEditor(SerializedProperty property)
        {
            var label = EditorGUI.BeginProperty(Rect.zero, null, property);
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);

            var text =
                SyntaxHighlighter.ColorizeSyntax("@test ID //hello world");

            var richTextStyle = new GUIStyle(EditorStyles.textArea) { richText = true, wordWrap = true };
            GUILayout.TextArea(text, richTextStyle);

            EditorGUILayout.HelpBox(
                "Hello World!", MessageType.Info);

            EditorGUI.EndProperty();
        }
    }
}
