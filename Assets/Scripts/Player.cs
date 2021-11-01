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

        private PivotMovement movement;        
        private PlayerState currentState;
        private PlayerInput currentInput;
        private Vector3 pressPosition;

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
                    return;
                case PlayerState.Planting:
                    //UpdatePlant();
                    return;
            }
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
            movement.MoveTo(moveToPosition, currentInput == PlayerInput.Plant);
            currentState = PlayerState.Moving;
            pressPosition = moveToPosition;
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

        private void PlantTreeAt(Vector3 position)
        {
            currentState = PlayerState.Planting;
        }
    }
}
