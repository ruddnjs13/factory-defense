using Code.Events;
using Core.GameEvent;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Code.SceneSystem
{
    public class SceneEnter : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private SceneData sceneData;
        [SerializeField] private GameEventChannelSO uiChannel;
        
        [Header("Present Lock Settings")]
        [SerializeField] private bool isPresentLocked = false;
        [SerializeField] private Color lockedColor = Color.gray;
        private Image _sceneEnterImage;
        private TextMeshProUGUI _sceneEnterText;

        private RectTransform Rect => transform as RectTransform;
        
        private void Awake()
        {
            _sceneEnterImage = GetComponent<Image>();
            _sceneEnterText = GetComponentInChildren<TextMeshProUGUI>();
            
            if (isPresentLocked)
            {
                if (!sceneData.canEnter)
                {
                    _sceneEnterImage.color = lockedColor;
                    _sceneEnterText.color = lockedColor;
                }
            }
        }

        private void OnApplicationQuit()
        {
            sceneData.SaveData();
        }

        
        [ContextMenu("Delete Saved Scene Data")]
        public void DeleteSavedSceneData() => sceneData.DeleteData();

        public void OnPointerClick(PointerEventData eventData)
        {
            if(!sceneData.canEnter) return;
            uiChannel.RaiseEvent(UIEvents.SelectStageEvent.Initializer(Rect.anchoredPosition, sceneData));
        }
    }
}