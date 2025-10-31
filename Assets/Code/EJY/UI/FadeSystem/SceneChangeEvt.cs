using Code.Events;
using Core.GameEvent;
using UnityEngine;

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

        private void HandleChangeSceneEvent(ChangeSceneEvent evt)
        {
            if(string.IsNullOrEmpty(evt.sceneName)) return;
            SceneChange(evt.sceneName);
        }
    }
}