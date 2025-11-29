using System;
using UnityEngine;

namespace GameDevUtils.Runtime.UI
{
    public abstract class BaseFieldSetup : MonoBehaviour
    {
        protected DebugFieldManager FieldManager;

        private void Awake()
        {
            FieldManager = GetComponent<DebugFieldManager>();
            Setup();
        }
        
        protected abstract void Setup();
    }
}