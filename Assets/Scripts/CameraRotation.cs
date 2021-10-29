using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class CameraRotation : MonoBehaviour
    {
        [SerializeField] private Camera mainCam;
        [SerializeField] private Transform cameraPivot;
        [SerializeField] private float minCamDistance;
        [SerializeField] private float maxCamDistance;
        [SerializeField] private float camZoomSpeed;
        [SerializeField] private int camZoomLevel;

        private bool isDragging;
        private Vector2 lastDragPos;
        private Vector2 dampForce;

        private float currentCamDistance;
        private float targetCamDistance;

        private const float SCREEN_TO_ROTATION_MULTIPLIER = 100f;

        private void Awake()
        {
            targetCamDistance = Mathf.Abs(mainCam.transform.localPosition.z);
        }

        private void Update()
        {
            UpdateRotation();
            UpdateZoom();
        }

        private void UpdateRotation()
        {
            bool mouseState = Input.GetMouseButton(0);
            if (mouseState)
            {
                if (isDragging)
                {
                    RotateCamera(ConvertSceenPositionToViewPortPosition(Input.mousePosition) - lastDragPos);
                }
                else
                {
                    isDragging = true;
                    Cursor.visible = false;
                }
                lastDragPos = ConvertSceenPositionToViewPortPosition(Input.mousePosition);
            }
            else
            {
                Cursor.visible = true;
                isDragging = false;
                if (dampForce.magnitude > 0.1f)
                {
                    RotateCamera(dampForce * Time.deltaTime, true);
                }
            }
            dampForce *= 0.95f;
        }

        private void UpdateZoom()
        {
            float scrollValue = -Input.mouseScrollDelta.y;
            Vector3 mainCamLocalPos = mainCam.transform.localPosition;
            targetCamDistance = Mathf.Clamp(targetCamDistance + scrollValue * ((maxCamDistance - minCamDistance) / camZoomLevel), minCamDistance, maxCamDistance);
            Vector3 mainCamTargetPos = new Vector3(mainCamLocalPos.x, mainCamLocalPos.y, -targetCamDistance);
            mainCam.transform.localPosition = Vector3.MoveTowards(mainCamLocalPos, mainCamTargetPos, camZoomSpeed * Time.deltaTime);
        }

        private void RotateCamera(Vector2 dragOffset, bool isDamping = false)
        {
            Vector2 rotation = new Vector2(dragOffset.y * -SCREEN_TO_ROTATION_MULTIPLIER, dragOffset.x * SCREEN_TO_ROTATION_MULTIPLIER * Screen.width / Screen.height);
            cameraPivot.Rotate(rotation.x, rotation.y, 0f);
            if (!isDamping && dragOffset.magnitude > 0f)
            {
                dampForce = dragOffset * SCREEN_TO_ROTATION_MULTIPLIER;
            }
        }

        private Vector2 ConvertSceenPositionToViewPortPosition(Vector2 position)
        {
            return new Vector2(position.x / Screen.width, position.y / Screen.height);
        }
    }
}