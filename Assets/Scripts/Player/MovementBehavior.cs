using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Player
{
    /// <summary>
    /// Handles movement input and applies it to a Rigidbody.
    /// </summary>
    public class MovementBehavior : MonoBehaviour
    {
        Vector2 lastInput;
        Rigidbody rb;
        [SerializeField] MovementStats moveStats;
        [SerializeField] MovementStats debugMoveStats;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }
        public void MoveRegular(Vector2 rawInput, bool isGrounded)
        {
            Move(rawInput, moveStats, isGrounded);
        }
        public void MoveDebug(Vector2 rawInput)
        {
            Move(rawInput, debugMoveStats);
        }

        /// <summary>
        /// Moves the player based on input values with acceleration and speed limits.
        /// </summary>
        /// <param name="rawInput">The input vector representing the desired movement direction.</param>
        /// <param name="moveStats">The movement stats to use for this movement.</param>
        /// <param name="isGrounded">Indicates whether the player is grounded.</param>
        void Move(Vector2 rawInput, MovementStats moveStats, bool isGrounded = true)
        {
            const float inputThreshold = 0.01f;
            float dt = Time.fixedDeltaTime;

            bool hasInput = Mathf.Abs(rawInput.x) > inputThreshold || Mathf.Abs(rawInput.y) > inputThreshold;
            Vector2 inputDir = hasInput ? rawInput.normalized : Vector2.zero;

            // Convert input direction to world space relative to the player's horizontal facing
            Vector3 forward = transform.forward;
            forward.y = 0f;
            forward.Normalize();
            Vector3 right = transform.right;
            right.y = 0f;
            right.Normalize();
            Vector3 worldDir = right * inputDir.x + forward * inputDir.y;
            Vector2 worldInputDir = hasInput ? new Vector2(worldDir.x, worldDir.z) : Vector2.zero;
            bool isDirectlyBackwardInput = hasInput && inputDir.y < -0.95f && Mathf.Abs(inputDir.x) < 0.1f;

            Vector2 currentHorizontalVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.z);
            Vector2 targetHorizontalVelocity = worldInputDir * moveStats.WalkSpeed;

            float acceleration = moveStats.WalkSpeedAccel;
            float deceleration = moveStats.WalkSpeedAccel * 1.35f;
            float directionChangeBoost = 2.0f;

            if (!isGrounded)
            {
                acceleration *= moveStats.AirborneAccelMultiplier;

                if (hasInput)
                {
                    currentHorizontalVelocity = isDirectlyBackwardInput
                        ? Vector2.MoveTowards(
                            currentHorizontalVelocity,
                            targetHorizontalVelocity,
                            acceleration * dt
                        )
                        : ApplyAirSteering(
                            currentHorizontalVelocity,
                            worldInputDir,
                            moveStats.WalkSpeed,
                            acceleration,
                            dt,
                            inputThreshold
                        );
                }
            }
            else if (!hasInput)
            {
                // Decelerate to a stop when there is no input on the ground.
                currentHorizontalVelocity = Vector2.MoveTowards(
                    currentHorizontalVelocity,
                    Vector2.zero,
                    deceleration * dt
                );
            }
            else
            {
                // Apply a boost to acceleration when changing direction sharply.
                float turnMultiplier = 1f;
                if (Mathf.Abs(currentHorizontalVelocity.x) > inputThreshold ||
                    Mathf.Abs(currentHorizontalVelocity.y) > inputThreshold)
                {
                    Vector2 currentDir = currentHorizontalVelocity.normalized;
                    float alignment = Vector2.Dot(currentDir, inputDir);
                    if (alignment < 0f)
                    {
                        turnMultiplier = directionChangeBoost;
                    }
                }

                currentHorizontalVelocity = Vector2.MoveTowards(
                    currentHorizontalVelocity,
                    targetHorizontalVelocity,
                    acceleration * turnMultiplier * dt
                );
            }

            rb.linearVelocity = new Vector3(currentHorizontalVelocity.x, rb.linearVelocity.y, currentHorizontalVelocity.y);
        }

        /// <summary>
        /// Applies steering to the player's velocity while airborne, allowing for smoother direction changes.
        /// Does this by calculating a desired direction based on input and gradually rotating the current velocity 
        /// towards it, while also allowing for acceleration up to the target speed.
        /// </summary>
        Vector2 ApplyAirSteering(
            Vector2 currentHorizontalVelocity,
            Vector2 worldInputDir,
            float targetSpeed,
            float acceleration,
            float dt,
            float inputThreshold)
        {
            if (currentHorizontalVelocity.sqrMagnitude <= inputThreshold * inputThreshold)
            {
                return Vector2.MoveTowards(
                    currentHorizontalVelocity,
                    worldInputDir * targetSpeed,
                    acceleration * dt
                );
            }

            float currentSpeed = currentHorizontalVelocity.magnitude;
            Vector3 currentDirection = new Vector3(currentHorizontalVelocity.x, 0f, currentHorizontalVelocity.y).normalized;
            Vector3 desiredDirection = new Vector3(worldInputDir.x, 0f, worldInputDir.y).normalized;

            float maxTurnRadians = acceleration * dt / Mathf.Max(currentSpeed, 0.01f);
            Vector3 steeredDirection = Vector3.RotateTowards(currentDirection, desiredDirection, maxTurnRadians, 0f);
            Vector2 steeredVelocity = new Vector2(steeredDirection.x, steeredDirection.z) * currentSpeed;

            if (currentSpeed < targetSpeed)
            {
                steeredVelocity = Vector2.MoveTowards(
                    steeredVelocity,
                    worldInputDir * targetSpeed,
                    acceleration * dt
                );
            }

            return steeredVelocity;
        }
    }
}