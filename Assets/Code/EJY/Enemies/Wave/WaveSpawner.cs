using System;
using System.Collections;
using Code.Enemies;
using Code.Events;
using Core.GameEvent;
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
        [SerializeField] private GameEventChannelSO uiChannel;

        [Inject] private PoolManagerMono _poolManager;

        private float _currentTime = 0f;
        private int _currentWaveTotalEnemyCnt = 0;
        private int _deadEnemyCnt = 0;
        private bool _inProgress = false;
        
        public int TotalWave => spawnData.stageSpawnData.Count;
        public int CurrentWave { get; private set; } = 0;

        private void Awake()
        {
            _currentTime = spawnTimeBetweenWaves;
        }

        private void Start()
        {
            uiChannel.RaiseEvent(UIEvents.WaveInfoEvent.Initializer(GetWaveTotalEnemyCnt(), CurrentWave + 1));
        }

        private void Update()
        {
            // 웨이브 진행 중이ㅁ
            if(_inProgress && CurrentWave == spawnData.stageSpawnData.Count) return;
            
            _currentTime -= Time.deltaTime;
            uiChannel.RaiseEvent(UIEvents.WaveTimerEvent.Initializer(_currentTime));
            if (_currentTime <= 0)
            {
                ProcessWave();
            }
        }

        public void ProcessWave()
        {
            _inProgress = true;
            _currentTime = spawnTimeBetweenWaves;
            _currentWaveTotalEnemyCnt = GetWaveTotalEnemyCnt();

            _deadEnemyCnt = 0;
            StartCoroutine(WaveCoroutine());
            CurrentWave++;
        }

        private int GetWaveTotalEnemyCnt()
        {
            int total = 0;
            
            foreach (var data in spawnData.stageSpawnData[CurrentWave].dataList)
                total += data.spawnCnt;
            
            return total;
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
                    enemy.Init(targetTrm,CheckInProgress);

                    yield return new WaitForSeconds(spawnTerm);
                }
            }
        }

        private void CheckInProgress()
        {
            _deadEnemyCnt++;
            _inProgress = _currentWaveTotalEnemyCnt != _deadEnemyCnt;
            if(_inProgress)
                uiChannel.RaiseEvent(UIEvents.WaveInfoEvent.Initializer(GetWaveTotalEnemyCnt(), CurrentWave + 1));
        }
    }
}