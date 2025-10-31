using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.SceneSystem
{
    [CreateAssetMenu(fileName = "SceneData", menuName = "SO/SceneData", order = 0)]
    public class SceneData : ScriptableObject
    {
        [FormerlySerializedAs("stageName")] public string sceneName;
        public string displayName;
        public bool canEnter = false;

        private void OnEnable()
        {
            // 입장가능한지 데이터 불러오기
            if(string.IsNullOrEmpty(sceneName) || !PlayerPrefs.HasKey(sceneName)) return;
            
            SerializeSceneData data = JsonUtility.FromJson<SerializeSceneData>(PlayerPrefs.GetString(sceneName));
            canEnter = data.canEnter;
        }

        public void SaveData()
        {
            if (canEnter) return;
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

        private void OnValidate()
        {
            sceneName = name;
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