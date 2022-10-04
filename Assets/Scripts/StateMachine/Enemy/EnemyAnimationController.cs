using UnityEngine;

namespace StateMachine.Enemy
{
    public class EnemyAnimationController : MonoBehaviour
    {
        [SerializeField] private EnemyAI manager;
        [SerializeField] private Animator animator;

        public void Attacked() => manager.Attacked = true;
        public void AttackAnimEnded() => manager.AttackAnimEnded = true;
        public void DeathAnimEnd() => manager.Death();
    }
}