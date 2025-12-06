using GameSaveKit.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameSaveKit.Tests
{
    public sealed class UTestButtons : MonoBehaviour
    {
        [SerializeField] private TestSaving testSaving;
        
        [SerializeField] private Text text;
        [SerializeField] private Button saveButton;
        [SerializeField] private Button loadButton;
        
        void Start()
        {
            saveButton.onClick.AddListener(Save);
            loadButton.onClick.AddListener(Load);
        }
        
        void Update()
        {
            text.text = $"Value: {testSaving.testValue}";
        }
        
        void OnDestroy()
        {
            saveButton.onClick.RemoveListener(Save);
            loadButton.onClick.RemoveListener(Load);
        }
        
        void Save()
        {
            SaveablePrefs.Save<TestPrefs>();
        }
        
        void Load()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}