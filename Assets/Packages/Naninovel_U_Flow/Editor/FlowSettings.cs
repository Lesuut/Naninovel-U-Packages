using Naninovel.U.SideTip;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Naninovel.U.Flow.Commands
{
    public class FlowSettings : ConfigurationSettings<FlowConfiguration>
    {
        protected override Dictionary<string, Action<SerializedProperty>> OverrideConfigurationDrawers()
        {
            var drawers = base.OverrideConfigurationDrawers();
            drawers[nameof(FlowConfiguration.API)] = DrawContextKeyItemsEditor;
            return drawers;
        }

        private void DrawContextKeyItemsEditor(SerializedProperty property)
        {
            var label = EditorGUI.BeginProperty(Rect.zero, null, property);
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);

            var text =
                SyntaxHighlighter.ColorizeSyntax(
                 "\n" +
                 "@flow //Запустить поток сцен по порядку\r\n" +
                 "• Порядок хранится в Flow Assets Way\r\n" +
                 "• После прохождения пути при следующем запуске " +
                 "@flow начнётся следующий по очереди переход\r\n\r\n" +
                 "@flow SomeFlowAsset //Аргумент запускает указанный путь по имени отдельно от основного потока\r\n" +
                 "@flow SomeFlowAsset back:SomeBack //Параметр back указывает задний фон, с которого начнётся путь сценария\r\n\r\n" +
                 "@flowSetCustomEnd SomeBack //Аргумент - Указывает задний фон, на котором завершиться путь\r\n\r\n" +
                 "@flowPause //Приостановить выполнение потока. Скрывает кнопки передвижения\r\n" +
                 "@flowContinue //Продолжить выполнение потока. Показывает кнопки передвижения\r\n" +
                 "@flowStop //Завершает выполение потока в любой момент.\r\n\r\n" +
                 "// Low-use\r\n" +
                 "@flowButtonsHide true/false //Скрыть или показать кнопки потока\r\n• true - Скрывает кнопки\r\n• false - Показывает кнопки\r\n\r\n" +
                 "@setFlowWayIndex 1 //Установить индекс пути потока проигрывания\r\n• Аргумент - Числовой индекс пути для Flow Assets Way");



            var richTextStyle = new GUIStyle(EditorStyles.textArea) { richText = true, wordWrap = true };
            GUILayout.TextArea(text, richTextStyle);

            EditorGUILayout.HelpBox(
                "После импорта пакета Flow проверьте, что все файлы из папки Editor Default Resources были перенесены в проект!", MessageType.Info);

            EditorGUI.EndProperty();
        }
    }
}
