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

        public float Speed { get; private set; }

        public event UnityAction FinishMovement;

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
            targetPivotRotation = this.setting.pivot.localRotation;

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
            targetPivotRotation = setting.pivot.localRotation * Quaternion.FromToRotation(Vector3.up, pivotNewDirection);
            if (stopBeforeDestination)
            {
                targetPivotRotation = Quaternion.RotateTowards(targetPivotRotation, setting.pivot.localRotation, -plantingDestinationOffsetAngle);
            }
            totalPivotAngleDiff = GetCurrentPivotAngleDiff();
        }

        public void UpdateMove()
        {
            if (setting.surfacePoint.localRotation != targetSurfaceRotation)
            {
                setting.surfacePoint.localRotation = Quaternion.RotateTowards(setting.surfacePoint.localRotation, targetSurfaceRotation, setting.rotationSpeed * Time.deltaTime);
            }
            else if (setting.pivot.localRotation != targetPivotRotation)
            {
                float progress = GetCurrentPivotAngleDiff() / totalPivotAngleDiff;
                float pivotAngularSpeed = CalculatePivotSpeed(progress);
                setting.pivot.localRotation = Quaternion.RotateTowards(setting.pivot.localRotation, targetPivotRotation, pivotAngularSpeed * Time.deltaTime);
                Speed = setting.moveSpeedCurve.Evaluate(1f - progress);
            }
            else
            {
                Speed = 0f;
                FinishMovement?.Invoke();
            }
        }

        private float CalculatePivotSpeed(float normalisedAngleDiff)
        {
            return setting.moveSpeedCurve.Evaluate(1f - normalisedAngleDiff) * setting.moveSpeed;
        }

        private float GetCurrentPivotAngleDiff()
        {
            return Vector3.Angle(setting.pivot.localRotation * Vector3.up, targetPivotRotation * Vector3.up);
        }
    }
}
