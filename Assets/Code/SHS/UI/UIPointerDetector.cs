using System;
using UnityEngine;

namespace Chipmunk.UI
{
    public class UIPointerDetector : MonoBehaviour
    {
        public static bool IsPointerInUI { get; private set; } = false;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void OnAfterSceneLoad()
        {
            Initailize();
        }

        private static void Initailize()
        {
            UIPointerDetector uiPointerDetector = FindFirstObjectByType<UIPointerDetector>();
            if (uiPointerDetector == null)
            {
                GameObject detectorObject = new GameObject("IsPointerInUIDetector");
                uiPointerDetector = detectorObject.AddComponent<UIPointerDetector>();
                DontDestroyOnLoad(detectorObject);
                Debug.Log($"<color=green>UIPointerDetector</color> : Initailized");
            }
        }

        private void LateUpdate()
        {
            IsPointerInUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
        }
    }   
}