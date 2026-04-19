using UnityEngine;

namespace Assets.Scripts.Player
{
    public class GrenadeThrower : MonoBehaviour
    {
        Rigidbody rb;
        [SerializeField] GameObject grenadeThrowPoint;
        [SerializeField] GameObject grenadePrefab;
        [SerializeField] float throwStrength = 15f;

        public void Awake()
        {
            rb = GetComponent<Rigidbody>();           
        }

        public void ThrowGrenade(Quaternion rotation)
        {
            if (grenadePrefab == null)
            {
                Debug.LogError("Grenade prefab not assigned in the inspector.");
                return;
            }

            GameObject grenadeObject = Instantiate(grenadePrefab, grenadeThrowPoint.transform.position, Quaternion.identity);
            Grenade grenade = grenadeObject.GetComponent<Grenade>();
            if (grenade != null)
            {
                grenade.Throw(rotation, throwStrength);
            }
            else
            {
                Debug.LogError("Grenade prefab does not have a Grenade component.");
            }
        }
    }
}