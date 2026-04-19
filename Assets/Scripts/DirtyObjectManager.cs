using System.Collections.Generic;
using UnityEngine;

public class DirtyRoomManager : MonoBehaviour
{
    DirtySurfaceBehavior[] dirtySurfaces;
    float totalCleanedAmount = 0f; // range between 0 and totalSizeFactor
    float totalSizeFactor = 0f;
    private void Awake()
    {
        dirtySurfaces = transform.GetComponentsInChildren<DirtySurfaceBehavior>();
        if (dirtySurfaces.Length == 0)
        {
            Debug.LogWarning("No dirty surfaces found in children of " + gameObject.name);
        }

        foreach (DirtySurfaceBehavior surface in dirtySurfaces)
        {
            surface.SetDirtiness(1f);
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
}
