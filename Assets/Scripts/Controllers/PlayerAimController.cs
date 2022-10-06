﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abstract;
using DG.Tweening;
using Enums;
using Managers;
using Signals;
using StateMachine;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Controllers
{
    public class PlayerAimController : MonoBehaviour
    {
        public float AttackDelay = 0.2f;

        public List<IDamageable> Damageables = new List<IDamageable>();
        private Coroutine AttackCoroutine;

        [SerializeField] private PlayerManager manager;
        [SerializeField] private RigBuilder rigBuilder;
        [SerializeField] private Transform targetTransform;
        [SerializeField] private Transform targetInitialTransform;
        
        [SerializeField] private List<GameObject> guns;
        [SerializeField] private List<Transform> muzzleTransform;
        [SerializeField] private WeaponType weaponType;

        private IDamageable _closestDamageable;
        
        private void Update()
        {
            SetTarget();
        }
        
        #region EventSubscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            AiSignals.Instance.onEnemyAIDead += OnEnemyAIDead;
        }
        
        private void UnSubscribeEvents()
        {
            AiSignals.Instance.onEnemyAIDead -= OnEnemyAIDead;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        #endregion

        private void OnEnemyAIDead(IDamageable damageable)
        {
            Damageables.Remove(damageable);
            Damageables.TrimExcess();
            _closestDamageable = null;
        }

        private void SetTarget()
        {
            if (_closestDamageable == null)
            {
                targetTransform.localPosition = Vector3.Lerp(targetTransform.localPosition,
                    targetInitialTransform.localPosition, Mathf.SmoothStep(0, 1, Time.deltaTime * 12));
                return;
            }
            
            if(_closestDamageable == null) return;
            if (_closestDamageable.AmIDeath()) return;
            targetTransform.position = Vector3.Lerp(targetTransform.position,
                _closestDamageable.GetTransform().position, Mathf.SmoothStep(0, 1, Time.deltaTime * 24));
            //Damageables[0] => _closestDamageable
            //RotatePlayerSlowly(Damageables[0].GetTransform());
        }

        private void RotatePlayerSlowly(Transform target)
        {
            Vector3 rotateTo = (target.position - transform.position).normalized;
            manager.transform.Rotate(rotateTo * Time.deltaTime);
        }
        
        public void EnableAimRig(bool isEnabled) => rigBuilder.layers[0].active = isEnabled;

        public void ChangeWeaponRigPos(WeaponType weaponType)
        {
            for (int i = 1; i < rigBuilder.layers.Count; i++)
            {
                rigBuilder.layers[i].active = false;
                guns[i-1].SetActive(false);
            }
            
            int weaponTypeIndex = (int)weaponType;
            guns[weaponTypeIndex].SetActive(true);
            rigBuilder.layers[weaponTypeIndex + 1].active = true;
        }

        private GameObject GetBullet()
        {
            Transform muzzle = muzzleTransform[(int) weaponType];
            return PoolSignals.Instance.onGetPoolObject?.Invoke($"{weaponType}Bullet", muzzle);
        }


        #region Trigger

        private void OnTriggerEnter(Collider other)
        {
            IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable != null)
            {
                Damageables.Add(damageable);
                if (AttackCoroutine == null)
                {
                    AttackCoroutine = StartCoroutine(Attack());
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable != null)
            {
                Damageables.Remove(damageable);
                if (Damageables.Count == 0 && AttackCoroutine != null)
                {
                    StopCoroutine(AttackCoroutine);
                    AttackCoroutine = null;
                }
            }
        }
        #endregion

        private IDamageable GetClosestDamageable()
        {
            
            float closestDistance = float.MaxValue;

            IDamageable closestDamageable = null;
            
            for (int i = 0; i < Damageables.Count; i++)
            {
                Transform damageableTransform = Damageables[i].GetTransform();
                float distance = Vector3.Distance(transform.position, damageableTransform.position);

                if (distance < closestDistance)
                {
                    closestDamageable = Damageables[i];
                }
            }
            
            return closestDamageable;
        }
        
        
        private IEnumerator Attack()
        {
            WaitForSeconds Wait = new WaitForSeconds(AttackDelay);

            yield return Wait;
            
            while (Damageables.Count > 0)
            {
                _closestDamageable = GetClosestDamageable();
                
                if (_closestDamageable != null && !_closestDamageable.AmIDeath())
                {
                    GameObject bullet = GetBullet();
                    if (bullet != null)
                    {
                        bullet.GetComponent<Bullet>().Shoot(muzzleTransform[(int) weaponType].rotation);
                    }
                }

                // _closestDamageable = null;

                yield return Wait;

                Damageables.RemoveAll(DisabledDamageables);
            }

            AttackCoroutine = null;
        }

        protected  bool DisabledDamageables(IDamageable Damageable)
        {
            return Damageable != null && !Damageable.GetTransform().gameObject.activeSelf;
        }
    }
}