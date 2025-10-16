using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code.SHS.TickSystem
{
    [AddComponentMenu("")]
    public class TickManager : MonoBehaviour
    {
        public static float DeltaTime => deltaTime;
        public static float deltaTime { get; private set; }
        public static readonly float tickInterval = 0.1f;
        private float lastTickTime;
        private static List<ITick> tickables = new List<ITick>();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            GameObject tickManagerObject = new GameObject("TickManager");
            DontDestroyOnLoad(tickManagerObject);
            tickManagerObject.AddComponent<TickManager>();
        }

        public static void RegisterTick(ITick tick)
        {
            if (!tickables.Contains(tick))
            {
                tickables.Add(tick);
            }
        }

        public static void UnregisterTick(ITick tick)
        {
            if (tickables.Contains(tick))
            {
                tickables.Remove(tick);
            }
        }

        void Start()
        {
            lastTickTime = Time.time;
            StartCoroutine(TickCoroutine());
        }


        private IEnumerator TickCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(tickInterval);
                ExecuteTick();
            }
        }

        private void ExecuteTick()
        {
            float currentTime = Time.time;
            deltaTime = currentTime - lastTickTime;
            lastTickTime = currentTime;

            foreach (var tickCompo in tickables.ToArray())
            {
                tickCompo.OnTick(deltaTime);

                if (tickCompo.gameObject == null)
                    UnregisterTick(tickCompo);
            }
        }
    }
}