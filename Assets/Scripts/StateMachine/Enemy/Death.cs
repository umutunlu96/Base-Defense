﻿using DG.Tweening;
using Enums;
using Signals;
using UnityEngine;

namespace StateMachine.Enemy
{
    public class Death : IState
    {
        private readonly EnemyAI _enemyAI;
        private readonly Animator _animator;
        private readonly EnemyType _enemyType;
        private static readonly int death = Animator.StringToHash("Death");

        public Death(EnemyAI enemyAI, Animator animator, EnemyType enemyType)
        {
            _enemyAI = enemyAI;
            _animator = animator;
            _enemyType = enemyType;
        }
        
        public void Tick()
        {
        }

        public void OnEnter()
        {
            _animator.applyRootMotion = false;
            _animator.SetTrigger(death);
            PoolSignals.Instance.onGetPoolObject?.Invoke("Money", _enemyAI.transform);
        }

        public void OnExit()
        {
            
        }
    }
}