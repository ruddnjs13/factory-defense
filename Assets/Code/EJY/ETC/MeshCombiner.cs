using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

namespace Code.EJY.ETC
{
    public class MeshCombiner : MonoBehaviour
    {
        [SerializeField] private Grid mapGrid;
        
        private MeshFilter _meshFilter;
        
        private void Awake()
        {
            _meshFilter = GetComponent<MeshFilter>();
            _meshFilter.mesh = new Mesh();

            CombineMesh();
        }

        private void CombineMesh()
        {
            //자식에 있는 메시필터들을 전부 가져온다.
            MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>(false);
            
            CombineInstance[] combine = new CombineInstance[meshFilters.Length];

            int vertexCount = 0;
            for (int i = 0; i < meshFilters.Length; i++)
            {
                //별도의 메시 인스턴스를 만든녀석은 합칠수 없다.
                if(meshFilters[i].sharedMesh == null) continue;
                
                combine[i].mesh = meshFilters[i].sharedMesh;
                combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
                
                //수집이 완료된 게임오브젝트는 액티브를 꺼준다.
                meshFilters[i].gameObject.SetActive(false);
                
                vertexCount += meshFilters[i].sharedMesh.vertexCount;
            }
            
            _meshFilter.mesh = new Mesh(); //새로운 메시 객체를 할당한다.
            if (vertexCount > 65535) //익덱싱이 2바이트를 초과할경우는 int형태로 버퍼를 전환한다.
                _meshFilter.mesh.indexFormat = IndexFormat.UInt32;
            
            _meshFilter.mesh.CombineMeshes(combine); //수집된 콤바인들을 메시로 합친다.
            gameObject.SetActive(true); //자기 자신은 다시 켜줌으로서 완성시킨다.
        }
    }
}