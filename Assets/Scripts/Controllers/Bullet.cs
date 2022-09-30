using Enums;
using UnityEngine;

namespace Controllers
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private PlayerWeaponType WeaponType;
        private float AutoDestroyTime;
    }
}