using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static Tethered.Input.PlayerInputActions;

namespace Tethered.Input
{
    [CreateAssetMenu(fileName = "PlayerOneInputReader", menuName = "Input/Player One Input Reader")]
    public class PlayerOneInputReader : ScriptableObject, IPlayerOneActions, IInputReader
    {
        public event UnityAction<Vector2, bool> Move = delegate { };
        public event UnityAction Interact = delegate { };

        public int NormMoveX { get; private set; }
        public int NormMoveY { get; private set; }

        private PlayerInputActions inputActions;

        private void OnEnable()
        {
            // Check if the input actions have been initialized
            if(inputActions == null)
            {
                // Initialize the input actions and set callbacks
                inputActions = new PlayerInputActions();
                inputActions.PlayerOne.SetCallbacks(this);
            }

            // Enable the input actions
            Enable();
        }

        /// <summary>
        /// Enable the input actions
        /// </summary>
        public void Enable() => inputActions.Enable();

        /// <summary>
        /// Disable the input actions
        /// </summary>
        public void Disable() => inputActions.Disable();

        /// <summary>
        /// Callback function to handle movement input
        /// </summary>
        public void OnMove(InputAction.CallbackContext context)
        {
            // Get the raw movement input from the control
            Vector2 rawMovementInput = context.ReadValue<Vector2>();

            // Invoke the movement event
            Move.Invoke(rawMovementInput, context.started);

            // Set variables
            NormMoveX = (int)(rawMovementInput * Vector2.right).normalized.x;
            NormMoveY = (int)(rawMovementInput * Vector2.up).normalized.y;
        }

        /// <summary>
        /// Callback function to handle interact input
        /// </summary>
        public void OnInteract(InputAction.CallbackContext context)
        {
            // If the button has been lifted, invoke the event
            if (context.canceled) Interact.Invoke();
        }
    }
}