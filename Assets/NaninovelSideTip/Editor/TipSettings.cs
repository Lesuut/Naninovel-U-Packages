using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Naninovel.U.SideTip
{
    public class TipSettings : ConfigurationSettings<TipConfiguration>
    {
        private const float headerLeftMargin = 5;
        private const float paddingWidth = 10;

        private static readonly GUIContent keyContent = new GUIContent("Key", "The <color=yellow>key</color> identifying the tip context item.");
        private static readonly GUIContent defaultValueContent = new GUIContent("Value", "<color=yellow>Source Locale</color> tip value for this key.");

        private ReorderableList contextKeyItemsList;

        protected override Dictionary<string, Action<SerializedProperty>> OverrideConfigurationDrawers()
        {
            var drawers = base.OverrideConfigurationDrawers();
            drawers[nameof(TipConfiguration.ContextKeyItems)] = DrawContextKeyItemsEditor;
            return drawers;
        }

        private void DrawContextKeyItemsEditor(SerializedProperty property)
        {
            var label = EditorGUI.BeginProperty(Rect.zero, null, property);
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);

            // Проверяем, что ReorderableList инициализирован
            if (contextKeyItemsList is null || contextKeyItemsList.serializedProperty.serializedObject != SerializedObject)
                InitializeContextKeyItemsList();

            // Рисуем ReorderableList
            contextKeyItemsList.DoLayoutList();

            if (GUILayout.Button("Open Tip Text Utility", GUIStyles.NavigationButton))
                TipTextWindow.OpenWindow();

            var text =
                "<b>API</b>\n\n" +
                "<color=#6cb2ed>@tip</color>  <color=#cd8843>key:</color><color=#e2be7f>\"key_value\"</color> - <color=#35a946>Fast</color> Open the tip window\n" +
                "<color=#6cb2ed>@tip</color> - <color=#35a946>Fast</color> Close the tip window\n\n" +
                "<color=#6cb2ed>@showTip</color>  <color=#cd8843>key:</color><color=#e2be7f>\"key_value\"</color> - Open the tip window\n" +
                "<color=#6cb2ed>@hideTip</color> - Close the tip window\n\n" +
                "<color=#6cb2ed>@if</color> <color=#e2be7f>IsCurrentTipKey(\"key1\")</color> - Сheck if the current tip key matches the input key\n" +
                "<color=#6cb2ed>@endif</color>\n\n" +
                "<color=#6cb2ed>@if</color> <color=#e2be7f>GetCurrentTipKey()==\"key2\"</color> - Return the current tip key\n" +
                "<color=#6cb2ed>@endif</color>\n";
            var richTextStyle = new GUIStyle(EditorStyles.textArea) { richText = true, wordWrap = true };
            GUILayout.TextArea(text, richTextStyle);

            EditorGUILayout.HelpBox(
                "Before you start, go to:\n" +
                "Project Settings/Nanonovel/UI/Manage UI Resources\n" +
                "and add the SideTipUI element to the list.\n" +
                "It can be found at:\n" +
                "Assets/NaninovelTip/Prefabs/SideTipUI", MessageType.Info);

            EditorGUI.EndProperty();
        }

        private void InitializeContextKeyItemsList()
        {
            contextKeyItemsList = new ReorderableList(
                SerializedObject,
                SerializedObject.FindProperty(nameof(TipConfiguration.ContextKeyItems)),
                true, // draggable
                true, // display header
                true, // allow adding
                true  // allow removing
            );

            // Настроим стиль заголовка
            contextKeyItemsList.drawHeaderCallback = DrawContextKeyItemsListHeader;

            // Настроим стиль элементов списка
            contextKeyItemsList.drawElementCallback = DrawContextKeyItemsListElement;
        }

        private void DrawContextKeyItemsListHeader(Rect rect)
        {
            /*var headerStyle = new GUIStyle(EditorStyles.label) // Просто цветные заоловки
            {
                normal = { textColor = new Color(166f / 255f, 222f / 255f, 255f / 255f) },
            };*/

            var propertyRect = new Rect(headerLeftMargin + rect.x, rect.y, (rect.width / 2f) - paddingWidth, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(propertyRect, keyContent/*, headerStyle*/);

            propertyRect.x += propertyRect.width + paddingWidth;
            EditorGUI.LabelField(propertyRect, defaultValueContent/*, headerStyle*/);
        }

        private void DrawContextKeyItemsListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            var elementStyle = new GUIStyle(GUI.skin.box)
            {
                normal = { background = EditorGUIUtility.whiteTexture },
                border = new RectOffset(4, 4, 4, 4)
            };

            var propertyRect = new Rect(rect.x, rect.y + EditorGUIUtility.standardVerticalSpacing, (rect.width / 2f) - paddingWidth, EditorGUIUtility.singleLineHeight);
            var elementProperty = contextKeyItemsList.serializedProperty.GetArrayElementAtIndex(index);
            var keyProperty = elementProperty.FindPropertyRelative(nameof(TipContextKeyItem.Key));
            var defaultValueProperty = elementProperty.FindPropertyRelative(nameof(TipContextKeyItem.Value));

            EditorGUI.PropertyField(propertyRect, keyProperty, GUIContent.none);

            propertyRect.x += propertyRect.width + paddingWidth;
            EditorGUI.PropertyField(propertyRect, defaultValueProperty, GUIContent.none);
        }
    }
}
