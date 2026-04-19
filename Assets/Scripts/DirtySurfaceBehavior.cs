using UnityEngine;

public class DirtySurfaceBehavior : MonoBehaviour
{
    DirtyRoomManager manager;
    Renderer rend;
    Material mat;
    float dirtiness;
    float maxDirtiness;
    [SerializeField] float dirtinessProportion;
    float cleanThreshold = 0.1f; // The threshold below which we consider the surface clean

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        mat = rend.material;
        mat.SetVector("_Scale", rend.bounds.size);

        // The maximum dirtiness is based on the size of the object, 
        // so bigger objects are dirtier and take more effort to clean
        maxDirtiness = GetSizeFactor();
    }

    public void SetManager(DirtyRoomManager manager)
    {
        // This method is currently not used, but it could be used to register this surface with the manager if needed.
        this.manager = manager;
    }

    /// <summary>
    /// Sets the dirtiness of the surface and updates the material accordingly. 
    /// The input value is clamped between 0 and maxDirtiness, and the dirtinessProportion is 
    /// updated based on the new dirtiness value.
    /// </summary>
    /// <param name="value"></param>
    public void SetDirtiness(float value)
    {
        dirtiness = value;
        dirtinessProportion = Mathf.Clamp(dirtiness / maxDirtiness, 0f, 1f);
        mat.SetFloat("_Dirtiness", dirtinessProportion);
    }

    /// <summary>
    /// Sets the dirtiness proportion directly, which is a value between 0 and 1. 
    /// This will update the actual dirtiness value based on the maxDirtiness and update the material accordingly.
    /// Called by the manager at the start to initialize the surfaces as fully dirty.
    /// </summary>
    /// <param name="proportion"></param>
    public void SetDirtinessProportion(float proportion)
    {
        proportion = Mathf.Clamp(proportion, 0f, 1f);
        dirtinessProportion = proportion;
        dirtiness = proportion * maxDirtiness;
        mat.SetFloat("_Dirtiness", dirtinessProportion);
    }

    /// <summary>
    /// Reduces the dirtiness of the surface by the cleanAmount modified by the size of the object being cleaned.
    /// </summary>
    /// <returns>The actual amount of dirt cleaned</returns>
    public float CleanDirtSomeAmount(float cleanAmount)
    {
        float newDirtiness = Mathf.Clamp(dirtiness - cleanAmount, 0f, maxDirtiness);
        SetDirtiness(newDirtiness);

        if (dirtinessProportion < cleanThreshold)
        {
            DirtExecute();
        }

        if (manager != null)
        {
            manager.OnSurfaceCleaned(cleanAmount);
        }
        return cleanAmount; // Return the actual amount cleaned
    }
    public void DirtExecute()
    {
        float remainder = dirtiness;
        SetDirtiness(0);
        manager.OnSurfaceCleaned(remainder);
    }
    public float GetSizeFactor()
    {
        return Mathf.Log10(rend.bounds.size.magnitude);
    }
    
    public bool IsDirty()
    {
        return dirtinessProportion > cleanThreshold;
    }
}
