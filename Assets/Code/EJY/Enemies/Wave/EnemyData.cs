using RuddnjsPool;
using UnityEngine;

namespace Code.EJY.Enemies.Wave
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "SO/Enemy/EnemyData", order = 0)]
    public class EnemyData : ScriptableObject
    {
        public PoolingItemSO enemyPoolItem;
        public Sprite enemyIcon;
        public int priority;
    }
}