using System.Collections.Generic;
using UnityEngine;

public class DirtyRoomManager : MonoBehaviour
{
    DirtySurfaceBehavior[] dirtySurfaces;
    [SerializeField] float totalCleanedAmount = 0f; // range between 0 and totalSizeFactor
    [SerializeField] float totalSizeFactor = 0f;
    private void Start()
    {
        dirtySurfaces = transform.GetComponentsInChildren<DirtySurfaceBehavior>();
        if (dirtySurfaces.Length == 0)
        {
            Debug.LogWarning("No dirty surfaces found in children of " + gameObject.name);
        }

        foreach (DirtySurfaceBehavior surface in dirtySurfaces)
        {
            surface.SetDirtinessProportion(1f); // Start fully dirty
            totalSizeFactor += surface.GetSizeFactor();
            surface.SetManager(this);
        }
    }

    /// <summary>
    /// Updates the total cleaned amount
    /// </summary>
    /// <param name="cleanAmount"></param>
    public void OnSurfaceCleaned(float cleanAmount)
    {
        totalCleanedAmount += cleanAmount;
        totalCleanedAmount = Mathf.Clamp(totalCleanedAmount, 0f, totalSizeFactor);
        // Debug.Log("Total cleaned amount: " + totalCleanedAmount + " out of " + totalSizeFactor);
    }

    /// <summary>
    /// Returns the cleanliness percentage of the room, between 0 and 1.
    /// </summary>
    /// <returns></returns>
    public float GetCleanlinessPercentage()
    {
        if (totalSizeFactor == 0f)
        {
            return 1f; // If there are no surfaces, consider it fully clean
        }
        return totalCleanedAmount / totalSizeFactor;
    }

    /// <summary>
    /// Checks if the room is fully cleaned within a certain threshold. Returns 1 if fully cleaned, 0 otherwise.
    /// </summary>
    /// <returns>True if fully cleaned, false otherwise.</returns>
    public bool CheckIfFullyCleaned()
    {
        return GetCleanlinessPercentage() >= 0.9999f; // Consider it fully clean if it's 99% or more clean to account for floating point imprecision
    }
}
