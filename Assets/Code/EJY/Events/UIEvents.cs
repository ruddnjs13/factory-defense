using Core.GameEvent;

namespace Code.Events
{
    public static class UIEvents
    {
        public static WaveTimerEvent WaveTimerEvent = new WaveTimerEvent();
        public static WaveInfoEvent WaveInfoEvent = new WaveInfoEvent();
        public static ChangeWaveProgress ChangeWaveProgress = new ChangeWaveProgress();
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
}