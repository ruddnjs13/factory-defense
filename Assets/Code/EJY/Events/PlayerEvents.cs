using Blade.Core;

namespace Blade.Events
{
    public class PlayerEvents
    {
        public static PlayerDeadEvent PlayerDeadEvent = new PlayerDeadEvent();
        public static PlayerHealthEvent PlayerHealthEvent = new PlayerHealthEvent();
        public static PlayerExpEvent PlayerExpEvent = new PlayerExpEvent();
    }

    public class PlayerDeadEvent : GameEvent
    { }

    public class PlayerHealthEvent : GameEvent
    {
        public float health;

        public float maxHealth;
        
        public PlayerHealthEvent Init(float health, float maxHealth)
        {
            this.health = health;
            this.maxHealth = maxHealth;
                return this;
                }
    }
    
    public class PlayerExpEvent : GameEvent
    {
        public float currentExp;
        public float maxExp;
        
        public PlayerExpEvent Init(float currentExp, float maxExp)
        {
            this.currentExp = currentExp;
            this.maxExp = maxExp;
            return this;
        }
    }
}