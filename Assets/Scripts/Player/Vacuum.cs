using UnityEngine;

namespace Assets.Scripts.Player
{
    public class Vacuum : MonoBehaviour
    {
        Rigidbody rb;
        [SerializeField] float raycastRange;
        [SerializeField] float pullStrength;
        [SerializeField] float releasePushStrength;
        [SerializeField] float cleanAmount;

        bool sucking = false;

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
                if (dirtySurface != null && dirtySurface.IsDirty())
                {
                    Vector3 pullDirection = (hit.point - transform.position).normalized;
                    rb.linearVelocity = pullDirection * pullStrength;
                    dirtySurface.CleanDirtSomeAmount(cleanAmount * Time.deltaTime);
                    sucking = true;
                    return true;
                }
                else if (sucking == true && !dirtySurface.IsDirty())
                {
                    // give a little bonus push if we release while still latched on to a now-clean surface
                    // to encourage the player to keep sucking until it's fully clean.
                    OnVacuumRelease();
                }
            }
            else
            {
                sucking = false;
            }

            return false;
        }

        /// <summary>
        /// On release, give a small burst of velocity in the direction the player is facing to simulate a "push" effect.
        /// </summary>
        public void OnVacuumRelease()
        {
            if (!sucking)
            {
                return;
            }

            if (rb == null)
            {
                rb = GetComponent<Rigidbody>();
            }

            sucking = false;
            Vector3 pushDirection = transform.forward;
            rb.AddForce(pushDirection * releasePushStrength, ForceMode.Impulse);
        }
    }
}