using UnityEngine;

namespace Abstract
{
    public interface IDamageable
    {
        void TakeDamage(int damage);

        Transform GetTransform();

        bool AmIDeath();
    }
}