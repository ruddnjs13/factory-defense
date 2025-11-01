using Code.SceneSystem;
using Core.GameEvent;
using UnityEngine;

namespace Code.Events
{
    public static class UIEvents
    {
        public static WaveTimerEvent WaveTimerEvent = new WaveTimerEvent();
        public static WaveInfoEvent WaveInfoEvent = new WaveInfoEvent();
        public static ChangeWaveProgress ChangeWaveProgress = new ChangeWaveProgress();
        public static SelectStageEvent SelectStageEvent = new SelectStageEvent();
        public static GameResultEvent GameResultEvent = new GameResultEvent();
    }

    public class WaveTimerEvent : GameEvent
    {
        public float timer;

        public WaveTimerEvent Initializer(float timer)
        {
            this.timer = timer;
            return this;
        }
    }

    public class WaveInfoEvent : GameEvent
    {
        public int enemyCnt;
        public int currentWave;

        public WaveInfoEvent Initializer(int enemyCnt,int currentWave)
        {
            this.enemyCnt = enemyCnt;
            this.currentWave = currentWave;
            return this;
        }
    }

    public class ChangeWaveProgress : GameEvent
    {
        public bool inProgress;

        public ChangeWaveProgress Initializer(bool inProgress)
        {
            this.inProgress = inProgress;
            return this;
        }
    }

    public class SelectStageEvent : GameEvent
    {
        public Vector3 position;
        public SceneData data;
        
        public SelectStageEvent Initializer(Vector3 position, SceneData data)
        {
            this.position = position;
            this.data = data;
            return this;
        }
    }

    public class GameResultEvent : GameEvent
    {
        public bool isClear;

        public GameResultEvent Initializer(bool isClear)
        {
            this.isClear = isClear;
            return this;
        }
    }
}