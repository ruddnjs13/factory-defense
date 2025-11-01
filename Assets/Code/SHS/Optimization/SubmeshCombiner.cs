using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

namespace Code.SHS.Optimization
{
    public class SubmeshCombiner : MonoBehaviour
    {
        [SerializeField] MeshFilter meshFilter;
        [Tooltip("Textures corresponding to each submesh (index order must match submesh index)")]
        [SerializeField] Texture[] textures;
        [Tooltip("Optional material to assign after atlas creation. If null, renderer.sharedMaterial will be used (or a default Standard material created).")]
        [SerializeField] Material outputMaterial;
        [SerializeField] int atlasMaxSize = 2048;
        [SerializeField] int padding = 2;
        [SerializeField] bool assignMaterialToRenderer = true;

#if UNITY_EDITOR
        [ContextMenu("Combine Submeshes")]
        public void Combine()
        {
            // Original atlas-building Combine implementation stays here and is editor-only
            if (meshFilter == null)
            {
                Debug.LogWarning("meshFilter is not assigned.");
                return;
            }

            Mesh mesh = meshFilter.sharedMesh;
            if (mesh == null)
            {
                Debug.LogWarning("No mesh found on meshFilter.");
                return;
            }

            int subMeshCount = mesh.subMeshCount;
            if (subMeshCount <= 1)
            {
                Debug.Log("Mesh already has 1 or fewer submeshes. Nothing to combine.");
                return;
            }

            if (textures == null || textures.Length < subMeshCount)
            {
                Debug.LogWarning($"textures length ({(textures==null?0:textures.Length)}) is less than submesh count ({subMeshCount}). Fill missing entries if you want proper atlas mapping.");
            }

            // Convert provided textures to Texture2D[] for PackTextures
            Texture2D[] tex2D = new Texture2D[subMeshCount];
            bool[] texCreated = new bool[subMeshCount];
            for (int i = 0; i < subMeshCount; i++)
            {
                Texture t = (textures != null && i < textures.Length) ? textures[i] : null;
                tex2D[i] = ToTexture2D(t, out texCreated[i]);
            }

            // Create atlas using PackTextures. PackTextures ignores null entries, so provide a 1x1 white if missing.
            for (int i = 0; i < tex2D.Length; i++)
            {
                if (tex2D[i] == null)
                {
                    bool linearDefault = (QualitySettings.activeColorSpace == ColorSpace.Linear);
                    tex2D[i] = new Texture2D(1, 1, TextureFormat.RGBA32, false, linearDefault);
                    tex2D[i].SetPixel(0, 0, Color.white);
                    tex2D[i].Apply();
                    texCreated[i] = true; // mark as temp so we can cleanup
                }
            }

            bool atlasLinear = (QualitySettings.activeColorSpace == ColorSpace.Linear);
            Texture2D atlas = new Texture2D(1, 1, TextureFormat.RGBA32, false, atlasLinear);
             Rect[] rects = atlas.PackTextures(tex2D, padding, atlasMaxSize);
            atlas.Apply();

            // Debug: compare center pixel color of each source texture and atlas region to detect color shifts
            const float colorTolerance = 0.03f; // adjust as needed
            for (int i = 0; i < rects.Length && i < tex2D.Length; i++)
            {
                Texture2D src = tex2D[i];
                Rect r = rects[i];
                if (src == null) continue;

                // sample center of source and atlas
                Color srcCol = src.GetPixelBilinear(0.5f, 0.5f);
                Color atlasCol = atlas.GetPixelBilinear(r.x + r.width * 0.5f, r.y + r.height * 0.5f);
                float diff = Mathf.Max(Mathf.Abs(srcCol.r - atlasCol.r), Mathf.Abs(srcCol.g - atlasCol.g), Mathf.Abs(srcCol.b - atlasCol.b));
                if (diff > colorTolerance)
                {
                    Debug.LogWarning($"Atlas color mismatch for index {i}: src={srcCol} atlas={atlasCol} diff={diff:F4} (tolerance {colorTolerance})");
                }
                else
                {
                    Debug.Log($"Atlas color OK for index {i}: src={srcCol} atlas={atlasCol} diff={diff:F4}");
                }
            }

            // cleanup temporary textures created for packing
            for (int i = 0; i < tex2D.Length; i++)
            {
                if (texCreated[i] && tex2D[i] != null)
                {
                    if (Application.isPlaying) Object.Destroy(tex2D[i]); else Object.DestroyImmediate(tex2D[i]);
                    tex2D[i] = null;
                }
            }

            // Read mesh data
            Vector3[] oldVerts = mesh.vertices;
            Vector3[] oldNormals = mesh.normals;
            Vector4[] oldTangents = mesh.tangents;
            Vector2[] oldUV = mesh.uv;
            Color[] oldColors = mesh.colors;

            System.Collections.Generic.List<Vector3> newVerts = new System.Collections.Generic.List<Vector3>();
            System.Collections.Generic.List<Vector3> newNormals = new System.Collections.Generic.List<Vector3>();
            System.Collections.Generic.List<Vector4> newTangents = new System.Collections.Generic.List<Vector4>();
            System.Collections.Generic.List<Vector2> newUV = new System.Collections.Generic.List<Vector2>();
            var combinedColors = new System.Collections.Generic.List<Color>();
            var combinedIndices = new System.Collections.Generic.List<int>();

            // For each submesh, copy triangles and duplicate vertices so UV can be remapped per-submesh
            for (int si = 0; si < subMeshCount; si++)
            {
                int[] tris = mesh.GetTriangles(si);
                float rx = si < rects.Length ? rects[si].x : 0f;
                float ry = si < rects.Length ? rects[si].y : 0f;
                float rw = si < rects.Length ? rects[si].width : 1f;
                float rh = si < rects.Length ? rects[si].height : 1f;

                for (int t = 0; t < tris.Length; t++)
                {
                    int oldIndex = tris[t];
                    if (oldIndex < 0 || oldIndex >= oldVerts.Length) continue; // safety
                    int newIndex = newVerts.Count;

                    // add vertex attributes
                    newVerts.Add(oldVerts[oldIndex]);
                    if (oldNormals != null && oldNormals.Length > oldIndex) newNormals.Add(oldNormals[oldIndex]);
                    if (oldTangents != null && oldTangents.Length > oldIndex) newTangents.Add(oldTangents[oldIndex]);
                    if (oldColors != null && oldColors.Length > oldIndex) combinedColors.Add(oldColors[oldIndex]);

                    Vector2 uv = (oldUV != null && oldUV.Length > oldIndex) ? oldUV[oldIndex] : Vector2.zero;
                    // remap uv into atlas rect
                    Vector2 remapped = new Vector2(rx + uv.x * rw, ry + uv.y * rh);
                    newUV.Add(remapped);

                    combinedIndices.Add(newIndex);
                }
            }

            Mesh combinedMesh = new Mesh();
            combinedMesh.name = mesh.name + "_Combined";

            // Support large meshes
            if (newVerts.Count > 65535)
            {
                combinedMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            }

            combinedMesh.SetVertices(newVerts);
            if (newNormals.Count == newVerts.Count) combinedMesh.SetNormals(newNormals);
            if (newTangents.Count == newVerts.Count) combinedMesh.SetTangents(newTangents);
            if (combinedColors.Count == newVerts.Count) combinedMesh.SetColors(combinedColors);
            combinedMesh.SetUVs(0, newUV);

            combinedMesh.SetTriangles(combinedIndices, 0); // single submesh
            combinedMesh.RecalculateBounds();
            if (newNormals.Count == 0) combinedMesh.RecalculateNormals();

            // Assign combined mesh
            meshFilter.sharedMesh = combinedMesh;

            // Assign material and atlas
            Renderer rend = meshFilter.GetComponent<Renderer>();
            Material mat = outputMaterial;
            if (mat == null)
            {
                if (rend != null && rend.sharedMaterial != null)
                {
                    mat = new Material(rend.sharedMaterial);
                }
                else
                {
                    mat = new Material(Shader.Find("Standard"));
                }
            }

            // Use property IDs to avoid string-based lookup warnings
            int mainTexId = Shader.PropertyToID("_MainTex");
            int baseMapId = Shader.PropertyToID("_BaseMap");

            if (mat.HasProperty(mainTexId)) mat.SetTexture(mainTexId, atlas);
            else if (mat.HasProperty(baseMapId)) mat.SetTexture(baseMapId, atlas);

            if (assignMaterialToRenderer && rend != null)
            {
                rend.sharedMaterial = mat;
            }

            // Save generated assets into Resources for reuse
            SaveAssetsToResources(atlas, combinedMesh, mat, mesh.name);

            Debug.Log($"Combined {subMeshCount} submeshes into one mesh. Atlas size: {atlas.width}x{atlas.height}");
        }
#else
        [ContextMenu("Combine Submeshes")]
        public void Combine()
        {
            Debug.LogWarning("Combine (atlas) is editor-only. Use CombineMeshOnly() at runtime or in builds.");
        }
#endif

