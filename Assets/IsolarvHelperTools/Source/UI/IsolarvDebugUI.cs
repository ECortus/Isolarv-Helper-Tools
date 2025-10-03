using System;
using UnityEngine;

namespace IsolarvHelperTools.Source.UI
{
    public class IsolarvDebugUI : MonoBehaviour
    {
        private void Start()
        {
            GameObject.DontDestroyOnLoad(this.gameObject);
        }
    }
}