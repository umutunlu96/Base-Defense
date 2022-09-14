using Enums;
using UnityEngine;

namespace Abstract
{
    public abstract class Enemy : MonoBehaviour
    {
        public Enemy prefab;
        
        public int Health;

        public int Damage;

        public float AttackRange;

        public float Speed;

        public float ChaseSpeed;

        public int Chase;
        
        public EnemyType EnemyType;

        protected Enemy EnemyFactoryMethod()
        {
            return Instantiate(prefab);
        }

        protected void TurnOnEnemy(Enemy enemy)
        {
            enemy.gameObject.SetActive(true);
        }
        
        protected void TurnOffEnemy(Enemy enemy)
        {
            enemy.gameObject.SetActive(false);
        }
        
        protected Enemy(int health, int damage, float attackRange, float speed, float chaseSpeed, int chase, EnemyType enemyType)
        {
            Health = health;
            Damage = damage;
            AttackRange = attackRange;
            Speed = speed;
            ChaseSpeed = chaseSpeed;
            Chase = chase;
            EnemyType = enemyType;
        }
    }
}