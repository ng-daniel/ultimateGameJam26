using Unity.VisualScripting;
using UnityEngine;

public class NoticeMeScript : MonoBehaviour
{
    Rigidbody rb;
    Camera cam;
    [SerializeField] float distanceFromCamera;
    [SerializeField] float followLerpSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
    }
    
    void LateUpdate()
    {
        // float infront of the camera's looking rotation at a fixed distance
        
        // get target position by shooting a ray from the camera's current rotation quaternion
        Vector3 targetPos = cam.transform.position + cam.transform.rotation * Vector3.forward * distanceFromCamera;

        // lerp rigidbody to target position
        rb.MovePosition(Vector3.Lerp(rb.position, targetPos, followLerpSpeed * Time.deltaTime));
    }
}
