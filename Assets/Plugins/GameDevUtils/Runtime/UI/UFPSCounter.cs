using System;
using GameDevUtils.Runtime.UI.Abstract;
using TMPro;
using UnityEngine;

namespace GameDevUtils.Runtime.UI
{
    public class UFPSCounter : UDynamicTextField
    {
        protected override string GetText()
        {
            var fpsFloat = 1.0f / Time.deltaTime;
            var fpsRounded = Math.Round(fpsFloat, 1);
            
            string fpsString = $"{fpsRounded}";
            if (fpsRounded == Math.Round(fpsFloat, 0))
                fpsString += ".0";
            
            return $"fps: {fpsString}";
        }
    }
}