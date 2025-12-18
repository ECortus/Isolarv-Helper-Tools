using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameDevUtils.Runtime.UI
{
    public class IsolarvDebugUI : MonoBehaviour
    {
        static IsolarvDebugUI instance { get; set; }
        
        [SerializeField] private bool startOpen = false;
        
        [Space(5)]
        [SerializeField] private CanvasGroup root;
        [SerializeField] private Button foldoutRootButton;

        bool isRootOpened = false;

        private void Awake()
        {
            if (instance != null)
            {
                DebugHelper.LogWarning("IsolarvDebugUI already exist on scene.");
                
                ObjectHelper.Destroy(gameObject);
                return;
            }
            
            instance = this;
            ObjectHelper.DontDestroyOnLoad(this.gameObject);
        }

        void Start()
        {
            SetupFoldoutButtons();

            if (startOpen)
                Open();
            else
                Close();
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
            root.gameObject.SetActive(true);
            root.alpha = 1f;
            
            isRootOpened = true;
        }

        void Close()
        {
            root.alpha = 0f;
            root.gameObject.SetActive(false);
            
            isRootOpened = false;
        }
    }
}