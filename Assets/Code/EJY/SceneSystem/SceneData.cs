using System;
using System.Collections.Generic;
using System.Linq;
using Code.EJY.Enemies.Wave;
using Code.EJY.UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.SceneSystem
{
    [CreateAssetMenu(fileName = "SceneData", menuName = "SO/SceneData", order = 0)]
    public class SceneData : ScriptableObject
    {
        public StageWaveDataSO waveData;
        [FormerlySerializedAs("stageName")] public string sceneName;
        public string displayName;
        public bool canEnter = false;

        private Dictionary<EnemyData, Sprite> _enemyIconDict = new Dictionary<EnemyData, Sprite>();
        
        private int EnemyKind => _enemyIconDict.Count;
        
        private void OnEnable()
        {
            // 입장가능한지 데이터 불러오기
            if(string.IsNullOrEmpty(sceneName) || !PlayerPrefs.HasKey(sceneName) || canEnter) return;
            
            SerializeSceneData data = JsonUtility.FromJson<SerializeSceneData>(PlayerPrefs.GetString(sceneName));
            canEnter = data.canEnter;
        }

        public void SaveData()
        {
            SerializeSceneData data = new SerializeSceneData(canEnter);
            string json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(sceneName, json);
            PlayerPrefs.Save();
        }

        public void DeleteData()
        {
            PlayerPrefs.DeleteKey(sceneName);
            PlayerPrefs.Save();
        }

        public void SetEnemyIcons(List<EnemyIcon> enemyIcons)
        {
            for (int i = 0; i < EnemyKind; ++i)
            {
                enemyIcons[i].gameObject.SetActive(true);
                enemyIcons[i].SetIcon(_enemyIconDict.Values.ElementAt(i));
            }
            for (int i = EnemyKind; i < enemyIcons.Count; ++i)
            {
                enemyIcons[i].gameObject.SetActive(false);
            }
        }

        private void OnValidate()
        {
            sceneName = name;
            
            if(waveData == null) return;

            foreach (var waveDataList in waveData.stageSpawnData)
            {
                foreach (var spawnData in waveDataList.dataList)
                {
                    if (!_enemyIconDict.ContainsKey(spawnData.enemyData))
                    {
                        _enemyIconDict.Add(spawnData.enemyData, spawnData.enemyData.enemyIcon);
                    }
                }
            }
            
            _enemyIconDict = _enemyIconDict.OrderBy(data => data.Key.priority)
                .ToDictionary(data => data.Key, data => data.Value);
        }
    }

    [Serializable]
    public class SerializeSceneData
    {
        public bool canEnter;

        public SerializeSceneData(bool canEnter)
        {
            this.canEnter = canEnter;
        }
    }
}