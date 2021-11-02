using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class CameraRotation : MonoBehaviour
    {
        [SerializeField] private DragNotifier[] dragNotifiers;
        [SerializeField] private Camera mainCam;
        [SerializeField] private Planet planet;
        [SerializeField] private float minCamDistance;
        [SerializeField] private float maxCamDistance;
        [SerializeField] private float camZoomSpeed;
        [SerializeField] private int camZoomLevel;

        private bool mouseState;
        private bool isDragging;
        private Vector2 lastDragPos;
        private Vector2 dampForce;

        private float currentCamDistance;
        private float targetCamDistance;

        private const float SCREEN_TO_ROTATION_MULTIPLIER = 100f;

        private void Awake() // Set default zoom distance and bind drag event with handlers
        {
            targetCamDistance = Mathf.Abs(mainCam.transform.localPosition.z);
            foreach (DragNotifier notifer in dragNotifiers)
            {
                notifer.BeginDragEvent += HandleBeginDrag;
                notifer.DragEvent += HandleDrag;
                notifer.EndDragEvent += HandleEndDrag;
            }
        }

        private void Update()
        {
            UpdateRotation();
            UpdateZoom();
        }

        private void HandleBeginDrag(PointerEventData eventData) // Hide mouse and set first frame's pointer position when drag begins
        {
            isDragging = true;
            lastDragPos = ConvertSceenPositionToViewPortPosition(eventData.position);
            Cursor.visible = false;
        }

        private void HandleDrag(PointerEventData eventData) // Rotate with pointer position offset from the last frame
        {
            Vector2 currentMousePos = ConvertSceenPositionToViewPortPosition(eventData.position);
            RotateCamera(currentMousePos - lastDragPos);
            lastDragPos = currentMousePos;
        }

        private void HandleEndDrag(PointerEventData eventData) // Reset state and show mouse
        {
            isDragging = false;
            Cursor.visible = true;
        }

        private void UpdateRotation()
        {
            if (!isDragging && dampForce.magnitude > 0.1f) // If rotation damping force is high enough, continue camera rotation for smoothness
            {
                RotateCamera(dampForce * Time.deltaTime, true);
                dampForce *= 0.95f; // decay the force to make it gradually stop
            }
        }

        private void UpdateZoom() // Check if scrollwheel is scrolled to set camera target distance. Then, gradually move camera to the target distance for smoothness
        {
            float scrollValue = -Input.mouseScrollDelta.y;
            Vector3 mainCamLocalPos = mainCam.transform.localPosition;
            targetCamDistance = Mathf.Clamp(targetCamDistance + scrollValue * ((maxCamDistance - minCamDistance) / camZoomLevel), minCamDistance, maxCamDistance);
            Vector3 mainCamTargetPos = new Vector3(mainCamLocalPos.x, mainCamLocalPos.y, -targetCamDistance);
            mainCam.transform.localPosition = Vector3.MoveTowards(mainCamLocalPos, mainCamTargetPos, camZoomSpeed * Time.deltaTime);
        }

        private void RotateCamera(Vector2 dragOffset, bool isDamping = false) // Calculate rotation degree from viewport offset value, then rotate the planet
        {
            Vector2 rotation = new Vector2(dragOffset.y * SCREEN_TO_ROTATION_MULTIPLIER, dragOffset.x * -SCREEN_TO_ROTATION_MULTIPLIER * Screen.width / Screen.height);
            planet.Rotate(rotation.x, rotation.y);
            if (!isDamping && dragOffset.magnitude > 0f) // If hasn't already damping, set damp force to be the last positive rotation power
            {
                dampForce = dragOffset * SCREEN_TO_ROTATION_MULTIPLIER;
            }
        }

        private Vector2 ConvertSceenPositionToViewPortPosition(Vector2 position) // Convert pixel position to viewport position
        {
            return new Vector2(position.x / Screen.width, position.y / Screen.height);
        }
    }
}