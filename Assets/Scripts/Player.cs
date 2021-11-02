using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class Player : MonoBehaviour // Keep and update Player's state
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
            movement = new PivotMovement(movementSetting); // Create movement class instance
            movement.FinishMovement += ExitState; // Look out for state exit condition

            if (TryGetComponent(out plantItem))
            {
                UnityEvent growEndEvent = plantItem.GetGrowEndEvent(); // Look out for state exit condition
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

        private void Update() // Update state handler and calculate speed for animation
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

        private void ProcessInput(PointerEventData.InputButton input, Vector3 moveToPosition) // Read input to enter corresponding state
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
                    plantItem.SetPlantingPosition(moveToPosition); // Set position early so that the PlaceHolder land on the correct place in case the Planet rotate
                    break;
            }
            movement.MoveTo(moveToPosition, currentInput == PlayerInput.Plant); // If planting, stop the player before the destination
            currentState = PlayerState.Moving;
        }

        private void ExitState() // Exit from current state to the corresponding next state
        {
            if (currentState == PlayerState.Moving)
            {
                switch (currentInput)
                {
                    case PlayerInput.Move: // Stop at the destination
                        currentInput = PlayerInput.None;
                        currentState = PlayerState.Idle;
                        break;
                    case PlayerInput.Plant: // Plant at the destination
                        EnterPlantingState();
                        break;
                }
                animator.SetFloat("Speed", movement.Speed);
            }
            else if (currentState == PlayerState.Planting) // Stop after planting
            {
                currentInput = PlayerInput.None;
                currentState = PlayerState.Idle;
                animator.SetBool("IsPlanting", false);
            }
        }

        private void EnterPlantingState() // Start planting at the destination
        {
            currentState = PlayerState.Planting;
            animator.SetBool("IsPlanting", true);
            plantItem.SetPlantingRotation(transform.rotation);
            hasBegunPlantingAnim = false;
        }

        public void OnStartPlantAnim() // Called by the AnimationEvent. Start growing item when the player finish transition from the last animation state
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
