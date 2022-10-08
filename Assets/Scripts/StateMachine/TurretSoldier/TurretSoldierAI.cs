using System.Collections.Generic;
using Controllers;
using Enums;
using Keys;
using Managers;
using Signals;
using Sirenix.OdinInspector;
using UnityEngine;

namespace StateMachine.TurretSoldier
{
    public class TurretSoldierAI : MonoBehaviour
    {
        public bool IsPlayerUsingTurret = false;
        [SerializeField] private WeaponType weaponType;
        [SerializeField] private TurretManager manager;
        [SerializeField] private Transform turret;
        [SerializeField] private Transform turretMuzzle;
        [ShowInInspector] private List<Transform> enemies = new List<Transform>();
        [ShowInInspector] private bool _canShoot;
        [ShowInInspector] private int _ammo;
        private InputParams _inputParams;
        private float _timer;
        
        #region EventSubscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            AiSignals.Instance.onEnemyDead += OnEnemyDead;
            InputSignals.Instance.onInputDragged += OnInputDragged;
        }
        
        private void UnSubscribeEvents()
        {
            AiSignals.Instance.onEnemyDead -= OnEnemyDead;
            InputSignals.Instance.onInputDragged -= OnInputDragged;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        #endregion
        
        #region Event Functions

        private void OnEnemyDead(Transform enemy) => enemies.Remove(enemy);

        private void OnInputDragged(InputParams inputParams) => _inputParams = inputParams;
        
        #endregion
        
        private void Update()
        {
            if (!IsPlayerUsingTurret)
            {
                if (_canShoot && _ammo > 0 && enemies.Count > 0)
                {
                    float singleStep = Time.deltaTime * 2;
                    Vector3 targetDirection = enemies[0].position - turret.position;
                    targetDirection.y = 0;
                    Vector3 newDirection = Vector3.RotateTowards(turret.forward, targetDirection, singleStep, 0f);
                    turret.rotation = Quaternion.LookRotation(newDirection);
                
                    _timer += Time.deltaTime;
                    if (_timer > .5f)
                    {
                        Shoot();
                        _timer = 0;
                    }
                }
                else
                {
                    turret.rotation = Quaternion.Slerp(turret.transform.rotation, Quaternion.Euler(0,0,0), Time.deltaTime * 2);
                }
            }
            else
            {
                if (_ammo > 0)
                {
                    
                }
            }
        }

        public void UpdateAmmo(int ammo)
        {
            _ammo += ammo * 4;
        }

        private GameObject GetBullet() { return PoolSignals.Instance.onGetPoolObject?.Invoke($"{weaponType}Bullet", turretMuzzle); }
        
        private void Shoot()
        {
            if (enemies.Count == 0) return;
            GameObject bullet = GetBullet();
            bullet.GetComponent<Bullet>().Shoot(turretMuzzle.rotation);
            _ammo--;
            if(_ammo <= 2)
                manager.GetAmmo(turret);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            enemies.Add(other.transform);
            _canShoot = true;
        }
        
        private void OnTriggerExit(Collider other)
        {
            enemies.Remove(other.transform);
            enemies.TrimExcess();
            if (enemies.Count > 0) _canShoot = true;
            else _canShoot = false;
        }
    }
}