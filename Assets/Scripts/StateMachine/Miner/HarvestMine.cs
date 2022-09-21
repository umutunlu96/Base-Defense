using StateMachine.Miner;
using UnityEngine;

namespace StateMachine
{
    public class HarvestMine : IState
    {
        private readonly MinerAI _minerAI;
        private readonly Animator _animator;
        private readonly Transform _gemArea;
        private float _resourcesPerSecond = 5;
        private float _timer;
        
        private static readonly int Mine = Animator.StringToHash("Mine");

        public HarvestMine(MinerAI minerAI, Animator animator, Transform gemArea)
        {
            _minerAI = minerAI;
            _animator = animator;
            _gemArea = gemArea;
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
            _minerAI.transform.LookAt(_gemArea);
            _animator.SetTrigger(Mine);
        }

        public void OnExit()
        {
            _timer = 0;
        }
    }
}