using System;
using System.Collections.Generic;
using GameDevUtils.Runtime.Scene;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace GameDevUtils.Runtime.UI
{
    public class UScreenCollection : MonoBehaviour
    {
        [SerializeField] List<GameObject> allScreens = new List<GameObject>();

        LoadingScreen screenSettings;
        
        GameObject currentScreen;
        Slider currentSlider;

        public void ShowScreen()
        {
            screenSettings ??= GetComponentInParent<LoadingScreen>();

            int index = GetIndex();

            for (int i = 0; i < allScreens.Count; i++)
            {
                var screen = allScreens[i];
                screen.SetActive(false);
            }
            
            currentScreen = allScreens[index];
            currentSlider = currentScreen.GetComponentInChildren<Slider>();
            
            currentScreen.SetActive(true);
        }

        int GetIndex()
        {
            var type = screenSettings.screenType;
            int index = -1;

            if (type == LoadingScreen.EScreenType.FirstInList)
            {
                index = 0;
            }
            else if (type == LoadingScreen.EScreenType.AsSceneIndex)
            {
                index = SceneLoader.SceneIndexInQueue;
                index = Mathf.Clamp(index, 0, allScreens.Count - 1);
            }
            else if (type == LoadingScreen.EScreenType.Random)
            {
                index = Random.Range(0, allScreens.Count);
            }
            else
            {
                throw new NotImplementedException();
            }

            return index;
        }

        public Slider GetSlider()
        {
            return currentSlider;
        }
    }
}