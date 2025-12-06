using TMPro;
using UnityEngine;

namespace GameDevUtils.Runtime.UI
{
    public class UFPSCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;

        void Update()
        {
            var fps = 1.0f / Time.deltaTime;
            text.text = $"fps: {Mathf.Ceil(fps)}";
        }
    }
}