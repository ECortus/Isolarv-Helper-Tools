using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameDevUtils.Runtime.UI
{
    public class IsolarvDebugUI : MonoBehaviour
    {
        [SerializeField] private bool startOpen = false;
        
        [Space(5)]
        [SerializeField] private CanvasGroup root;
        [SerializeField] private Button foldoutRootButton;
        
        bool isRootOpened = false;
        
        private void Start()
        {
            SetupFoldoutButtons();

            if (startOpen)
                Open();
            else
                Close();
            
            GameObject.DontDestroyOnLoad(this.gameObject);
        }

        void SetupFoldoutButtons()
        {
            foldoutRootButton.onClick.AddListener(FoldoutRoot);
        }

        void FoldoutRoot()
        {
            if (!isRootOpened)
                Open();
            else
                Close();
        }

        void Open()
        {
            root.alpha = 1f;
            isRootOpened = true;
        }

        void Close()
        {
            root.alpha = 0f;
            isRootOpened = false;
        }
    }
}