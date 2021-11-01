using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class PivotMovement
    {
        private MovementSetting setting;
        private Quaternion targetSurfaceRotation;
        private Quaternion targetPivotRotation;
        private float totalPivotAngleDiff;

        private float radius;
        private float plantingDestinationOffsetAngle;

        public event Action FinishMovement;

        private const float PLANTING_DESTINATION_OFFSET_LENGTH = -1f;
        private const float ANGLE_IN_CIRCLE = 360f;

        [System.Serializable]
        public struct MovementSetting
        {
            public float rotationSpeed;
            public float moveSpeed;
            public Transform pivot;
            public Transform surfacePoint;
            public AnimationCurve moveSpeedCurve;
        }

        public PivotMovement(MovementSetting setting)
        {
            this.setting = setting;
            targetSurfaceRotation = this.setting.surfacePoint.localRotation;
            targetPivotRotation = this.setting.pivot.rotation;

            radius = (this.setting.surfacePoint.position - this.setting.pivot.position).magnitude;
            plantingDestinationOffsetAngle = PLANTING_DESTINATION_OFFSET_LENGTH * ANGLE_IN_CIRCLE / (2f * Mathf.PI * radius);
        }

        public void MoveTo(Vector3 moveToPosition, bool stopBeforeDestination)
        {
            Vector3 surfaceOffsetWorld = moveToPosition - setting.surfacePoint.position;
            Vector3 surfaceOffsetLocalDirection = setting.surfacePoint.InverseTransformDirection(surfaceOffsetWorld).normalized;
            Vector3 surfaceNewDirection = new Vector3(surfaceOffsetLocalDirection.x, 0f, surfaceOffsetLocalDirection.z);
            targetSurfaceRotation = setting.surfacePoint.localRotation * Quaternion.FromToRotation(Vector3.forward, surfaceNewDirection);

            Vector3 targetOffset = stopBeforeDestination ? surfaceOffsetWorld.normalized * PLANTING_DESTINATION_OFFSET_LENGTH : Vector3.zero;
            Vector3 pivotNewDirection = setting.pivot.InverseTransformDirection(moveToPosition - setting.pivot.position).normalized;
            targetPivotRotation = setting.pivot.rotation * Quaternion.FromToRotation(Vector3.up, pivotNewDirection);
            if (stopBeforeDestination)
            {
                targetPivotRotation = Quaternion.RotateTowards(targetPivotRotation, setting.pivot.rotation, -plantingDestinationOffsetAngle);
            }
            totalPivotAngleDiff = GetCurrentPivotAngleDiff();
        }

        public void UpdateMove()
        {
            if (setting.surfacePoint.localRotation != targetSurfaceRotation)
            {
                setting.surfacePoint.localRotation = Quaternion.RotateTowards(setting.surfacePoint.localRotation, targetSurfaceRotation, setting.rotationSpeed * Time.deltaTime);
            }
            else if (setting.pivot.rotation != targetPivotRotation)
            {
                float pivotAngularSpeed = CalculatePivotSpeed(GetCurrentPivotAngleDiff() / totalPivotAngleDiff);
                setting.pivot.rotation = Quaternion.RotateTowards(setting.pivot.rotation, targetPivotRotation, pivotAngularSpeed * Time.deltaTime);
            }
            else
            {
                FinishMovement?.Invoke();
            }
        }

        private float CalculatePivotSpeed(float normalisedAngleDiff)
        {
            return setting.moveSpeedCurve.Evaluate(1f - normalisedAngleDiff) * setting.moveSpeed;
        }

        private float GetCurrentPivotAngleDiff()
        {
            return Vector3.Angle(setting.pivot.rotation * Vector3.up, targetPivotRotation * Vector3.up);
        }
    }
}
