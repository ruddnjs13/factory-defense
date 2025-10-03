using Blade.Core;
using UnityEngine;

namespace Blade.Events
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

        public PopupTextEvent Init(string text, int typeHash, Vector3 position, float showDuration)
        {
            this.text = text;
            textTypeHash = typeHash;
            this.position = position;
            this.showDuration = showDuration;
            return this;
        }
    }
}