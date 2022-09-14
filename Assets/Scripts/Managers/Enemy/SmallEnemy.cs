using Enums;

namespace Managers.Enemy
{
    public class SmallEnemy : Abstract.Enemy
    {
        public SmallEnemy(int health, int damage, float attackRange, float speed, float chaseSpeed, int chase, EnemyType enemyType) : base(health, damage, attackRange, speed, chaseSpeed, chase, enemyType)
        {
        }
    }
}