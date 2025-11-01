using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class MeshCombiner : MonoBehaviour
{
    private MeshFilter _meshFilter;

    private IEnumerator Start()
    {
        yield return null; // 씬 로드 완료 대기

        _meshFilter = GetComponent<MeshFilter>();
        _meshFilter.mesh = new Mesh();
        CombineMesh();
        gameObject.AddComponent<BoxCollider>();
    }

    private void CombineMesh()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>(false);
        var combine = new CombineInstance[meshFilters.Length];
        int vertexCount = 0;

        for (int i = 0; i < meshFilters.Length; i++)
        {
            var sharedMesh = meshFilters[i].sharedMesh;
            if (sharedMesh == null) continue;

            if (!sharedMesh.isReadable )
            {
                Debug.LogWarning($"[MeshCombiner] {sharedMesh.name} is not readable. Skipped.");
                continue;
            }

            combine[i].mesh = sharedMesh;
            combine[i].transform = transform.worldToLocalMatrix * meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
            vertexCount += sharedMesh.vertexCount;
        }

        _meshFilter.mesh = new Mesh();
        if (vertexCount > 65535)
            _meshFilter.mesh.indexFormat = IndexFormat.UInt32;

        _meshFilter.mesh.CombineMeshes(combine, true, true);
        gameObject.SetActive(true);
    }
}
