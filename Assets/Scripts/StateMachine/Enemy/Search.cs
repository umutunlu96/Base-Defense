using UnityEngine;
using UnityEngine.AI;

namespace StateMachine.Enemy
{
    public class Search : IState
    {
        private readonly EnemyAI _enemyAI;

        public Search(EnemyAI enemyAI)
        {
            _enemyAI = enemyAI;
        }
        
        public void Tick()
        {
        }

        public void OnEnter()
        {
            Transform target = AiSignals.Instance.onGetBaseAttackPoint();
            _enemyAI.BaseTarget = target;
            _enemyAI.CurrentTarget = target;
        }

        public void OnExit()
        {

        }
    }
}