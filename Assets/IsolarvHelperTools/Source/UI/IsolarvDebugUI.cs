using System;
using UnityEngine;
using UnityEngine.UI;

namespace IsolarvHelperTools.Source.UI
{
    public class IsolarvDebugUI : MonoBehaviour
    {
        [SerializeField] private bool startOpen = false;
        [SerializeField] private CanvasGroup root;
        [SerializeField] private Button foldoutButton;
        
        bool isOpened = false;
        
        private void Start()
        {
            SetupFoldoutButton();

            if (startOpen)
                Open();
            else
                Close();
            
            GameObject.DontDestroyOnLoad(this.gameObject);
        }

        void SetupFoldoutButton()
        {
            foldoutButton.onClick.AddListener(Foldout);
        }

        void Foldout()
        {
            if (!isOpened)
            {
                Open();
            }
            else
            {
                Close();
            }
        }

        void Open()
        {
            root.alpha = 1f;
            isOpened = true;
        }

        void Close()
        {
            root.alpha = 0f;
            isOpened = false;
        }
    }
}