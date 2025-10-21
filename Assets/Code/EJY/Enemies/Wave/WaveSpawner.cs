using System;
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
        [SerializeField] private float timeBetweenWaves = 30f;
        [SerializeField] private int maxEnemyCnt = 8, minEnemyCnt = 3;
        [SerializeField] private Transform spawnTrm, targetTrm;
        [SerializeField] private WaveDataSO[] data;

        [Inject] private PoolManagerMono _poolManager; 
        
        private int _enemyCnt;

        private void Start()
        {
            StartCoroutine(WaveCoroutine());
        }

        private IEnumerator WaveCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(timeBetweenWaves);
            
                _enemyCnt = Random.Range(minEnemyCnt, maxEnemyCnt);

                for (int i = 0; i < _enemyCnt; ++i)
                {
                    int idx = Random.Range(0, data.Length);
                
                    
                    FSMEnemy enemy = _poolManager.Pop<FSMEnemy>(data[idx].enemyPoolItem);
                    enemy.transform.position = spawnTrm.position;
                    enemy.Init(targetTrm);
                    
                    yield return new WaitForSeconds(0.5f);
                }
            }
        }
    }
}