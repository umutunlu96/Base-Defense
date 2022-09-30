using System;
using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] private PlayerManager manager;
        [SerializeField] private RigBuilder rigBuilder;
        [SerializeField] private Transform targetTransform;
        [SerializeField] private List<GameObject> guns;
        public List<Transform> enemyTargetList = new List<Transform>();
        
        private void Start()
        {
            StartCoroutine(Shoot());
        }

        public void UpdateEnemyList(Transform enemyTransform)
        {
            if (enemyTargetList.Contains(enemyTransform))
            {
                enemyTargetList.Remove(enemyTransform);
                enemyTargetList.TrimExcess();
                return;
            }
            
            if (!enemyTargetList.Contains(enemyTransform));
            enemyTargetList.Add(enemyTransform);
        }
        
        public void SetTarget()
        {
            if(enemyTargetList == null) return;
            targetTransform.position = enemyTargetList[0].transform.position;
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
        
        private IEnumerator Shoot()
        {
            while (enemyTargetList.Count > 0)
            {
                SetTarget();
                GameObject bullet = PoolSignals.Instance.onGetPoolObject?.Invoke("Bullet", transform);

                bullet.transform.DOMove(targetTransform.position, .25f);

                yield return new WaitForSeconds(.15f);
            }
        }
    }
}