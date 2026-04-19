using UnityEngine;
using UnityEngine.Rendering;

public class DirtySurfaceBehavior : MonoBehaviour
{
    Renderer rend;
    Material mat;
    // [SerializeField] float maxDirtiness = 10f;

    Mesh mesh;
    RenderTexture maskRt; // main bitmask texture
    [SerializeField] int textureSize;
    [SerializeField] CommandBuffer cmd;

    Vector2 localBoundsMin;
    Vector2 localBoundsSize;

    Material brushMat;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        mat = rend.material;
        mat.SetVector("_Scale", rend.bounds.size);
        mesh = GetComponent<MeshFilter>().mesh;
    }

    void Update()
    {
        // mat.SetVector("_Scale", transform.localScale);
        // if (tempTimer < maxDirtiness)
        // {
        //     tempTimer += Time.deltaTime;
        //     mat.SetFloat("_Dirtiness", tempTimer / maxDirtiness);
        // }
    }

    void Initialize()
    {
        maskRt = new RenderTexture(textureSize, textureSize, 0, RenderTextureFormat.Default)
        {
            enableRandomWrite = true
        };
        maskRt.Create();
        mat.SetTexture("_MainTex", maskRt);

        // clear the render texture to white (full dirtiness)
        RenderTexture temp = RenderTexture.active;
        RenderTexture.active = maskRt;
        GL.Clear(true, true, Color.white);
        RenderTexture.active = temp;

        // set bounds for mapping world space to UV space in the shader
        Bounds bounds = mesh.bounds;
        localBoundsMin = new Vector2(bounds.min.x, bounds.min.z);
        localBoundsSize = new Vector2(bounds.size.x, bounds.size.z);
        
        mat.SetVector("_BoundsMin", localBoundsMin);
        mat.SetVector("_BoundsSize", localBoundsSize);
    }

    public void Paint(PaintBehavior painter, RaycastHit hit)
    {
        Vector3 localHit = transform.InverseTransformPoint(hit.point);
        
        Vector2 uv;
        uv.x = (localHit.x - localBoundsMin.x) / localBoundsSize.x;
        uv.y = (localHit.z - localBoundsMin.y) / localBoundsSize.y;

        uv = new Vector2(Mathf.Clamp01(uv.x), Mathf.Clamp01(uv.y));

        brushMat.SetTexture("_MainTex", maskRt);
        brushMat.SetVector("_BrushUV", uv);
        brushMat.SetFloat("_Radius", painter.spreadRadius);
        brushMat.SetColor("_Color", painter.color);

        RenderTexture temp = RenderTexture.GetTemporary(maskRt.width, maskRt.height);
        Graphics.Blit(maskRt, temp);
        Graphics.Blit(temp, maskRt, brushMat);
        RenderTexture.ReleaseTemporary(temp);
    }
}
