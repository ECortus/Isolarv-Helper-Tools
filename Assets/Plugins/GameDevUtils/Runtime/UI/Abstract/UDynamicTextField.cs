using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace GameDevUtils.Runtime.UI.Abstract
{
    public abstract class UDynamicTextField : MonoBehaviour
    {
        protected abstract string GetText();
        
        enum EUpdateMethod
        {
            Update, FixedUpdate, InvokeRepeating
        }
        
        [SerializeField] private TMP_Text text;
        
        [Space(5)]
        [SerializeField] private EUpdateMethod updateMethod = EUpdateMethod.Update;
        
        [SerializeField, DrawIf("updateMethod", EUpdateMethod.InvokeRepeating), Range(0.05f, 2f)] 
        private float invokeDelay = 1f;

        private void Start()
        {
            if (updateMethod != EUpdateMethod.InvokeRepeating)
                return;
            
            AsyncTaskHelper.CreateTask(async () =>
            {
                while (true)
                {
                    await UniTask.Delay((int)(invokeDelay * 1000));
                    SetText();
                }
            });
        }

        void Update()
        {
            if (updateMethod != EUpdateMethod.Update)
                return;
            
            SetText();
        }
        
        void FixedUpdate()
        {
            if (updateMethod != EUpdateMethod.FixedUpdate)
                return;
            
            SetText();
        }

        void SetText()
        {
            text.text = GetText();
        }
    }
}