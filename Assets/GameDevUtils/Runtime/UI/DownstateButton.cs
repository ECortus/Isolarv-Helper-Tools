using UnityEngine.Events;
using UnityEngine.UI;

namespace GameDevUtils.Runtime.UI
{
    public class DownstateButton : Button
    {
        public UnityEvent onDown;
        public UnityEvent onHold;
        public UnityEvent onUp;

        bool isDown = false;
        
        void Update()
        {
            if (IsPressed())
            {
                if (!isDown)
                {
                    isDown = true;
                    onDown?.Invoke();
                }   
                
                WhilePressed();
            }
            else if (!IsPressed() && isDown)
            {
                isDown = false;
                onUp?.Invoke();
            }
        }
        
        void WhilePressed()
        {
            onHold?.Invoke();
        }
    }

}