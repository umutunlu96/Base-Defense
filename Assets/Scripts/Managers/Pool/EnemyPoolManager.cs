using System;
using System.Collections.Generic;
using Extentions;
using UnityEngine;

namespace Managers.Pool
{
    public class EnemyPoolManager : MonoBehaviour
    {
        public List<GameObject> RedEnemyList;
        public List<GameObject> OrangeEnemyList;
        public List<GameObject> BigRedEnemyList;

        [SerializeField] private GameObject redEnemyPrefab;
        [SerializeField] private GameObject orangeEnemyPrefab;
        [SerializeField] private GameObject bigRedEnemyPrefab;

        public static ObjectPool<GameObject> RedEnemyPool;
        public static ObjectPool<GameObject> OrangeEnemyPool;
        public static ObjectPool<GameObject> BigRedEnemyPool;

        public int RedEnemyCount;
        public int OrangeEnemyCount;
        public int BigRedEnemyCount;


        private void Awake()
        {
            InitPool();
        }

        private void InitPool()
        {
            RedEnemyPool = new ObjectPool<GameObject>(RedEnemyFactory, RedEnemyTurnOnCallback, RedEnemyTurnOffCallback,
                RedEnemyCount, true);
            OrangeEnemyPool = new ObjectPool<GameObject>(OrangeEnemyFactory, OrangeEnemyTurnOnCallback,
                OrangeEnemyTurnOffCallback, OrangeEnemyCount, true);
            BigRedEnemyPool = new ObjectPool<GameObject>(BigRedEnemyFactory, BigRedEnemyTurnOnCallback,
                BigRedEnemyTurnOffCallback, BigRedEnemyCount, true);
        }

        #region RedEnemyFactory
        
        private GameObject RedEnemyFactory()
        {
            GameObject redEnemyObject = Instantiate(redEnemyPrefab, transform);
            return redEnemyObject;
        }

        private void RedEnemyTurnOnCallback(GameObject enemy)
        {
            RedEnemyList.Remove(enemy);
            RedEnemyList.TrimExcess();
            enemy.SetActive(true);
        }
        
        private void RedEnemyTurnOffCallback(GameObject enemy)
        {
            RedEnemyList.Add(enemy);
            RedEnemyList.TrimExcess();
            enemy.SetActive(false);
        }
        
        #endregion

        #region OrangeEnemyFactory
        
        private GameObject OrangeEnemyFactory()
        {
            GameObject orangeEnemyObject = Instantiate(orangeEnemyPrefab, transform);
            return orangeEnemyObject;
        }

        private void OrangeEnemyTurnOnCallback(GameObject enemy)
        {
            OrangeEnemyList.Remove(enemy);
            OrangeEnemyList.TrimExcess();
            enemy.SetActive(true);
        }
        
        private void OrangeEnemyTurnOffCallback(GameObject enemy)
        {
            OrangeEnemyList.Add(enemy);
            OrangeEnemyList.TrimExcess();
            enemy.SetActive(false);
        }
        
        #endregion
        
        #region BigRedEnemyFactory
        
        private GameObject BigRedEnemyFactory()
        {
            GameObject bigRedEnemyObject = Instantiate(bigRedEnemyPrefab, transform);
            return bigRedEnemyObject;
        }

        private void BigRedEnemyTurnOnCallback(GameObject enemy)
        {
            BigRedEnemyList.Remove(enemy);
            BigRedEnemyList.TrimExcess();
            enemy.SetActive(true);
        }
        
        private void BigRedEnemyTurnOffCallback(GameObject enemy)
        {
            BigRedEnemyList.Add(enemy);
            BigRedEnemyList.TrimExcess();
            enemy.SetActive(false);
        }
        
        #endregion
    }
}