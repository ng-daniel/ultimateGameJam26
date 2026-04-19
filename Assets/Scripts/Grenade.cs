using UnityEngine;

namespace Assets.Scripts.Player
{
    public class Grenade : MonoBehaviour
    {
        Rigidbody rb;
        [SerializeField] float explosionRadius = 5f;
        [SerializeField] float explosionForce = 700f;
        [SerializeField] float cleanAmount = 0.5f;
        [SerializeField] float fuseTime = 0.5f;
        [SerializeField] GameObject explosionEffectPrefab;

        public void Awake()
        {
            rb = GetComponent<Rigidbody>();           
        }

        public void Update()
        {
            fuseTime -= Time.deltaTime;
            if (fuseTime <= 0f)
            {
                Explode();
            }
        }

        public void Throw(Quaternion rotation, float throwStrength)
        {
            Vector3 throwDirection = rotation * Vector3.forward;
            rb.AddForce(throwDirection * throwStrength, ForceMode.Impulse);
        }

        public void Explode()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
            foreach (Collider nearbyObject in colliders)
            {
                Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                }

                DirtySurfaceBehavior dirtySurface = nearbyObject.GetComponent<DirtySurfaceBehavior>();
                if (dirtySurface != null && dirtySurface.IsDirty())
                {
                    dirtySurface.CleanDirtSomeAmount(cleanAmount); // Adjust the amount as needed
                }
            }
            // Optionally, you could add visual and sound effects here for the explosion.
            if (explosionEffectPrefab != null)
            {
                Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            }
            Destroy(gameObject); // Destroy the grenade after exploding
        }
    }
}