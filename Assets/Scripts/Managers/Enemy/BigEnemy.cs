using Enums;

namespace Managers.Enemy
{
    public class BigEnemy : Abstract.Enemy
    {
        public BigEnemy(int health, int damage, float attackRange, float speed, float chaseSpeed, int chase, EnemyType enemyType) : base(health, damage, attackRange, speed, chaseSpeed, chase, enemyType)
        {
        }
    }
}