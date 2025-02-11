#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace U.StringDropdown
{
    [CustomPropertyDrawer(typeof(StringDropdown))]
    public class StringDropdownDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Получаем свойства массива строк и текущего индекса
            SerializedProperty optionsProperty = property.FindPropertyRelative("options");
            SerializedProperty selectedIndexProperty = property.FindPropertyRelative("selectedIndex");

            // Проверяем, что массив строк существует
            if (optionsProperty == null || !optionsProperty.isArray)
            {
                EditorGUI.LabelField(position, "Invalid options array");
                return;
            }

            // Создаем массив строк для выпадающего списка
            string[] options = new string[optionsProperty.arraySize];
            for (int i = 0; i < optionsProperty.arraySize; i++)
            {
                options[i] = optionsProperty.GetArrayElementAtIndex(i).stringValue;
            }

            // Отображаем выпадающий список
            selectedIndexProperty.intValue = EditorGUI.Popup(position, label.text, selectedIndexProperty.intValue, options);
        }
    }
#endif
}