using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class Planet : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Camera mainCam;

        [HideInInspector] public UnityEvent<PointerEventData.InputButton, Vector3> MouseClickEvent;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (Vector2.SqrMagnitude(eventData.position - eventData.pressPosition) > Constants.MOUSE_CLICK_SQ_OFFSET_LIMIT)
            {
                return;
            }
            MouseClickEvent?.Invoke(eventData.button, eventData.pointerCurrentRaycast.worldPosition);
        }
    }
}