        [ContextMenu("Combine Mesh Only")]
        public void CombineMeshOnly()
        {
            if (meshFilter == null)
            {
                Debug.LogWarning("meshFilter is not assigned.");
                return;
            }

            Mesh mesh = meshFilter.sharedMesh;
            if (mesh == null)
            {
                Debug.LogWarning("No mesh found on meshFilter.");
                return;
            }

            int subMeshCount = mesh.subMeshCount;
            if (subMeshCount <= 1)
            {
                Debug.Log("Mesh already has 1 or fewer submeshes. Nothing to combine.");
                return;
            }

            // Read mesh data
            Vector3[] oldVerts = mesh.vertices;
            Vector3[] oldNormals = mesh.normals;
            Vector4[] oldTangents = mesh.tangents;
            Vector2[] oldUV = mesh.uv;
            Color[] oldColors = mesh.colors;

            var newVerts = new System.Collections.Generic.List<Vector3>(oldVerts.Length);
            var newNormals = new System.Collections.Generic.List<Vector3>(oldVerts.Length);
            var newTangents = new System.Collections.Generic.List<Vector4>(oldVerts.Length);
            var newUV = new System.Collections.Generic.List<Vector2>(oldVerts.Length);
            var combinedColors = new System.Collections.Generic.List<Color>(oldVerts.Length);
            var combinedIndices = new System.Collections.Generic.List<int>(mesh.triangles.Length);

            // Map from original vertex index -> new vertex index (to preserve sharing)
            var indexMap = new System.Collections.Generic.Dictionary<int, int>(oldVerts.Length);

            for (int si = 0; si < subMeshCount; si++)
            {
                int[] tris = mesh.GetTriangles(si);
                for (int t = 0; t < tris.Length; t++)
                {
                    int oldIndex = tris[t];
                    if (oldIndex < 0 || oldIndex >= oldVerts.Length) continue; // safety

                    if (!indexMap.TryGetValue(oldIndex, out int newIndex))
                    {
                        newIndex = newVerts.Count;
                        indexMap.Add(oldIndex, newIndex);

                        // copy vertex attributes
                        newVerts.Add(oldVerts[oldIndex]);
                        if (oldNormals != null && oldNormals.Length > oldIndex) newNormals.Add(oldNormals[oldIndex]);
                        if (oldTangents != null && oldTangents.Length > oldIndex) newTangents.Add(oldTangents[oldIndex]);
                        if (oldColors != null && oldColors.Length > oldIndex) combinedColors.Add(oldColors[oldIndex]);
                        Vector2 uv = (oldUV != null && oldUV.Length > oldIndex) ? oldUV[oldIndex] : Vector2.zero;
                        newUV.Add(uv);
                    }

                    combinedIndices.Add(newIndex);
                }
            }

            Mesh combinedMesh = new Mesh();
            combinedMesh.name = mesh.name + "_CombinedMeshOnly";
            if (newVerts.Count > 65535) combinedMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

            combinedMesh.SetVertices(newVerts);
            if (newNormals.Count == newVerts.Count) combinedMesh.SetNormals(newNormals);
            if (newTangents.Count == newVerts.Count) combinedMesh.SetTangents(newTangents);
            if (combinedColors.Count == newVerts.Count) combinedMesh.SetColors(combinedColors);
            combinedMesh.SetUVs(0, newUV);
            combinedMesh.SetTriangles(combinedIndices, 0);
            combinedMesh.RecalculateBounds();
            if (newNormals.Count == 0) combinedMesh.RecalculateNormals();

            meshFilter.sharedMesh = combinedMesh;

#if UNITY_EDITOR
            // Save combined mesh to Resources so it persists across editor sessions
            SaveMeshToResources(combinedMesh, mesh.name);
#endif

            Debug.Log($"Combined mesh only for {mesh.name}, submeshes {subMeshCount} -> 1 (UVs preserved, shared vertices kept)");
        }

#if UNITY_EDITOR
        void SaveAssetsToResources(Texture2D atlas, Mesh meshAsset, Material matAsset, string baseName)
        {
            if (atlas == null || meshAsset == null || matAsset == null)
                return;

            string resourcesDir = "Assets/Resources/Generated";
            if (!Directory.Exists(resourcesDir))
            {
                Directory.CreateDirectory(resourcesDir);
            }

            string atlasPngPath = Path.Combine(resourcesDir, baseName + "_atlas.png").Replace("\\", "/");
            string meshPath = Path.Combine(resourcesDir, baseName + "_mesh.asset").Replace("\\", "/");
            string matPath = Path.Combine(resourcesDir, baseName + "_mat.asset").Replace("\\", "/");

            // Remove existing assets to overwrite
            AssetDatabase.DeleteAsset(atlasPngPath);
            AssetDatabase.DeleteAsset(meshPath);
            AssetDatabase.DeleteAsset(matPath);

            // Write atlas PNG to disk
            try
            {
                // Ensure we write an sRGB PNG so Unity imports it as expected for color textures.
                Texture2D exportTex = null;
                RenderTexture prevRT = RenderTexture.active;
                RenderTexture rt = RenderTexture.GetTemporary(atlas.width, atlas.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.sRGB);
                Graphics.Blit(atlas, rt);
                RenderTexture.active = rt;
                exportTex = new Texture2D(atlas.width, atlas.height, TextureFormat.RGBA32, false, false); // linear=false => sRGB
                exportTex.ReadPixels(new Rect(0, 0, atlas.width, atlas.height), 0, 0);
                exportTex.Apply();

                byte[] png = exportTex.EncodeToPNG();
                File.WriteAllBytes(atlasPngPath, png);

                // cleanup
                RenderTexture.active = prevRT;
                RenderTexture.ReleaseTemporary(rt);
                if (exportTex != null)
                {
#if UNITY_EDITOR
                    Object.DestroyImmediate(exportTex);
#else
                    Object.Destroy(exportTex);
#endif
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to write atlas PNG: {e}");
                return;
            }

            // Import the newly created PNG and ensure it's readable
            AssetDatabase.ImportAsset(atlasPngPath, ImportAssetOptions.ForceSynchronousImport | ImportAssetOptions.ForceUpdate);
            TextureImporter ti = AssetImporter.GetAtPath(atlasPngPath) as TextureImporter;
            if (ti != null)
            {
                bool dirty = false;
                if (!ti.isReadable) { ti.isReadable = true; dirty = true; }
                if (ti.textureType != TextureImporterType.Default) { ti.textureType = TextureImporterType.Default; dirty = true; }
                if (ti.mipmapEnabled) { ti.mipmapEnabled = false; dirty = true; }
                // Ensure sRGB (color) texture so colors match original albedo
                if (!ti.sRGBTexture) { ti.sRGBTexture = true; dirty = true; }
                if (dirty) ti.SaveAndReimport();
            }

            // Load imported atlas and assign to material (if present)
            if (AssetDatabase.LoadAssetAtPath<Texture2D>(atlasPngPath) is Texture2D importedAtlas)
            {
                int mainTexId = Shader.PropertyToID("_MainTex");
                int baseMapId = Shader.PropertyToID("_BaseMap");
                if (matAsset.HasProperty(mainTexId)) matAsset.SetTexture(mainTexId, importedAtlas);
                else if (matAsset.HasProperty(baseMapId)) matAsset.SetTexture(baseMapId, importedAtlas);
            }

            // Create mesh and material assets
            AssetDatabase.CreateAsset(meshAsset, meshPath);
            AssetDatabase.CreateAsset(matAsset, matPath);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"Saved generated assets to {resourcesDir}");
        }

