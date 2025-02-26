using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Naninovel.U.CG
{
    public class UCGalleryPaginator : MonoBehaviour
    {
        [Range(1, 16)]
        [SerializeField] private int itemsPerPage = 6; // Количество объектов на странице
        [Space]
        [SerializeField] private Button nextPageButton;
        [SerializeField] private Button previousPageButton;
        [SerializeField] private UnityEvent<string> onPageChanged; // Вызывает событие с номером страницы

        private List<GameObject> activeObjects = new List<GameObject>(); // Только активные объекты
        private int currentPage = 0;
        private int totalPages;

        private void Awake()
        {
            foreach (var obj in GetChildObjects(transform))
            {
                if (obj.activeSelf)
                {
                    activeObjects.Add(obj);
                }
            }

            totalPages = Mathf.CeilToInt((float)activeObjects.Count / itemsPerPage);
            nextPageButton.onClick.AddListener(NextPage);
            previousPageButton.onClick.AddListener(PreviousPage);
            UpdatePage();
        }

        private void UpdatePage()
        {
            for (int i = 0; i < activeObjects.Count; i++)
            {
                activeObjects[i].SetActive(i >= currentPage * itemsPerPage && i < (currentPage + 1) * itemsPerPage);
            }

            onPageChanged?.Invoke($"{currentPage + 1}");

            previousPageButton.interactable = currentPage > 0;
            nextPageButton.interactable = currentPage < totalPages - 1;
        }

        public void NextPage()
        {
            if (currentPage < totalPages - 1)
            {
                currentPage++;
                UpdatePage();
            }
        }

        public void PreviousPage()
        {
            if (currentPage > 0)
            {
                currentPage--;
                UpdatePage();
            }
        }

        private GameObject[] GetChildObjects(Transform parent)
        {
            int childCount = parent.childCount;
            GameObject[] children = new GameObject[childCount];

            for (int i = 0; i < childCount; i++)
            {
                children[i] = parent.GetChild(i).gameObject;
            }

            return children;
        }

    }
}