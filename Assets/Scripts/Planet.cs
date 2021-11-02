using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class Planet : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Camera mainCam;
        [SerializeField] private Transform cursorFx;

        private bool isPointerOver;

        [HideInInspector] public UnityEvent<PointerEventData.InputButton, Vector3> MouseClickEvent;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (Vector2.SqrMagnitude(eventData.position - eventData.pressPosition) > EventSystem.current.pixelDragThreshold)
            {
                return;
            }
            MouseClickEvent?.Invoke(eventData.button, eventData.pointerCurrentRaycast.worldPosition);
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            isPointerOver = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isPointerOver = false;
            cursorFx.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (!isPointerOver)
            {
                return;
            }
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo) && hitInfo.transform == transform)
            {
                cursorFx.gameObject.SetActive(true);
                PlaceCursorFx(hitInfo.point);
            }
            else
            {
                cursorFx.gameObject.SetActive(false);
            }
        }

        private void PlaceCursorFx(Vector3 position)
        {
            cursorFx.SetPositionAndRotation(position, Quaternion.FromToRotation(transform.up, (position - transform.position).normalized));
        }
    }
}
