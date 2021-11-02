using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private PivotMovement.MovementSetting movementSetting;

        private Animator animator;
        private PivotMovement movement;
        private PlayerState currentState;
        private PlayerInput currentInput;
        private PlantItem plantItem;
        private bool hasBegunPlantingAnim;

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
            movement = new PivotMovement(movementSetting);
            movement.FinishMovement += ExitState;

            if (TryGetComponent(out plantItem))
            {
                UnityEvent growEndEvent = plantItem.GetGrowEndEvent();
                growEndEvent.AddListener(ExitState);
            }
            else
            {
                Debug.LogWarning("Missing PlantItem");
            }

            if (!TryGetComponent(out animator))
            {
                Debug.LogWarning("Missing Animator");
            }
        }

        public void SetInputEvent(UnityEvent<PointerEventData.InputButton, Vector3> moveEvent)
        {
            moveEvent.AddListener(ProcessInput);
        }

        private void Update()
        {
            switch (currentState)
            {
                case PlayerState.Idle:
                    return;
                case PlayerState.Moving:
                    movement.UpdateMove();
                    animator.SetFloat("Speed", movement.Speed);
                    return;
            }
        }

        private void ProcessInput(PointerEventData.InputButton input, Vector3 moveToPosition)
        {
            if (currentState == PlayerState.Planting)
            {
                return;
            }
            switch (input)
            {
                case PointerEventData.InputButton.Left:
                    currentInput = PlayerInput.Move;
                    break;
                case PointerEventData.InputButton.Right:
                    currentInput = PlayerInput.Plant;
                    plantItem.SetPlantingPosition(moveToPosition);
                    break;
            }
            movement.MoveTo(moveToPosition, currentInput == PlayerInput.Plant);
            currentState = PlayerState.Moving;
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
                        EnterPlantingState();
                        break;
                }
                animator.SetFloat("Speed", movement.Speed);
            }
            else if (currentState == PlayerState.Planting)
            {
                currentInput = PlayerInput.None;
                currentState = PlayerState.Idle;
                animator.SetBool("IsPlanting", false);
            }
        }

        private void EnterPlantingState()
        {
            currentState = PlayerState.Planting;
            animator.SetBool("IsPlanting", true);
            plantItem.SetPlantingRotation(transform.rotation);
            hasBegunPlantingAnim = false;
        }

        public void OnStartPlantAnim()
        {
            if (hasBegunPlantingAnim)
            {
                return;
            }
            plantItem.BeginPlanting();
            hasBegunPlantingAnim = true;
        }
    }
}
