using System;
using Abstract;
using Data.UnityObject;
using Data.ValueObject;
using Enums;
using Signals;
using UnityEngine;

namespace Controllers
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private PlayerWeaponType WeaponType;
        private BulletData _bulletData;
        private Rigidbody _rigidBody;
        private const string DisableMethodName = "Disable";
        private float _autoDestroyTime;
        private float _spawnDelay;
        private float _moveSpeed;
        private int _damage;
        private void Awake()
        {
            Initialization();
        }

        private void Initialization()
        {
            _bulletData = GetData();
            _autoDestroyTime = _bulletData.AutoDestroyTime;
            _spawnDelay = _bulletData.SpawnDelay;
            _moveSpeed = _bulletData.MoveSpeed;
            _damage = _bulletData.Damage;

            _rigidBody = GetComponent<Rigidbody>();
        }
        
        private BulletData GetData() => Resources.Load<CD_Bullet>("Data/CD_Bullet").BulletDatas[WeaponType];
        
        private void OnEnable()
        {
            CancelInvoke(DisableMethodName);
            Invoke(DisableMethodName, _autoDestroyTime);
        }

        public void Shoot(Quaternion muzzleRotation)
        {
            transform.rotation = muzzleRotation;
            _rigidBody.AddForce(transform.forward * _moveSpeed, ForceMode.VelocityChange);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            IDamageable damageable;
            if (other.TryGetComponent<IDamageable>(out damageable))
            {
                damageable.TakeDamage(_damage);
            }

            Disable();
        }

        private void Disable()
        {
            CancelInvoke(DisableMethodName);
            _rigidBody.velocity = Vector3.zero;
            PoolSignals.Instance.onReleasePoolObject?.Invoke($"{WeaponType}Bullet", this.gameObject);
        }
    }
}