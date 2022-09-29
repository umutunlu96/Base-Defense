using System;
using Enums;
using UnityEngine;

namespace Controllers
{
    public class PlayerAnimationController : MonoBehaviour
    {
        #region Variables
        
        #region Serialize Variables

        [SerializeField] private Animator animator;
        
        #endregion

        #region Private Variavles
        
        private static readonly int Idle = Animator.StringToHash("Idle");
        private static readonly int Run = Animator.StringToHash("Run");
        private static readonly int HoldPistol = Animator.StringToHash("HoldPistol");
        private static readonly int HoldRifle = Animator.StringToHash("HoldRifle");

        
        #endregion

        #endregion
        
        public void TranslatePlayerAnimationState(PlayerAnimationState state)
        {
            switch (state)
            {
                case PlayerAnimationState.Idle:
                    animator.SetTrigger(Idle);
                    break;
                case PlayerAnimationState.Run:
                    animator.SetTrigger(Run);
                    break;
                case PlayerAnimationState.HoldPistol:
                    animator.SetTrigger(HoldPistol);
                    break;
                case PlayerAnimationState.HoldRifle:
                    animator.SetTrigger(HoldRifle);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
    }
}