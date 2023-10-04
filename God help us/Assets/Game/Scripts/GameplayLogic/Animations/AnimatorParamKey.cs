using UnityEngine;

namespace Game.Scripts.GameplayLogic.Animations
{
    public static class AnimatorParamKey
    {
        public static readonly int Idle = Animator.StringToHash("Idle");
        public static readonly int Walking = Animator.StringToHash("IsWalking");
        public static readonly int Hit = Animator.StringToHash("Hit");
    }
}