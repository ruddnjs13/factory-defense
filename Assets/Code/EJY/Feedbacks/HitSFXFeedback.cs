using System.Collections.Generic;
using System.Linq;
using Code.Combat;
using Code.Effects;
using Code.Entities;
using Code.Events;
using Code.Sounds;
using Core.GameEvent;
using RuddnjsPool;
using UnityEngine;

namespace Code.Feedbacks
{
    public class HitSFXFeedback : Feedback
    {
        [SerializeField] private GameEventChannelSO soundChannel;
        [SerializeField] private PoolManagerSO poolManager;
        [SerializeField] private ActionData actionData;
        private Dictionary<DamageType, SoundSO> _hitImpactDict;

        private void Awake()
        {
            _hitImpactDict = new Dictionary<DamageType, SoundSO>();
            GetComponentsInChildren<HitSFX>().ToList()
                .ForEach(x => _hitImpactDict.Add(x.AllowedDamageType, x.SoundClip));
        }

        public override void CreateFeedback()
        {
            SoundSO soundSO = _hitImpactDict.GetValueOrDefault(actionData.DamageData.damageType);
            Debug.Assert(soundSO != null,
                $"DamageType can not found, damage type is {actionData.DamageData.damageType}");
            soundChannel.RaiseEvent(SoundsEvents.PlaySfxEvent.Init(transform.position, soundSO));
        }

        public override void StopFeedback()
        {
        }
    }
}