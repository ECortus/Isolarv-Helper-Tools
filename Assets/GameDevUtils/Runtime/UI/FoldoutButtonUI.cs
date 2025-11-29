using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameDevUtils.Runtime.UI
{
    public class FoldoutButtonUI : MonoBehaviour
    {
        [SerializeField] private bool startOpen = false;
        [SerializeField] private GameObject content;
        
        [Space(5)]
        [SerializeField] private Button foldoutButton;
        [SerializeField] private TextMeshProUGUI foldoutButtonText;
        
        bool isOpen = false;
        
        private void Start()
        {
            SetupFoldoutButton();

            if (startOpen)
                Open();
            else
                Close();
        }

        void SetupFoldoutButton()
        {
            foldoutButton.onClick.AddListener(Foldout);
        }

        void Foldout()
        {
            if (!isOpen)
                Open();
            else
                Close();
        }

        void Open()
        {
            content.SetActive(true);
            isOpen = true;
            
            foldoutButtonText.text = "ON";
        }

        void Close()
        {
            content.SetActive(false);
            isOpen = false;
            
            foldoutButtonText.text = "OFF";
        }
    }
}