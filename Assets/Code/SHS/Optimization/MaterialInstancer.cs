using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(MeshRenderer))]
public class MaterialInstancer : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private MaterialPropertyBlock mpb;

    private static readonly int ColorID = Shader.PropertyToID("_Color");

    void Awake()
    {
        Initialize();
    }

    void OnEnable()
    {
        Initialize();
        ApplyProperties();
    }

    void OnValidate()
    {
        Initialize();
        ApplyProperties();
    }

    private void Initialize()
    {
        if (meshRenderer == null)
            meshRenderer = GetComponent<MeshRenderer>();

        if (mpb == null)
            mpb = new MaterialPropertyBlock();

        // Ensure the renderer has the property block set (prevents lost state in editor)
        if (meshRenderer != null)
            meshRenderer.SetPropertyBlock(mpb);
    }

    private void ApplyProperties()
    {
        Debug.Assert(mpb != null, "MaterialPropertyBlock is not initialized.");
    }
    void Update()
    {
        #if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            ApplyProperties();
        }
        #endif
    }
}