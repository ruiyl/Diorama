using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class PivotMovement : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed;
        [SerializeField] private float moveSpeed;
        [SerializeField] private Transform surfacePoint;
        [SerializeField] private AnimationCurve moveSpeedCurve;

        private PlayerState currentState;
        private PlayerInput currentInput;

        private Quaternion targetSurfaceRotation;
        private Quaternion targetPivotRotation;
        private Vector3 pressPosition;
        private float totalPivotAngleDiff;

        private float radius;
        private float plantingDestinationOffsetAngle;

        private const float PLANTING_DESTINATION_OFFSET_LENGTH = -1f;
        private const float ANGLE_IN_CIRCLE = 360f;

        private enum PlayerState
        {
            Idle,
            Moving,
            Planting,
        }

        private enum PlayerInput
        {
            None,
            Move,
            Plant,
        }

        private void Awake()
        {
            currentState = PlayerState.Idle;

            targetSurfaceRotation = surfacePoint.localRotation;
            targetPivotRotation = transform.rotation;

            radius = (surfacePoint.position - transform.position).magnitude;
            plantingDestinationOffsetAngle = PLANTING_DESTINATION_OFFSET_LENGTH * ANGLE_IN_CIRCLE / (2f * Mathf.PI * radius);
        }

        public void SetMoveEvent(UnityEvent<PointerEventData.InputButton, Vector3> moveEvent)
        {
            moveEvent.AddListener(ProcessInput);
        }

        private void ProcessInput(PointerEventData.InputButton input, Vector3 moveToPosition)
        {
            switch (input)
            {
                case PointerEventData.InputButton.Left:
                    currentInput = PlayerInput.Move;
                    break;
                case PointerEventData.InputButton.Right:
                    currentInput = PlayerInput.Plant;
                    break;
            }
            MoveTo(moveToPosition, currentInput == PlayerInput.Plant);
        }

        private void MoveTo(Vector3 moveToPosition, bool stopBeforeDestination)
        {
            Vector3 surfaceOffsetWorld = moveToPosition - surfacePoint.position;
            Vector3 surfaceOffsetLocalDirection = surfacePoint.InverseTransformDirection(surfaceOffsetWorld).normalized;
            Vector3 surfaceNewDirection = new Vector3(surfaceOffsetLocalDirection.x, 0f, surfaceOffsetLocalDirection.z);
            targetSurfaceRotation = surfacePoint.localRotation * Quaternion.FromToRotation(Vector3.forward, surfaceNewDirection);

            Vector3 targetOffset = stopBeforeDestination ? surfaceOffsetWorld.normalized * PLANTING_DESTINATION_OFFSET_LENGTH : Vector3.zero;
            Vector3 pivotNewDirection = transform.InverseTransformDirection(moveToPosition - transform.position).normalized;
            targetPivotRotation = transform.rotation * Quaternion.FromToRotation(Vector3.up, pivotNewDirection);
            if (stopBeforeDestination)
            {
                targetPivotRotation = Quaternion.RotateTowards(targetPivotRotation, transform.rotation, -plantingDestinationOffsetAngle);
            }

            pressPosition = moveToPosition;
            totalPivotAngleDiff = GetCurrentPivotAngleDiff();
            currentState = PlayerState.Moving;
        }

        private void PlantTreeAt(Vector3 position)
        {
            currentState = PlayerState.Planting;
        }

        private void Update()
        {
            switch (currentState)
            {
                case PlayerState.Idle:
                    return;
                case PlayerState.Moving:
                    UpdateMove();
                    return;
                case PlayerState.Planting:
                    UpdatePlant();
                    return;
            }
        }

        private void UpdateMove()
        {
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
                ExitState();
            }
        }

        private void UpdatePlant()
        {

        }

        private void ExitState()
        {
            if (currentState == PlayerState.Moving)
            {
                switch (currentInput)
                {
                    case PlayerInput.Move:
                        currentInput = PlayerInput.None;
                        currentState = PlayerState.Idle;
                        break;
                    case PlayerInput.Plant:
                        PlantTreeAt(pressPosition);
                        break;
                }
            }
            else if (currentState == PlayerState.Planting)
            {
                currentInput = PlayerInput.None;
                currentState = PlayerState.Idle;
            }
        }

        private float CalculatePivotSpeed(float normalisedAngleDiff)
        {
            return moveSpeedCurve.Evaluate(1f - normalisedAngleDiff) * moveSpeed;
        }

        private float GetCurrentPivotAngleDiff()
        {
            return Vector3.Angle(transform.rotation * Vector3.up, targetPivotRotation * Vector3.up);
        }
    }
}
