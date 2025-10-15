using System;
using Chipmunk.ComponentContainers;
using UnityEngine;

namespace Code.SHS.Animations
{
    [RequireComponent(typeof(Animator)), DisallowMultipleComponent]
    public class AnimatorTrigger : MonoBehaviour, IContainerComponent
    {
        public event Action OnAnimationEnd;
        public event Action OnAnimationTrigger;

        public ComponentContainer ComponentContainer { get; set; }

        public void OnInitialize(ComponentContainer componentContainer)
        {
        }

        public void AnimationTrigger() => OnAnimationTrigger?.Invoke();
        public void AnimationEnd() => OnAnimationEnd?.Invoke();
    }
}