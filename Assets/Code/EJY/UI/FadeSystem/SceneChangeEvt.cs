using Code.Events;
using Core.GameEvent;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.UI.FadeSystem
{
    public class SceneChangeEvt : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO sceneChannel;
        
        private void Awake()
        {
            sceneChannel.AddListener<ChangeSceneEvent>(HandleChangeSceneEvent);
        }

        private void OnDestroy()
        {
            sceneChannel.RemoveListener<ChangeSceneEvent>(HandleChangeSceneEvent);
        }

        public void SceneChange(string targetSceneName) => sceneChannel.RaiseEvent(SceneEvents.FadeEvent.Initializer(true, targetSceneName));
        public void SendFadeOutEvent() => sceneChannel.RaiseEvent(SceneEvents.FadeEvent.Initializer(false, string.Empty));
        public void ReloadCurrentScene() => sceneChannel.RaiseEvent(SceneEvents.FadeEvent.Initializer(true, SceneManager.GetActiveScene().name));

        private void HandleChangeSceneEvent(ChangeSceneEvent evt)
        {
            if(string.IsNullOrEmpty(evt.sceneName)) return;
            SceneManager.LoadScene(evt.sceneName);
        }
    }
}