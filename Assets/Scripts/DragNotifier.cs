using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class DragNotifier : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler // Class that notifies when player is dragging on the attached object
    {
        public event UnityAction<PointerEventData> BeginDragEvent;
        public event UnityAction<PointerEventData> DragEvent;
        public event UnityAction<PointerEventData> EndDragEvent;

        private bool isPointerDown;
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            BeginDragEvent?.Invoke(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            DragEvent?.Invoke(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            EndDragEvent?.Invoke(eventData);
        }
    }
}
