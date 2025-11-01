using Code.Events;
using Core.GameEvent;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Code.UI.FadeSystem
{
    public class FadeCanvas : MonoBehaviour
    {
        [SerializeField] private Image fadeImage;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private GameEventChannelSO sceneChannel;
        [SerializeField] private float duration = 3f;
        [SerializeField] private bool inStartFadeIn;

        private readonly int _scrollHash = Shader.PropertyToID("_Scroll");
        private readonly int _rotateHash = Shader.PropertyToID("_Rotate"); 

        private void Awake()
        {
            sceneChannel.AddListener<FadeEvent>(HandleFadeEvent);
            SetCanvasGroup(false);
        }

        private void Start()
        {
            fadeImage.material = new Material(fadeImage.material);

            if (inStartFadeIn)
            {
                Fade(false);
            }
        }

        private void OnDestroy()
        {
            sceneChannel.RemoveListener<FadeEvent>(HandleFadeEvent);
        }

        private void HandleFadeEvent(FadeEvent evt)
        {
            Fade(evt.isFadeIn, evt.sceneName);
        }

        private void SetCanvasGroup(bool isStart)
        {
            canvasGroup.alpha = isStart ? 1 : 0;
            canvasGroup.interactable = isStart;
            canvasGroup.blocksRaycasts = isStart;
        }

        [ContextMenu("Test Fade In")]
        public void FadeIn() => Fade(true);
        [ContextMenu("Test Fade Out")]
        public void FadeOut() => Fade(false);
        
        private void Fade(bool isFadeIn, string sceneName = null)
        {
            int startScrollValue = isFadeIn ? 0 : 4;
            int endScrollValue = isFadeIn ? 4 : 0;
            float startRotateValue = 0f;
            float endRotateValue = 1.55f;
            
            fadeImage.material.SetFloat(_scrollHash, startScrollValue);
            fadeImage.material.SetFloat(_rotateHash, startRotateValue);
            
            SetCanvasGroup(true);

            Sequence seq = DOTween.Sequence();
            
            seq.SetUpdate(true);
            seq.Append(fadeImage.material.DOFloat(endScrollValue, _scrollHash, duration))
                .Join(fadeImage.material.DOFloat(endRotateValue, _rotateHash, duration))
                    .OnComplete(() =>
                    {
                        if (isFadeIn && !string.IsNullOrEmpty(sceneName))
                        {
                            Time.timeScale = 1;
                            SetCanvasGroup(false);
                            sceneChannel.RaiseEvent(SceneEvents.ChangeSceneEvent.Initializer(sceneName));
                        }
                        else
                        {
                            SetCanvasGroup(false);
                        }
                    });
        }
    }
}