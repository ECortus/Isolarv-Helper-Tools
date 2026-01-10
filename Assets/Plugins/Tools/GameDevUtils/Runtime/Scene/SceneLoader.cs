using System;
using GameDevUtils.Runtime.Settings;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameDevUtils.Runtime.Scene
{
    [System.Serializable]
    public class SceneLoader
    {
        public static int SceneIndexInQueue { get; private set; } = -1;
        
        public delegate AsyncOperation LoadSceneOperation();
        
        public static void LoadScene(int index)
        {
            LoadSceneOperation operation = () => GetLoadOperation(index);
            SceneIndexInQueue = index;
            
            LoadScene_Internal(operation);
        }
        
        public static void LoadScene(string name)
        {
            LoadSceneOperation operation = () => GetLoadOperation(name);
            SceneIndexInQueue = SceneManager.GetSceneByName(name).buildIndex;
            
            LoadScene_Internal(operation);
        }
        
        static AsyncOperation GetLoadOperation(int index)
        {
            return SceneManager.LoadSceneAsync(index, DevUtilsParametersSettings.LoadSceneMode);
        }
        
        static AsyncOperation GetLoadOperation(string name)
        {
            return SceneManager.LoadSceneAsync(name, DevUtilsParametersSettings.LoadSceneMode);
        }

        static void LoadScene_Internal(LoadSceneOperation operation)
        {
            bool sceneIsQueued = false;
            
            if (DevUtilsParametersSettings.EnableLoadingScreen)
            {
                if (!LoadingScreen.HasOrFindInstance)
                {
                    DebugHelper.LogWarning("LoadingScreen hasn't been loaded yet.");
                }
                else
                {
                    sceneIsQueued = true;
                    
                    var screen = LoadingScreen.GetInstance;
                    screen.LoadScene(operation);
                }
            }

            if (!sceneIsQueued)
            {
                operation();
            }
            
            ResetSceneInQueue();
        }

        static void ResetSceneInQueue()
        {
            SceneIndexInQueue = -1;
        }

        public static void QuitGame()
        {
            Application.Quit();
        }
    }
}