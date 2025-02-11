using UnityEngine;
using System;

namespace U.StringDropdown
{
    [System.Serializable]
    public class StringDropdown
    {
        [SerializeField] private string[] options; // Массив строк для выпадающего списка
        [SerializeField] private int selectedIndex = 0; // Текущий выбранный индекс

        // Событие, которое будет вызываться при изменении выбранного индекса
        public event Action<string> OnValueChanged;

        // Конструктор для инициализации массива строк
        public StringDropdown(params string[] options)
        {
            this.options = options;
        }

        // Свойство для получения текущего выбранного значения
        public string Value
        {
            get
            {
                if (options == null || options.Length == 0)
                    return string.Empty;
                return options[selectedIndex];
            }
        }

        // Свойство для получения массива строк (опционально)
        public string[] Options => options;

        // Свойство для получения и установки текущего индекса
        public int SelectedIndex
        {
            get => selectedIndex;
            set
            {
                // Если индекс изменился, вызываем событие
                if (selectedIndex != value)
                {
                    selectedIndex = Mathf.Clamp(value, 0, options.Length - 1);
                    OnValueChanged?.Invoke(Value); // Вызываем событие с текущим значением
                }
            }
        }
    }
}
