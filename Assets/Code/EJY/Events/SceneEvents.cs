using Core.GameEvent;

namespace Code.Events
{
    public static class SceneEvents
    {
        public static FadeEvent FadeEvent = new FadeEvent();
        public static ChangeSceneEvent ChangeSceneEvent = new ChangeSceneEvent();
    }
    
    public class FadeEvent : GameEvent
    {
        public bool isFadeIn;
        public string sceneName;

        public FadeEvent Initializer(bool isFadeIn, string sceneName)
        {
            this.isFadeIn = isFadeIn;
            this.sceneName = sceneName;
            return this;
        }
    }
    
    public class ChangeSceneEvent : GameEvent
    {
        public string sceneName;
        public ChangeSceneEvent Init(string sceneName)
        {
            this.sceneName = sceneName;
            return this;
        }
    }
}