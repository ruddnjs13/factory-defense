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
        public static float Interpolation { get; private set; }
        
        private float accumulator;
        private float lastTime;
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
            lastTime = Time.realtimeSinceStartup;
        }

        void Update()
        {
            float currentTime = Time.realtimeSinceStartup;
            float frameDelta = currentTime - lastTime;
            lastTime = currentTime;

            accumulator += frameDelta;

            while (accumulator >= tickInterval)
            {
                ExecuteTick(tickInterval);
                accumulator -= tickInterval;
            }

            Interpolation = Mathf.Clamp01(accumulator / tickInterval);
        }

        private void ExecuteTick(float dt)
        {
            deltaTime = dt;

            foreach (var tickCompo in tickables.ToArray())
            {
                if (tickCompo == null || (tickCompo is UnityEngine.Object obj && obj == null))
                {
                    UnregisterTick(tickCompo);
                    continue;
                }

                tickCompo.OnTick(dt);
            }
        }
    }
}