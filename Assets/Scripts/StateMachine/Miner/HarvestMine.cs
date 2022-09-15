using UnityEngine;

namespace StateMachine
{
    public class HarvestMine : IState
    {
        private readonly MinerAI _minerAI;
        private readonly Animator _animator;
        private float _resourcesPerSecond = 2;
        private float _timer;
        
        private static readonly int Mine = Animator.StringToHash("Mine");

        public HarvestMine(MinerAI minerAI, Animator animator)
        {
            _minerAI = minerAI;
            _animator = animator;
        }

        public void Tick()
        {
            if (_minerAI.ResourceArea != null)
            {
                _timer += Time.deltaTime;

                if (_timer >= _resourcesPerSecond)
                {
                    _minerAI.TakeFromTarget();
                    // Debug.Log("Gathered Gem From Mine");
                    _animator.SetTrigger(Mine);
                }
            }
        }

        public void OnEnter()
        {
            _resourcesPerSecond = Random.Range(1.5f, 3f);
            // Debug.Log("Harvest Started");
        }

        public void OnExit()
        {
            // Debug.Log("Harvest Ended");
            _timer = 0;
        }
    }
}