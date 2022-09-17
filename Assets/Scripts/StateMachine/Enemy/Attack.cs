using TMPro.EditorUtilities;
using UnityEngine;

namespace StateMachine.Enemy
{
    public class Attack : IState
    {
        private readonly EnemyAI _enemyAI;
        private readonly Animator _animator;
        private static readonly int attack = Animator.StringToHash("Attack");
        private float _timer;

        public Attack(EnemyAI enemyAI, Animator animator)
        {
            _enemyAI = enemyAI;
            _animator = animator;
        }
        
        
        public void Tick()
        {
            _timer += Time.deltaTime;

            if (_timer >= 5)
            {
                Attaaack();
            }
        }

        public void OnEnter()
        {
            Attaaack();
        }

        public void OnExit()
        {
            
        }

        private void Attaaack()
        {
            _animator.SetTrigger(attack);
            Debug.Log("Attack");
            //Playersignals.instance.onPlayerGetDamage?.Invoke(_enemyAi.Damage);
            _timer = 0;
        }
    }
}