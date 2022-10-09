using DG.Tweening;
using Enums;
using Signals;
using UnityEngine;

namespace StateMachine.Enemy
{
    public class Death : IState
    {
        private readonly EnemyAI _enemyAI;
        private readonly Animator _animator;
        private readonly Renderer _renderer;
        private readonly EnemyType _enemyType;
        private static readonly int death = Animator.StringToHash("Death");

        public Death(EnemyAI enemyAI, Animator animator, Renderer renderer, EnemyType enemyType)
        {
            _enemyAI = enemyAI;
            _animator = animator;
            _renderer = renderer;
            _enemyType = enemyType;
        }
        
        public void Tick()
        {
        }

        public void OnEnter()
        {
            _animator.applyRootMotion = false;
            _animator.SetTrigger(death);
            AiSignals.Instance.onEnemyDead?.Invoke(_enemyAI.transform);
            PoolSignals.Instance.onGetPoolObject?.Invoke("Money", _enemyAI.transform);
            AiSignals.Instance.onEnemyAIDead?.Invoke(_enemyAI);
        }

        public void OnExit()
        {
            
        }
    }
}