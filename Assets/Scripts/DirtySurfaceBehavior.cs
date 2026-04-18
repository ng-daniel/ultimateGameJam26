using UnityEngine;

public class DirtySurfaceBehavior : MonoBehaviour
{
    Material mat;
    [SerializeField] float maxDirtiness = 1f;

    private void Awake()
    {
        mat = GetComponent<Renderer>().material;
        mat.SetVector("Scale", transform.localScale);
    }

    void Update()
    {
        mat.SetVector("_Scale", transform.localScale);
    }
}
