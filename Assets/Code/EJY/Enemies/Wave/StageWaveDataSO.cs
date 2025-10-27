using System;
using System.Collections.Generic;
using RuddnjsPool;
using UnityEngine;

namespace Code.EJY.Enemies.Wave
{
    [CreateAssetMenu(fileName = "WaveData", menuName = "SO/Enemy/Wave", order = 0)]
    public class StageWaveDataSO : ScriptableObject
    {
        // 총 웨이브 데이터
        public List<DataList> stageSpawnData;
    }

    [Serializable]
    // 몹 한 종류당 데이터
    public struct SpawnData
    {
        public PoolingItemSO enemyPoolItem;
        public int spawnCnt;
    }

    [Serializable]
    // 한 웨이브 데이터
    public struct DataList
    {
        public List<SpawnData> dataList;
    }
}