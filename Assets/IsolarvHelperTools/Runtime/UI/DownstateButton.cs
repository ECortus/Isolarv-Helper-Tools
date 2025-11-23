using UnityEngine.Events;
using UnityEngine.UI;

namespace IsolarvHelperTools.Runtime.UI
{
    public class DownstateButton : Button
    {
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