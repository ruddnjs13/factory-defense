using Code.Core;
using Core.GameEvent;
using UnityEngine;

namespace Code.Events
{
    public static  class TextEvents
    {
        public static PopupTextEvent PopupTextEvent = new PopupTextEvent();
    }
    
    public class PopupTextEvent : GameEvent
    {
        public string text;
        public int textTypeHash;
        public Vector3 position;
        public float showDuration;

        public PopupTextEvent Initializer(string text, int typeHash, Vector3 position, float showDuration)
        {
            this.text = text;
            textTypeHash = typeHash;
            this.position = position;
            this.showDuration = showDuration;
            return this;
        }
    }
}