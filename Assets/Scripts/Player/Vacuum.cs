using UnityEngine;

namespace Assets.Scripts.Player
{
    public class Vacuum : MonoBehaviour
    {
        Rigidbody rb;
        [SerializeField] float raycastRange = 2f;
        [SerializeField] float pullStrength = 10f;
        [SerializeField] float cleanAmount = 1.5f;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        /// <summary>
        /// Shoots a raycast in the given direction and range.
        /// Checks if the hit surface is a DirtySurfaceBehavior.
        /// If so, "latch" on to the hit position and move the velocity
        /// in the direction of the hit location.
        /// </summary>
        /// <returns>True if we hit a dirty surface, false if not.</returns>
        public bool TrySuckDirtySurface(Quaternion rotation)
        {
            if (rb == null)
            {
                rb = GetComponent<Rigidbody>();
            }

            Vector3 direction = rotation * Vector3.forward;

            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, raycastRange))
            {
                DirtySurfaceBehavior dirtySurface = hit.collider.GetComponentInParent<DirtySurfaceBehavior>();
                if (dirtySurface != null)
                {
                    Vector3 pullDirection = (hit.point - transform.position).normalized;
                    rb.linearVelocity = pullDirection * pullStrength;
                    dirtySurface.CleanDirtSomeAmount(cleanAmount * Time.deltaTime);
                    return true;
                }
            }

            return false;
        }
    }
}