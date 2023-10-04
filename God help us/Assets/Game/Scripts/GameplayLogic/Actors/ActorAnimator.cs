using System;
using System.Collections.Generic;
using Game.Scripts.GameplayLogic.Animations;
using UnityEngine;

namespace Game.Scripts.GameplayLogic.Actors
{
    public class ActorAnimator : MonoBehaviour, IAnimationStateReader
    {
        public event Action<AnimatorState> StateEntered;
        public event Action<AnimatorState> StateExited;
        
        private readonly Dictionary<int, AnimatorState> _animationStates = new()
        {
            {AnimatorParamKey.Idle, AnimatorState.Idle},
            {AnimatorParamKey.Walking, AnimatorState.Walking},
            {AnimatorParamKey.Hit, AnimatorState.Hit},
        };
        
        private Animator _animator;
        private AnimatorState _animatorState;
        
        private void Awake() =>
            _animator = GetComponent<Animator>();

        public void PlayWalking(bool isWalking) => 
            _animator.SetBool(AnimatorParamKey.Walking, isWalking);

        public void PlayHit() => 
            _animator.SetTrigger(AnimatorParamKey.Hit);

        public void EnteredState(int stateHash)
        {
            _animatorState = StateFor(stateHash);
            StateEntered?.Invoke(_animatorState);
        }

        public void ExitedState(int stateHash) =>
            StateExited?.Invoke(StateFor(stateHash));
        
        private AnimatorState StateFor(int stateHash) => 
            _animationStates.ContainsKey(stateHash) ? _animationStates[stateHash] : AnimatorState.Idle;
    }
}