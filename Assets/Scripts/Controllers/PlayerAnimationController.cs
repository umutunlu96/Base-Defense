using System;
using Enums;
using Signals;
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
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int Death = Animator.StringToHash("Death");
        private static readonly int HoldPistol = Animator.StringToHash("HoldPistol");
        private static readonly int HoldRifle = Animator.StringToHash("HoldRifle");
        
        #endregion

        #endregion

        public void SetSpeed(float speed) => animator.SetFloat(Speed, speed);
        
        public void EnableAimLayer() => animator.SetLayerWeight(1, 1);
        
        public void DisableAimLayer() => animator.SetLayerWeight(1, 0);

        public void OnDeathAnimComplete() => PlayerSignals.Instance.onPlayerDeadAnimComplete?.Invoke();
        
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
                case PlayerAnimationState.Death:
                    animator.SetTrigger(Death);
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