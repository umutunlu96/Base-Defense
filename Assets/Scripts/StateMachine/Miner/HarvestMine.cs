using Enums;
using Signals;
using UnityEngine;
using UnityEngine.AI;

namespace StateMachine.Miner
{
    public class HarvestMine : IState
    {
        private readonly MinerAI _minerAI;
        private readonly Animator _animator;
        private readonly Transform _gemArea;
        private readonly NavMeshObstacle _navMeshObstacle;
        private readonly MineWorkerType _workerType;
        private float _resourcesPerSecond = 5;
        private float _timer;
        
        private static readonly int Mine = Animator.StringToHash("Mine");
        private static readonly int Gather = Animator.StringToHash("Gather");

        public HarvestMine(MinerAI minerAI, Animator animator, MineWorkerType workerType , Transform gemArea, NavMeshObstacle navMeshObstacle)
        {
            _minerAI = minerAI;
            _animator = animator;
            _gemArea = gemArea;
            _navMeshObstacle = navMeshObstacle;
            _workerType = workerType;
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
            if (_workerType == MineWorkerType.Miner)
            {
                _animator.SetTrigger(Mine);
                return;
            }

            if (_workerType == MineWorkerType.Gatherer)
            {
                _animator.SetTrigger(Gather);
                _minerAI.PickAxeTransform.gameObject.SetActive(false);
            }
        }

        public void OnExit()
        {
            _minerAI.PickAxeTransform.gameObject.SetActive(true);
            _navMeshObstacle.enabled = false;
            _timer = 0;
            _minerAI.Gem = PoolSignals.Instance.onGetPoolObject?.Invoke(PoolType.Gem, _minerAI.DiamondTransform);
            if (_minerAI.Gem == null) return;
            _minerAI.Gem.transform.SetParent(_minerAI.DiamondTransform);
            _minerAI.Gem.transform.rotation = Quaternion.identity;
        }
    }
}