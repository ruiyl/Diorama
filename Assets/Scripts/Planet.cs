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

        [HideInInspector] public UnityEvent<Vector3> MouseClickEvent;

        public void OnPointerClick(PointerEventData eventData)
        {
            MouseClickEvent?.Invoke(eventData.pointerPressRaycast.worldPosition);
        }
    }
}

