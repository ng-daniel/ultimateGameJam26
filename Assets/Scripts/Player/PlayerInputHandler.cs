using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Player
{
    /// <summary>
    /// Handles player movement input and applies it to the Rigidbody.
    /// </summary>
    public class PlayerInputHandler : MonoBehaviour
    {
        
        InputAction lookAction;
        InputAction moveAction;
        InputAction jumpAction;
        InputAction interactAction;
        InputAction crouchAction;
        InputAction lmbAction;
        InputAction rmbAction;
        InputAction select1;
        InputAction select2;


        PlayerController playerController;
        RoomLoader roomLoader;

        private void Awake()
        {
            lookAction = InputSystem.actions.FindAction("Look");
            moveAction = InputSystem.actions.FindAction("Move");
            jumpAction = InputSystem.actions.FindAction("Jump");
            interactAction = InputSystem.actions.FindAction("Interact");
            crouchAction = InputSystem.actions.FindAction("Crouch");
            lmbAction = InputSystem.actions.FindAction("Attack");
            rmbAction = InputSystem.actions.FindAction("SecondaryAttack");
            select1 = InputSystem.actions.FindAction("Select1");
            select2 = InputSystem.actions.FindAction("Select2");
            
            playerController = FindFirstObjectByType<PlayerController>();
            if (playerController == null)
            {
                Debug.LogError("PlayerController not found at start of scene.");
            }
            roomLoader = FindFirstObjectByType<RoomLoader>();
            if (roomLoader == null)
            {
                Debug.LogError("RoomLoader not found at start of scene.");
            }
        }

        void OnEnable()
        {
            jumpAction.started += OnJumpStart;
            interactAction.started += OnInteractStart;
            lmbAction.canceled += OnLeftClickRelease;
            rmbAction.started += OnRightClick;
            select1.started += OnSelect1;
            select2.started += OnSelect2;
        }
        void OnDisable()
        {
            jumpAction.started -= OnJumpStart;
            interactAction.started -= OnInteractStart;
            lmbAction.canceled -= OnLeftClickRelease;
            rmbAction.started -= OnRightClick;
            select1.started -= OnSelect1;
            select2.started -= OnSelect2;
        }

        void Update()
        {
            if (playerController == null)
            {
                playerController = FindFirstObjectByType<PlayerController>();
                return;
            }
            
            Vector2 lookValue = lookAction.ReadValue<Vector2>();
            if (playerController != null)
            {
                playerController.Look(lookValue);
            }
        }
        void FixedUpdate()
        {
            if (playerController == null)
            {
                return;
            }
            
            Vector2 moveValue = moveAction.ReadValue<Vector2>();
            if (playerController != null)
            {
                playerController.Move(moveValue);
            }

            bool leftMouseHeld = lmbAction.ReadValue<float>() > 0.1f;
            if (playerController != null)
            {
                 playerController.GetVacuumBehavior().SuckVisual(leftMouseHeld);
            }
            if (leftMouseHeld && playerController != null)
            {
                playerController.TryVacuumPrimary();
            }

            bool jumping = jumpAction.ReadValue<float>() > 0.1f;
            bool crouching = crouchAction.ReadValue<float>() > 0.1f;
            if (jumping && !crouching)
            {
                playerController.TryFlyIfDebug(true);
            }
            else if (crouching && !jumping)
            {
                playerController.TryFlyIfDebug(false);
            }
            else
            {
                playerController.TryStopFlyIfDebug();
            }
        }
        void OnJumpStart(InputAction.CallbackContext context)
        {
            if (playerController != null && !playerController.GetJumpBehavior().TryStartJump())
            {
                Debug.Log("Jump failed: player is not grounded.");
            }
        }

        void OnFlyUp(InputAction.CallbackContext context)
        {
            if (playerController != null)
            {
                playerController.TryFlyIfDebug(true);
            }
        }

        void OnFlyDown(InputAction.CallbackContext context)
        {
            if (playerController != null)
            {
                playerController.TryFlyIfDebug(false);
            }
        }

        void OnInteractStart(InputAction.CallbackContext context)
        {
            // no more debug toggle!!!!!
            
            // if (playerController != null)
            // {
            //     playerController.ToggleDebugMode();
            // }
        }

        void OnLeftClickRelease(InputAction.CallbackContext context)
        {
            Debug.Log("LEFT: Left click released");
            if (playerController != null)
            {
                playerController.TryReleaseVacuumPrimary();
            }
        }

        void OnRightClick(InputAction.CallbackContext context)        
        {
            if (playerController != null)
            {
                playerController.TryGrenadeSecondary();
            }
        }

        void OnSelect1(InputAction.CallbackContext context)
        {
            if (roomLoader != null)
            {
                roomLoader.TryRestartLevel();
            }
        }

        void OnSelect2(InputAction.CallbackContext context)
        {
            if (roomLoader != null)
            {
                roomLoader.TryBackToMainMenu();
            }
        }
    }
}