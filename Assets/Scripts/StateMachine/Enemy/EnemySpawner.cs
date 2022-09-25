using System.Collections;
using System.Collections.Generic;
using Data.UnityObject;
using Data.ValueObject;
using Enums;
using Signals;
using Sirenix.OdinInspector;
using UnityEngine;

namespace StateMachine.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        #region Variables

        #region Public
        

        #endregion

        #region Serialized

        [SerializeField] private List<Transform> spawnPoints;
        
        #endregion

        #region Private

        [ShowInInspector] private SpawnData _spawnData;
        private List<EnemySpawnData> _enemySpawnDatas = new List<EnemySpawnData>();
        private List<EnemySpawnData> _enemySpawnDataCache = new List<EnemySpawnData>();
        private float _spawnDelay;

        [ShowInInspector] private int _spawnedEnemyCount;
        [ShowInInspector] private int _spawnLimit;
        
        #endregion
        
        #endregion
        
        private int GetLevelData() => LevelSignals.Instance.onGetLevelID();
        
        private SpawnData GetSpawnData() => Resources.Load<CD_Spawner>("Data/CD_Spawner").SpawnDatas[GetLevelData()-1];

        private void Start()
        {
            _spawnData = GetSpawnData();
            _spawnDelay = _spawnData.EnemySpawnDelay;
            
            for (int i = 0; i < _spawnData.EnemySpawnDatas.Count; i++)
            {
                _enemySpawnDatas.Add(GetSpawnData().EnemySpawnDatas[i]);
                _enemySpawnDatas[i].CurrentSpawnAmount = 0;
            }

            _enemySpawnDataCache = _enemySpawnDatas;

            for (int i = 0; i < _enemySpawnDatas.Count; i++)
            {
                _spawnLimit += _enemySpawnDatas[i].MaxSpawnAmount;
            }

            StartCoroutine(SpawnController());
        }

        private bool CheckIfEnemyCanSpawn(int enemyType)
        {
            return _enemySpawnDataCache[(int) enemyType].CurrentSpawnAmount < _enemySpawnDataCache[(int) enemyType].MaxSpawnAmount;
        }
        
        private void GetEnemy(string enemyName, Transform spawnPoint)
        {
            PoolSignals.Instance.onGetPoolObjectWithString?.Invoke(enemyName, spawnPoint);
        }

        private void SpawnEnemy()
        {
            string enemyName = "";
            int randomEnemy = Random.Range(0, _enemySpawnDataCache.Count);
            int randomSpawnPoint = Random.Range(0, spawnPoints.Count);
            
            if (CheckIfEnemyCanSpawn(randomEnemy))
            {
                enemyName = _enemySpawnDatas[randomEnemy].EnemyType.ToString();
                GetEnemy(enemyName, spawnPoints[randomSpawnPoint]);
                _enemySpawnDataCache[randomEnemy].CurrentSpawnAmount++;
                _spawnedEnemyCount++;
            }
        }
        
        private IEnumerator SpawnController()
        {
            while (_spawnedEnemyCount <= _spawnLimit)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(_spawnDelay);
            }
            yield return new WaitForSeconds(_spawnDelay);
        }
    }
}