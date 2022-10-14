using UnityEngine;

namespace Abstract
{
    public interface IDamageable
    {
        void TakeDamage(float damage);

        Transform GetTransform();

        bool AmIDeath();
    }
}