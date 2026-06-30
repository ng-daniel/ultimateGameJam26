using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Player
{
    /// <summary>
    /// Handles movement input and applies it to a Rigidbody.
    /// </summary>
    public class LookBehavior : MonoBehaviour
    {
        Vector2 rotation;
        [SerializeField] float lookSensitivity = 0.25f;
        Quaternion lastLookRotation;
        Quaternion initialRotation;

        public void Start()
        {
            rotation = Vector2.zero;
            initialRotation = transform.rotation;
        }

        public Quaternion LookToQuaternion(Vector2 lookInput)
        {
            Cursor.lockState = CursorLockMode.Locked;

            rotation.x += lookInput.x * lookSensitivity;
            rotation.y += lookInput.y * lookSensitivity;
            rotation.y = Mathf.Clamp(rotation.y, -90f, 90f);
            Quaternion xRotation = Quaternion.Euler(0f, rotation.x, 0f);
            Quaternion yRotation = Quaternion.Euler(-rotation.y, 0f, 0f);
            Quaternion result = xRotation * yRotation;
            
            lastLookRotation = initialRotation * result; // combine with initial rotation to maintain the original orientation
            return lastLookRotation;
        }

        public Quaternion GetLastLookRotation()
        {
            return lastLookRotation;
        }

        public void SetInitialLook(Transform target)
        {
            if (target != null)
            {
                transform.rotation = target.rotation;
                initialRotation = target.rotation;
            }
            else
            {
                Debug.LogError("Target transform is null in SetInitialLook.");
            }
        }
    }
}