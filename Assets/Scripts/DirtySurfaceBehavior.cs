using UnityEngine;

public class DirtySurfaceBehavior : MonoBehaviour
{
    DirtyRoomManager manager;
    Renderer rend;
    Material mat;
    float dirtiness;
    float cleanThreshold = 0.1f; // The threshold below which we consider the surface clean

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
        dirtiness = Mathf.Clamp(value, 0f, 1f);
        mat.SetFloat("_Dirtiness", dirtiness);
    }

    /// <summary>
    /// Reduces the dirtiness of the surface by the cleanAmount modified by the size of the object being cleaned.
    /// </summary>
    /// <returns>The actual amount of dirt cleaned</returns>
    public float CleanDirtSomeAmount(float cleanAmount)
    {
        float currentDirtiness = dirtiness;
        float sizeFactor = GetSizePenalty();
        float effectiveCleanAmount = cleanAmount * sizeFactor;
        float newDirtiness = Mathf.Clamp(currentDirtiness - effectiveCleanAmount, 0f, 1f);
        mat.SetFloat("_Dirtiness", newDirtiness);
        dirtiness = newDirtiness;

        if (dirtiness < cleanThreshold)
        {
            dirtiness = 0f;
            mat.SetFloat("_Dirtiness", 0f);
        }

        if (manager != null)
        {
            manager.OnSurfaceCleaned(effectiveCleanAmount);
        }
        // print($"Went from {cleanAmount} to {effectiveCleanAmount} dirt from surface.");
        return effectiveCleanAmount; // Return the actual amount cleaned
    }
    public float GetSizeFactor()
    {
        return Mathf.Log10(rend.bounds.size.magnitude);
    }
    public float GetSizePenalty()
    {
        return 1f / GetSizeFactor();
    }

    public bool IsDirty()
    {
        return dirtiness > cleanThreshold;
    }
}
