using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class PivotMovement : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed;
        [SerializeField] private float moveSpeed;
        [SerializeField] private Transform surfacePoint;
        [SerializeField] private AnimationCurve moveSpeedCurve;

        private Quaternion targetSurfaceRotation;
        private Quaternion targetPivotRotation;
        private Vector3 moveTo;
        private float totalPivotAngleDiff;
        private bool isMoving;

        private void Awake()
        {
            targetSurfaceRotation = surfacePoint.localRotation;
            targetPivotRotation = transform.rotation;
        }

        public void SetMoveEvent(UnityEvent<Vector3> moveEvent)
        {
            moveEvent.AddListener(MoveTo);
        }

        private void MoveTo(Vector3 moveToPosition)
        {
            Vector3 surfaceOffsetDirection = surfacePoint.InverseTransformDirection(moveToPosition - surfacePoint.position).normalized;
            Vector3 surfaceNewDirection = new Vector3(surfaceOffsetDirection.x, 0f, surfaceOffsetDirection.z);
            targetSurfaceRotation = surfacePoint.localRotation * Quaternion.FromToRotation(Vector3.forward, surfaceNewDirection);
            Vector3 pivotNewDirection = transform.InverseTransformDirection(moveToPosition - transform.position).normalized;
            targetPivotRotation = transform.rotation * Quaternion.FromToRotation(Vector3.up, pivotNewDirection);
            moveTo = moveToPosition;
            totalPivotAngleDiff = GetCurrentPivotAngleDiff();
            isMoving = true;
        }

        private void Update()
        {
            if (!isMoving) return;
            if (surfacePoint.localRotation != targetSurfaceRotation)
            {
                surfacePoint.localRotation = Quaternion.RotateTowards(surfacePoint.localRotation, targetSurfaceRotation, rotationSpeed * Time.deltaTime);
            }
            else if (transform.rotation != targetPivotRotation)
            {
                float pivotAngularSpeed = CalculatePivotSpeed(GetCurrentPivotAngleDiff() / totalPivotAngleDiff);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetPivotRotation, pivotAngularSpeed * Time.deltaTime);
            }
            else
            {
                isMoving = false;
            }
        }

        private float CalculatePivotSpeed(float normalisedAngleDiff)
        {
            return moveSpeedCurve.Evaluate(1f - normalisedAngleDiff) * moveSpeed;
        }

        private float GetCurrentPivotAngleDiff()
        {
            return Vector3.Angle(surfacePoint.position - transform.position, moveTo - transform.position);
        }
    }
}
