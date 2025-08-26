using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Supertactic.VirtualCursor
{
    [RequireComponent(typeof(VirtualCursorManagerHelper))]
    public class VirtualCursorHoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public UnityEvent onPointerEnter;
        public UnityEvent onPointerExit;

        public void OnPointerEnter(PointerEventData eventData)
        {
            onPointerEnter.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            onPointerExit.Invoke();
        }
    }
}