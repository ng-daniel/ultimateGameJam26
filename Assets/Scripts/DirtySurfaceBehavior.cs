using UnityEngine;

public class DirtySurfaceBehavior : MonoBehaviour
{
    DirtyRoomManager manager;
    Renderer rend;
    Material mat;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        mat = rend.material;
        mat.SetVector("_Scale", rend.bounds.size);
    }

    public void SetManager(DirtyRoomManager manager)
    {
        // This method is currently not used, but it could be used to register this surface with the manager if needed.
        this.manager = manager;
    }

    public void SetDirtiness(float value)
    {
        mat.SetFloat("_Dirtiness", Mathf.Clamp(value, 0f, 1f));
    }

    /// <summary>
    /// Reduces the dirtiness of the surface by the cleanAmount modified by the size of the object being cleaned.
    /// </summary>
    /// <returns>The actual amount of dirt cleaned</returns>
    public float CleanProportionalToScale(float cleanAmount)
    {
        float currentDirtiness = mat.GetFloat("_Dirtiness");
        float sizeFactor = GetSizeFactor();
        float effectiveCleanAmount = cleanAmount * sizeFactor;
        float newDirtiness = Mathf.Clamp(currentDirtiness - effectiveCleanAmount, 0f, 1f);
        mat.SetFloat("_Dirtiness", newDirtiness);

        if (manager != null)
        {
            manager.OnSurfaceCleaned(currentDirtiness - newDirtiness);
        }

        return currentDirtiness - newDirtiness; // Return the actual amount cleaned
    }
    public float GetSizeFactor()
    {
        // formula: half of magnitude of size of the bounding box of the renderer
        return rend.bounds.size.magnitude / 2f;
    }
}
