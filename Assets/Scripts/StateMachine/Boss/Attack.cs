using Signals;
using UnityEngine;

namespace StateMachine.Boss
{
    public class Attack : IState
    {
        private readonly BossAI _bossAI;
        private readonly Animator _animator;
        private readonly Transform _hand;
        private readonly Transform _bombInitialTransform;
        private readonly Transform _playerTransform;
        
        private float _timer = 2.2f;
        
        private static readonly int attack = Animator.StringToHash("Attack");
        public Attack(BossAI bossAI, Animator animator, Transform playerTransform, Transform hand, Transform bombInitial)
        {
            _bossAI = bossAI;
            _animator = animator;
            _hand = hand;
            _bombInitialTransform = bombInitial;
            _playerTransform = playerTransform;
        }
        
        public void Tick()
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                ThrowGrenade();
                _timer = 2.2f;
            }
            RotateTowardsPlayer();
        }

        public void OnEnter()
        {
            ThrowGrenade();
        }

        public void OnExit()
        {
            
        }

        private void RotateTowardsPlayer()
        {
            float singleStep = Time.deltaTime * 2;
            Vector3 targetDirection = _playerTransform.position - _bossAI.transform.position;
            targetDirection.y = 0;
            Vector3 newDirection = Vector3.RotateTowards(_bossAI.transform.forward, targetDirection, singleStep, 0f);
            _bossAI.transform.rotation = Quaternion.LookRotation(newDirection);
        }
        
        private void ThrowGrenade()
        {
            GameObject grenade = GetGrenade();
            AiSignals.Instance.onGrenadeThrowed?.Invoke();
            grenade.transform.SetParent(_hand);
            grenade.transform.localPosition = _bombInitialTransform.localPosition;
            _animator.SetTrigger(attack);
        }

        private GameObject GetGrenade() => PoolSignals.Instance.onGetPoolObject?.Invoke("Grenade", _hand);
    }
}