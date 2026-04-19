using UnityEngine;

public class SpriteBillboard : MonoBehaviour
{
    Camera mainCamera; 

    void Start()
    {
        mainCamera = Camera.main;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        if (mainCamera != null)
        {
            transform.LookAt(mainCamera.transform);
        }
    }
}
