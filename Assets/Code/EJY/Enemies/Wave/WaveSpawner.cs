using System.Collections;
using Code.Enemies;
using RuddnjsLib.Dependencies;
using RuddnjsPool.RuddnjsLib.Pool.RunTime;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.EJY.Enemies.Wave
{
    public class WaveSpawner : MonoBehaviour
    {
        [SerializeField] private float spawnTimeBetweenWaves = 120f, spawnTerm = 0.5f;
        [SerializeField] private Transform spawnTrm, targetTrm;
        [SerializeField] private StageWaveDataSO spawnData;

        [Inject] private PoolManagerMono _poolManager;

        private float _currentTime = 0f;

        public int TotalWave => spawnData.stageSpawnData.Count;
        public int CurrentWave { get; private set; } = 0;

        private void Update()
        {
            _currentTime += Time.deltaTime;
            if (_currentTime >= spawnTimeBetweenWaves)
            {
                ProcessWave();
            }
        }

        private void ProcessWave()
        {
            _currentTime = 0f;
            StartCoroutine(WaveCoroutine());
            CurrentWave++;
        }

        private IEnumerator WaveCoroutine()
        {
            // 현재 웨이브의 데이터 리스트를 전부 순회
            foreach (var data in spawnData.stageSpawnData[CurrentWave].dataList)
            {
                for (int i = 0; i < data.spawnCnt; i++)
                {
                    FSMEnemy enemy =
                        _poolManager.Pop<FSMEnemy>(data.enemyPoolItem);
                    enemy.transform.position = spawnTrm.position;
                    enemy.Init(targetTrm);

                    yield return new WaitForSeconds(spawnTerm);
                }
            }
        }
    }
}