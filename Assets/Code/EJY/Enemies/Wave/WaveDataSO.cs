using Code.Enemies;
using UnityEngine;

namespace Code.EJY.Enemies.Wave
{
    [CreateAssetMenu(fileName = "WaveData", menuName = "SO/Enemy/Wave", order = 0)]
    public class WaveDataSO : ScriptableObject
    {
        public FSMEnemy enemyPrefab;
    }
}