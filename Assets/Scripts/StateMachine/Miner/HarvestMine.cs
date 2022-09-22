using Enums;
using Signals;
using StateMachine.Miner;
using UnityEngine;
using UnityEngine.AI;

namespace StateMachine
{
    public class HarvestMine : IState
    {
        private readonly MinerAI _minerAI;
        private readonly Animator _animator;
        private readonly Transform _gemArea;
        private readonly NavMeshObstacle _navMeshObstacle;
        private float _resourcesPerSecond = 5;
        private float _timer;
        
        private static readonly int Mine = Animator.StringToHash("Mine");

        public HarvestMine(MinerAI minerAI, Animator animator, Transform gemArea, NavMeshObstacle navMeshObstacle)
        {
            _minerAI = minerAI;
            _animator = animator;
            _gemArea = gemArea;
            _navMeshObstacle = navMeshObstacle;
        }

        public void Tick()
        {
            if (_minerAI.GemArea != null)
            {
                _timer += Time.deltaTime;

                if (_timer >= _resourcesPerSecond)
                {
                    _minerAI.TakeFromTarget();
                }
            }
        }

        public void OnEnter()
        {
            _navMeshObstacle.enabled = true;
            _minerAI.transform.LookAt(_gemArea);
            _animator.SetTrigger(Mine);
        }

        public void OnExit()
        {
            _navMeshObstacle.enabled = false;
            _timer = 0;
            _minerAI.Gem = PoolSignals.Instance.onGetPoolObject?.Invoke(PoolType.Gem, _minerAI.DiamondTransform);
            if (_minerAI.Gem == null) return;
            _minerAI.Gem.transform.SetParent(_minerAI.DiamondTransform);
            _minerAI.Gem.transform.rotation = Quaternion.identity;
        }
    }
}