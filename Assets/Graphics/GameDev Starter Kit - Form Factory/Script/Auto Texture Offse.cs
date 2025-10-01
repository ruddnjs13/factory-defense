using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class AutoTextureOffset : MonoBehaviour
{
    [Tooltip("Speed of texture scrolling in X and Y directions")]
    public Vector2 scrollSpeed = new Vector2(0f, -0.5f);
    
    [Tooltip("The material index to apply the offset to (if multiple materials)")]
    public int materialIndex = 1;
    
    private Renderer rend;
    private Vector2 offset = Vector2.zero;
    
    void Start()
    {
        rend = GetComponent<Renderer>();
        
        // Initialize with current offset if one exists
        if (rend.materials.Length > materialIndex)
        {
            offset = rend.materials[materialIndex].mainTextureOffset;
        }
    }
    
    void Update()
    {
        if (rend == null || rend.materials.Length <= materialIndex)
            return;
            
        // Calculate new offset
        offset += scrollSpeed * Time.deltaTime;
        
        // Apply the offset to the material
        rend.materials[materialIndex].mainTextureOffset = offset;
    }
}