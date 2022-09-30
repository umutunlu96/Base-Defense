using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using Enums;
using Managers;
using Signals;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Controllers
{
    public class PlayerAimController : MonoBehaviour
    {
        public List<Transform> enemyTargetList = new List<Transform>();

        [SerializeField] private float bulletSpeed = 10;
        [SerializeField] private PlayerManager manager;
        [SerializeField] private RigBuilder rigBuilder;
        [SerializeField] private Transform targetTransform;
        [SerializeField] private Transform targetInitialTransform;
        
        [SerializeField] private List<GameObject> guns;
        [SerializeField] private List<Transform> muzzleTransform;
        [SerializeField] private PlayerWeaponType playerWeaponType;
        private float _elapsedTime;
        private void Start()
        {
            Shoot();
        }

        private void Update()
        {
            SetTarget();
        }

        public void UpdateEnemyList(Transform enemyTransform, bool isAdd)
        {
            if (!isAdd)
            {
                enemyTargetList.Remove(enemyTransform);
                enemyTargetList.TrimExcess();
                print("Removed Enemy List");
                return;
            }
            enemyTargetList.Add(enemyTransform);
            print("Added Enemy List");
        }
        
        private void SetTarget()
        {
            if (enemyTargetList.Count == 0)
            {
                targetTransform.localPosition = Vector3.Lerp(targetTransform.localPosition,
                    targetInitialTransform.localPosition, Mathf.SmoothStep(0, 1, Time.deltaTime * 4));
                return;
            }
            
            targetTransform.position = Vector3.Lerp(targetTransform.position,
                enemyTargetList[0].transform.position, Mathf.SmoothStep(0, 1, Time.deltaTime * 12));
        }
        
        public void EnableAimRig(bool isEnabled) => rigBuilder.layers[0].active = isEnabled;

        public void ChangeWeaponRigPos(PlayerWeaponType weaponType)
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

        private GameObject GetBullet(PlayerWeaponType weaponType) => 
            PoolSignals.Instance.onGetPoolObject?.Invoke($"{weaponType}Bullet", muzzleTransform[(int) weaponType]);

        private async void Shoot()
        {
            if (enemyTargetList.Count == 0)
            {
                await Task.Delay(25);
                Shoot();
                return;
            }
            
            while (enemyTargetList.Count > 0)
            {
                GameObject bullet = GetBullet(playerWeaponType);
                bullet.transform.position = muzzleTransform[(int) playerWeaponType].position;
                float distance = Vector3.Distance(muzzleTransform[(int) playerWeaponType].position,
                    enemyTargetList[0].position);

                bullet.transform.DOMove(Vector3.forward, distance / bulletSpeed).SetDelay(.15f).OnComplete(() =>
                {
                    PoolSignals.Instance.onReleasePoolObject?.Invoke($"{playerWeaponType}Bullet".ToString(), bullet);
                });
                await Task.Delay(25);
            }
        }
    }
}