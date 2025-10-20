using UnityEngine;

namespace Chipmunk.ComponentContainers
{
    /// <summary>
    /// 수동으로 초기화해야 하는 ComponentContainer
    /// Awake에서 자동 초기화되지 않으며, ManualInitialize()를 직접 호출해야 함
    /// </summary>
    [DisallowMultipleComponent]
    public class ManualComponentContainer : ComponentContainer
    {
        /// <summary>
        /// 초기화 여부를 확인합니다.
        /// </summary>
        public bool IsInitialized => isInitialized;
        protected override void Awake()
        {
            // Awake에서는 딕셔너리만 초기화하고 컴포넌트는 초기화하지 않음
            InitializeDictionaries();
        }

        /// <summary>
        /// 컴포넌트를 수동으로 초기화합니다.
        /// 이 메서드를 호출하기 전까지 컴포넌트들은 초기화되지 않습니다.
        /// </summary>
        public void ManualInitialize()
        {
            if (isInitialized)
            {
                Debug.LogWarning($"ManualComponentContainer on {gameObject.name} is already initialized.");
                return;
            }

            AddComponentToDictionary();
            ComponentInitialize();
            AfterInitialize();
            isInitialized = true;
        }

    }
}

