using UnityEngine;

public class DirtySurfaceBehavior : MonoBehaviour
{
    Renderer rend;
    Material mat;
    [SerializeField] float maxDirtiness = 10f;

    float tempTimer = 0f;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        mat = rend.material;
        mat.SetVector("_Scale", rend.bounds.size);
    }

    void Update()
    {
        // mat.SetVector("_Scale", transform.localScale);
        if (tempTimer < maxDirtiness)
        {
            tempTimer += Time.deltaTime;
            mat.SetFloat("_Dirtiness", tempTimer / maxDirtiness);
        }
    }
}
