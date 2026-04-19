using UnityEngine;

public class PaintBehavior : MonoBehaviour
{
    public Color color = Color.black;
    public float spreadRadius = 0.1f;
    public float range = 10;

    public void Paint(Vector3 position, Quaternion direction)
    {
        bool foundSurface = Physics.SphereCast(position, spreadRadius, direction * Vector3.forward, out RaycastHit hit, range);
        if (foundSurface && hit.collider.TryGetComponent(out DirtySurfaceBehavior surface))
        {
            surface.Paint(this, hit);
        }
    }
}
