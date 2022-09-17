using UnityEngine;
using UnityEngine.AI;

namespace StateMachine.Enemy
{
    public class Search : IState
    {
        private readonly EnemyAI _enemyAI;
        private readonly AttackSide _attackSide;
        
        public Search(EnemyAI enemyAI, AttackSide attackSide)
        {
            _enemyAI = enemyAI;
            _attackSide = attackSide;
        }
        
        public void Tick()
        {
        }

        public void OnEnter()
        {
            Transform target = AiSignals.Instance.onGetBaseAttackPoint(_attackSide);
            _enemyAI.BaseTarget = target;
            _enemyAI.CurrentTarget = target;
        }

        public void OnExit()
        {

        }
    }
}