using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace GameDevUtils.Runtime.UI.Abstract
{
    public abstract class UDynamicTextField : UDynamicField
    {
        [Space(5)]
        [SerializeField] private TMP_Text text;

        protected override void UpdateField()
        {
            UpdateText();
        }
        
        protected abstract string GetText();
        
        void UpdateText()
        {
            text.text = GetText();
        }
    }
}