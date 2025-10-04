using Code.Core;
using Code.Sounds;
using Core.GameEvent;
using UnityEngine;

namespace Code.Events
{
    public static class SoundsEvents
    {
        public static PlaySFXEvent PlaySfxEvent = new PlaySFXEvent();
        public static PlayBGMEvent PlayBGMEvent = new PlayBGMEvent();
        public static StopBGMEvent StopBGMEvent = new StopBGMEvent();
        public static PlayLongSFXEvent PlayLongSFXEvent = new PlayLongSFXEvent();
        public static StopLongSFXEvent StopLongSFXEvent = new StopLongSFXEvent();
    }

    public class PlaySFXEvent : GameEvent
    {
        public Vector3 position;
        public SoundSO soundClip;

        public PlaySFXEvent Init(Vector3 position, SoundSO soundClip)
        {
            this.position = position;
            this.soundClip = soundClip;
            return this;
        }
    }
    public class PlayBGMEvent : GameEvent
    {
        public SoundSO soundClip;

        public PlayBGMEvent Init(SoundSO soundClip)
        {
            this.soundClip = soundClip;
            return this;
        }
    }

    public class StopBGMEvent : GameEvent
    {
        
    }

    public class PlayLongSFXEvent : GameEvent
    {
        public Vector3 position;
        public SoundSO soundClip;
        public int id;

        public PlayLongSFXEvent Init(Vector3 position, SoundSO soundClip, int id)
        {
            this.position = position;
            this.soundClip = soundClip;
            this.id = id;
            return this;
        }
    }
    
    public class StopLongSFXEvent : GameEvent
    {
        public int id;

        public StopLongSFXEvent Init(int id)
        {
            this.id = id;
            return this;
        }
    }
}