        void SaveMeshToResources(Mesh meshAsset, string baseName)
        {
            if (meshAsset == null) return;

            string resourcesDir = "Assets/Resources/Generated";
            if (!Directory.Exists(resourcesDir)) Directory.CreateDirectory(resourcesDir);

            string meshPath = Path.Combine(resourcesDir, baseName + "_mesh.asset").Replace("\\", "/");

            // Remove existing asset if present
            Mesh existing = AssetDatabase.LoadAssetAtPath<Mesh>(meshPath);
            if (existing != null)
            {
                AssetDatabase.DeleteAsset(meshPath);
            }

            // Clone the mesh to store as an independent asset (avoid referencing scene instance)
            Mesh meshToSave = Object.Instantiate(meshAsset);
            meshToSave.name = baseName + "_mesh";

            // Create the mesh asset
            AssetDatabase.CreateAsset(meshAsset, meshPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"Saved combined mesh asset to {meshPath}");
        }
#endif

#if UNITY_EDITOR
        // Convert arbitrary Texture to Texture2D. If it's already Texture2D and readable, cast and return.
        Texture2D ToTexture2D(Texture t, out bool created)
        {
            created = false;
            if (t == null) return null;

            // If it's a Texture2D and already readable, return it directly to avoid extra copy
            if (t is Texture2D tex2d && tex2d.isReadable)
            {
                return tex2d;
            }

            // For non-readable Texture2D or other Texture types, render into a RenderTexture and read pixels
            int w = t.width > 0 ? t.width : 1;
            int h = t.height > 0 ? t.height : 1;

            // Create a temporary RenderTexture with same dimensions using project color space
            RenderTextureReadWrite rrw = (QualitySettings.activeColorSpace == ColorSpace.Linear) ? RenderTextureReadWrite.Linear : RenderTextureReadWrite.sRGB;
            RenderTexture rt = RenderTexture.GetTemporary(w, h, 0, RenderTextureFormat.Default, rrw);
            RenderTexture prev = RenderTexture.active;

            // Ensure correct filtering/wrap by setting active RT and blitting
            Graphics.Blit(t, rt);
            RenderTexture.active = rt;

            bool linearFlag = (QualitySettings.activeColorSpace == ColorSpace.Linear);
            Texture2D readableTex = new Texture2D(w, h, TextureFormat.RGBA32, false, linearFlag);
            readableTex.ReadPixels(new Rect(0, 0, w, h), 0, 0);
            readableTex.Apply();

            RenderTexture.active = prev;
            RenderTexture.ReleaseTemporary(rt);

            created = true;
            return readableTex;
        }
#endif
    }
}

