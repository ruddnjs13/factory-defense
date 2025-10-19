using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Code.SHS.Machines
{
    [CreateAssetMenu(fileName = "new ConveyorSO", menuName = "Machine/ConveyorSO", order = 0)]
    public class ConveyorSO : MachineSO
    {
        public List<ConveyorData> conveyorDataList;

        private void Awake()
        {
            foreach (ConveyorData data in conveyorDataList)
            {
                data.Setup();
            }
        }
    }

    [System.Serializable]
    public struct ConveyorData
    {
        [SerializeField] private List<Vector2Int> inputPositions;
        public HashSet<Vector2Int> InputPositions { get; private set; }
        public GameObject prefab;

        public void Setup()
        {
            InputPositions = new HashSet<Vector2Int>(inputPositions);
        }
    }
}