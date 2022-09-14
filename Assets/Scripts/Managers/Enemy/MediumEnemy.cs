using Enums;

namespace Managers.Enemy
{
    public class MediumEnemy : Abstract.Enemy
    {

        public MediumEnemy(int health, int damage, float attackRange, float speed, float chaseSpeed, int chase, EnemyType enemyType) : base(health, damage, attackRange, speed, chaseSpeed, chase, enemyType)
        {
        }
    }
}
