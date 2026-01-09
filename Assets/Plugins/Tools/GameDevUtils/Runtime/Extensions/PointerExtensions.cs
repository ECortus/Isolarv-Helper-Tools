using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameDevUtils.Runtime
{
    public static class PointerExtensions
    {
        /// <summary>
        /// Cast a ray to test if Input.mousePosition is over any UI object in EventSystem.current. This is a replacement
        /// for IsPointerOverGameObject() which does not work on Android in 4.6.0f3
        /// </summary>
        /// 
        /// Referencing this code for GraphicRaycaster https://gist.github.com/stramit/ead7ca1f432f3c0f181f
        /// the ray cast appears to require only eventData.position.
        /// 

#if ENABLE_LEGACY_INPUT_MANAGER
        public static bool IsPointerOverUIObject() 
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            
            return results.Count > 0;
        }
#elif ENABLE_INPUT_SYSTEM
        public static bool IsPointerOverUIObject(this Camera cam, Vector2 position) 
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = position;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            
            return results.Count > 0;
        }
#else
        public static bool IsPointerOverUIObject(this Camera cam, Canvas canvas, Vector2 screenPosition) 
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = screenPosition;

            GraphicRaycaster uiRaycaster = canvas.gameObject.GetComponent<GraphicRaycaster>();
            List<RaycastResult> results = new List<RaycastResult>();
            uiRaycaster.Raycast(eventDataCurrentPosition, results);
            
            return results.Count > 0;
        }
#endif

        public static bool ConvertScreenInputToWorldPosition(this Camera cam, Vector2 screen, string[] layers, out Vector3 hitPoint)
        {
            var ray = cam.ScreenPointToRay(screen);
            
            if (Physics.Raycast(ray, out RaycastHit hit, 99999f, LayerMask.GetMask(layers)))
            {
                hitPoint = hit.point;
                return true;
            }
            
            hitPoint = Vector3.zero;
            return false;
        }
    }
}