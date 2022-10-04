using Signals;
using UnityEngine;

namespace Abstract
{
    public abstract class Enemy : MonoBehaviour
    {
        public int Health;
        public int Damage;
        public float AttackRange;
        public float ChaseRange;
        public float ChaseUpdateSpeed = .2f;
        public float WalkSpeed;
        public float RunSpeed;
        
        protected void TurnOnEnemy()
        {
            gameObject.SetActive(true);
        }
        
        protected void TurnOffEnemy()
        {
            gameObject.SetActive(false);
        }
        
        // protected Enemy(int health, int damage, float attackRange, float chaseRange, float chaseUpdateSpeed,
        //     float walkSpeed, float runSpeed)
        // {
        //     Health = health;
        //     Damage = damage;
        //     AttackRange = attackRange;
        //     ChaseRange = chaseRange;
        //     ChaseUpdateSpeed = chaseUpdateSpeed;
        //     WalkSpeed = walkSpeed;
        //     RunSpeed = runSpeed;
        // }
    }
}