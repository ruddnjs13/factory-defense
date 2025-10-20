using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Code.LKW.ETC
{
    public class RoundDecal : MonoBehaviour
    {
        [SerializeField] private DecalProjector decalProjector;
        [SerializeField] private float depth = 20f; // Depth of the decal

        public void SetProjectionActive(bool isActive)
        {
            decalProjector.enabled = isActive;
        }

        public void SetDecalSize(float size)
        {
            decalProjector.size = new Vector3(2 * size, 2 * size, depth);
        }
    }
}