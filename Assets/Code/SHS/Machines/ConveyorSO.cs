using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.SHS.Machines
{
    [CreateAssetMenu(fileName = "new ConveyorSO", menuName = "Machine/ConveyorSO", order = 0)]
    public class ConveyorSO : MachineSO
    {
        public List<ConveyorData> conveyorDataList;

        private void Awake()
        {
            Debug.Log("Setting up conveyor data");
            foreach (ConveyorData data in conveyorDataList)
            {
                data.Setup();
            }
        }

        private void OnEnable()
        {
            foreach (ConveyorData data in conveyorDataList)
            {
                data.Setup();
            }
        }
    }

    /// <summary>
    /// 컨베이어 벨트의 입출력 방향과 메시 정보를 담는 데이터 클래스
    /// </summary>
    [System.Serializable]
    public class ConveyorData
    {
        [SerializeField] private List<Direction> inputDirections;
        [SerializeField] private List<Direction> outputDirections;
        public HashSet<Direction> InputDirections { get; private set; }
        public HashSet<Direction> OutputDirections { get; private set; }
        public Mesh mesh;
        public Quaternion rotation;
        public GameObject prefab;

        /// <summary>
        /// 직렬화된 List를 HashSet으로 변환하여 초기화합니다.
        /// </summary>
        public void Setup()
        {
            InputDirections = new HashSet<Direction>(inputDirections);
            OutputDirections = new HashSet<Direction>(outputDirections);
        }
    }
}