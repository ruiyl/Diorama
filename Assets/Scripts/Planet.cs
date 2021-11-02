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
            if (Vector2.SqrMagnitude(eventData.position - eventData.pressPosition) > EventSystem.current.pixelDragThreshold) // Prevent clicking event from going off when the player finish dragging mouse
            {
                return;
            }
            MouseClickEvent?.Invoke(eventData.button, eventData.pointerCurrentRaycast.worldPosition); // Notify the Player
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            isPointerOver = true;
        }

        public void OnPointerExit(PointerEventData eventData) // Hide cursor effect when mouse is not on the planet
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
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition); // Calculate where the mouse hit the planet
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

        private void PlaceCursorFx(Vector3 position) // place and orientate the cursor effect
        {
            cursorFx.SetPositionAndRotation(position, Quaternion.FromToRotation(Vector3.up, (position - transform.position).normalized));
        }

        public void Rotate(float xDegree, float yDegree) // Rotate the planet
        {
            transform.RotateAround(transform.position, transform.position + Vector3.up, yDegree);
            transform.RotateAround(transform.position, transform.position + Vector3.right, xDegree);
        }
    }
}
