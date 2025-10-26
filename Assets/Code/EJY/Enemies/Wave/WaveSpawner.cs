using System.Collections;
using Code.Enemies;
using RuddnjsLib.Dependencies;
using RuddnjsPool.RuddnjsLib.Pool.RunTime;
using UnityEngine;

namespace Code.EJY.Enemies.Wave
{
    public class WaveSpawner : MonoBehaviour
    {
        [SerializeField] private float spawnTimeBetweenWaves = 120f, spawnTerm = 0.5f;
        [SerializeField] private Transform spawnTrm, targetTrm;
        [SerializeField] private StageWaveDataSO spawnData;

        [Inject] private PoolManagerMono _poolManager;

        private float _currentTime = 0f;
        private int _currentWaveTotalEnemyCnt = 0;
        private int _deadEnemyCnt = 0;
        private bool _inProgress = false;
        
        public int TotalWave => spawnData.stageSpawnData.Count;
        public int CurrentWave { get; private set; } = 0;
        

        private void Update()
        {
            // 웨이브 진행 중이ㅁ
            if(_inProgress) return;
            
            _currentTime += Time.deltaTime;
            if (_currentTime >= spawnTimeBetweenWaves)
            {
                ProcessWave();
            }
        }

        private void ProcessWave()
        {
            _inProgress = true;
            _currentTime = 0f;
            _currentWaveTotalEnemyCnt = 0;
            _deadEnemyCnt = 0;
            StartCoroutine(WaveCoroutine());
            CurrentWave++;
        }

        private IEnumerator WaveCoroutine()
        {
            // 현재 웨이브의 데이터 리스트를 전부 순회
            foreach (var data in spawnData.stageSpawnData[CurrentWave].dataList)
            {
                _currentWaveTotalEnemyCnt += data.spawnCnt;
                for (int i = 0; i < data.spawnCnt; i++)
                {
                    FSMEnemy enemy =
                        _poolManager.Pop<FSMEnemy>(data.enemyPoolItem);
                    enemy.transform.position = spawnTrm.position;
                    enemy.Init(targetTrm,CheckInProgress);

                    yield return new WaitForSeconds(spawnTerm);
                }
            }
        }

        private void CheckInProgress()
        {
            _deadEnemyCnt++;
            _inProgress = _currentWaveTotalEnemyCnt == _deadEnemyCnt;
        }
    }
}