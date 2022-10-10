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
        public bool HasSoldier = false;
        [SerializeField] private WeaponType weaponType;
        [SerializeField] private TurretManager manager;
        [SerializeField] private Transform turret;
        [SerializeField] private Transform turretMuzzle;
        [ShowInInspector] private List<Transform> enemies = new List<Transform>();
        [ShowInInspector] private bool _canShoot;
        [ShowInInspector] private int _ammo;
        private InputParams _inputParams;
        private float _timer;
        private bool _canRotateByPlayer;
        
        #region EventSubscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            AiSignals.Instance.onEnemyDead += OnEnemyDead;
            InputSignals.Instance.onInputDragged += OnInputDragged;
            InputSignals.Instance.onInputTaken += ActivateRotation;
            InputSignals.Instance.onInputReleased += DeactivateRotation;
        }
        
        private void UnSubscribeEvents()
        {
            AiSignals.Instance.onEnemyDead -= OnEnemyDead;
            InputSignals.Instance.onInputDragged -= OnInputDragged;
            InputSignals.Instance.onInputTaken -= ActivateRotation;
            InputSignals.Instance.onInputReleased -= DeactivateRotation;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        #endregion
        
        #region Event Functions

        private void ActivateRotation() => _canRotateByPlayer = true;
        
        private void DeactivateRotation() => _canRotateByPlayer = false; 
        
        private void OnEnemyDead(Transform enemy) => enemies.Remove(enemy);

        private void OnInputDragged(InputParams inputParams) => _inputParams = inputParams;
        
        #endregion
        
        private void Update()
        {
            if (!IsPlayerUsingTurret)
            {
                SoldierAiUsingTurret();
            }
            else
            {
                PlayerUsingTurret();
            }
        }

        private void SoldierAiUsingTurret()
        {
            if(!HasSoldier) return;
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
                    if (enemies.Count == 0) return;
                    Shoot();
                    _timer = 0;
                }
            }
            else
            {
                turret.rotation = Quaternion.Slerp(turret.transform.rotation, Quaternion.Euler(0,0,0), Time.deltaTime * 2);
            }
        }
        
        private void PlayerUsingTurret()
        {
            _timer += Time.deltaTime;
                
            if (_timer > .5f && _ammo > 0)
            {
                Shoot();
                _timer = 0;
            }
                
            if(!_canRotateByPlayer) return;
                
            if (_inputParams.movementVector.z <= -0.9f)
            {
                IsPlayerUsingTurret = false;
                PlayerSignals.Instance.onPlayerLeaveTurretArea?.Invoke();
            }

            if (_inputParams.movementVector.x < 0.05f)
            {
                turret.rotation = Quaternion.Slerp(turret.rotation,Quaternion.Euler(0,-60,0), Time.deltaTime);
            }
            else if (_inputParams.movementVector.x > 0.05f)
            {
                turret.rotation = Quaternion.Slerp(turret.rotation,Quaternion.Euler(0,60,0), Time.deltaTime);
            }
        }
        
        public void UpdateAmmo(int ammo) => _ammo += ammo * 4;

        private GameObject GetBullet() { return PoolSignals.Instance.onGetPoolObject?.Invoke($"{weaponType}Bullet", turretMuzzle); }
        
        private void Shoot()
        {
            GameObject bullet = GetBullet();
            bullet.GetComponent<Bullet>().Shoot(turretMuzzle.rotation);
            _ammo--;
            if (_ammo == 0)
                manager.LoadAmmo(turret);
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