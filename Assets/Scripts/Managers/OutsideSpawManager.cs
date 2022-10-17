using System.Collections;
using System.Collections.Generic;
using Data.UnityObject;
using Data.ValueObject;
using Enums;
using Signals;
using Sirenix.OdinInspector;
using StateMachine;
using StateMachine.Enemy;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
    public class OutsideSpawManager : MonoBehaviour
    {
        #region Variables

        #region Public
        

        #endregion

        #region Serialized

        [SerializeField] private List<Transform> enemySpawnPoints;
        [SerializeField] private List<Transform> hostageSpawnPoints;
        [SerializeField] private List<Transform> bombSpawnPoints;
        
        #endregion

        #region Private

        //Data
        private SpawnData _spawnData;
        private List<EnemySpawnData> _enemySpawnDatas = new List<EnemySpawnData>();

        //Visualize Data
        [Header("Enemy")]
        [ShowInInspector] private int _enemySpawnedCount;
        [ShowInInspector] private int _enemySpawnLimit;
        private float _enemySpawnDelay;
        [Header("Hostage")]
        [ShowInInspector] private int _hostageSpawnedCount;
        [ShowInInspector] private int _hostageSpawnLimit;
        private List<Transform> _hostageSpawnPointsCache = new List<Transform>();
        private float _hostageSpawnDelay;
        
        [Header("Bomb")]
        [ShowInInspector] private int _bombSpawnedCount;
        [ShowInInspector] private int _bombSpawnLimit;
        private List<Transform> _bombSpawnPointsCache = new List<Transform>();
        private float _bombSpawnDelay;
        
        #endregion
        
        #endregion
        
        private int GetLevelData() => LevelSignals.Instance.onGetLevelCount();
        
        private SpawnData GetSpawnData() => Resources.Load<CD_Spawner>("Data/CD_Spawner").SpawnDatas[GetLevelData()-1];
        
        private void Start()
        {
            Init();
        }

        private void Init()
        {
            _spawnData = GetSpawnData();
            InitEnemy();
            InitHostage();
            InitBomb();
        }

        #region EventSubscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            AiSignals.Instance.onEnemyDead += OnEnemyDead;
            AiSignals.Instance.onHostageRescued += OnHostageRescued;
        }
        
        private void UnSubscribeEvents()
        {
            AiSignals.Instance.onEnemyDead -= OnEnemyDead;
            AiSignals.Instance.onHostageRescued -= OnHostageRescued;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        #endregion

        #region Event Functions

        private void OnEnemyDead(Transform enemyTransform)
        {
            if (enemyTransform.TryGetComponent(out EnemyAI enemyAI))
            {
                EnemyType enemyType = enemyAI.EnemyType;
                _enemySpawnDatas[(int)enemyType].CurrentSpawnAmount--;
                _enemySpawnedCount--;
            }
        }

        private void OnHostageRescued(Transform hostageTransform)
        {
            _hostageSpawnPointsCache.Remove(hostageTransform.parent.parent);
            _hostageSpawnedCount--;
        }

        #endregion
        
        #region Enemy Spawn

        private void InitEnemy()
        {
            _enemySpawnDelay = _spawnData.EnemySpawnDelay;
            
            for (int i = 0; i < _spawnData.EnemySpawnDatas.Count; i++)
            {
                _enemySpawnDatas.Add(_spawnData.EnemySpawnDatas[i]);
                _enemySpawnLimit += _enemySpawnDatas[i].MaxSpawnAmount;
                _enemySpawnDatas[i].CurrentSpawnAmount = 0;
            }
            
            StartCoroutine(EnemySpawnController());
        }
        
        private bool CheckIfEnemyCanSpawn(int enemyType)
        {
            return _enemySpawnDatas[enemyType].CurrentSpawnAmount < _enemySpawnDatas[enemyType].MaxSpawnAmount;
        }
        
        private void GetEnemy(string enemyName, Transform spawnPoint)
        {
            GameObject enemy = PoolSignals.Instance.onGetPoolObject?.Invoke(enemyName, spawnPoint);
            enemy.transform.SetParent(transform);
            enemy.transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        
        private void SpawnEnemy()
        {
            int randomEnemy = Random.Range(0, _enemySpawnDatas.Count);
            int randomSpawnPoint = Random.Range(0, enemySpawnPoints.Count);
            
            if (CheckIfEnemyCanSpawn(randomEnemy))
            {
                var enemyName = _enemySpawnDatas[randomEnemy].EnemyType.ToString();
                GetEnemy(enemyName, enemySpawnPoints[randomSpawnPoint]);
                _enemySpawnDatas[randomEnemy].CurrentSpawnAmount++;
                _enemySpawnedCount++;
            }
        }
        
        private IEnumerator EnemySpawnController()
        {
            while (_enemySpawnedCount <= _enemySpawnLimit)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(_enemySpawnDelay);
            }
            yield return new WaitForSeconds(_enemySpawnDelay);
        }
        
        #endregion

        #region Hostage Spawn

        private void InitHostage()
        {
            _hostageSpawnDelay = _spawnData.HostageSpawnDatas.SpawnDelay;
            _hostageSpawnLimit = hostageSpawnPoints.Count;
            _spawnData.HostageSpawnDatas.CurrentSpawnAmount = 0;

            StartCoroutine(HostageSpawnController());
        }
        
        private bool CheckIfHostageCanSpawn()
        {
            return _hostageSpawnedCount <= _hostageSpawnLimit;
        }
        
        private void GetHostage(string poolType, Transform spawnPoint)
        {
            Vector3 randomRotate = new Vector3(0, Random.Range(0, 359), 0);
            GameObject hostage = PoolSignals.Instance.onGetPoolObject?.Invoke(poolType, spawnPoint);
            if(hostage==null) return;
            hostage.transform.SetParent(spawnPoint);
            hostage.transform.rotation = Quaternion.Euler(randomRotate);
        }
        
        private void SpawnHostage()
        {
            foreach (var t in hostageSpawnPoints)
            {
                if (!_hostageSpawnPointsCache.Contains(t))
                {
                    // print("Hostage Spawned");
                    _hostageSpawnedCount++;
                    _hostageSpawnPointsCache.Add(t);
                    GetHostage("Hostage", t);
                    break;
                }
            }
        }

        private IEnumerator HostageSpawnController()
        {
            while (CheckIfHostageCanSpawn())
            {
                SpawnHostage();
                yield return new WaitForSeconds(_hostageSpawnDelay);
            }
        }
        #endregion
        
        #region Bomb Spawn

        private void InitBomb()
        {
            _bombSpawnDelay = _spawnData.BombSpawnData.BombSpawnDelay;
            _bombSpawnLimit = bombSpawnPoints.Count;
            _spawnData.BombSpawnData.CurrentSpawnAmount = 0;

            StartCoroutine(BombSpawnController());
        }
        
        private bool CheckIfBombCanSpawn()
        {
            return _bombSpawnedCount <= _bombSpawnLimit;
        }
        
        private void GetBomb(string poolType, Transform spawnPoint)
        {
            GameObject bomb = PoolSignals.Instance.onGetPoolObject?.Invoke(poolType, spawnPoint);
            if(bomb==null) return;
            bomb.transform.SetParent(spawnPoint);
        }
        
        private void SpawnBomb()
        {
            foreach (var t in bombSpawnPoints)
            {
                if (!_bombSpawnPointsCache.Contains(t))
                {
                    _bombSpawnedCount++;
                    _bombSpawnPointsCache.Add(t);
                    GetBomb("Bomb", t);
                    break;
                }
            }
        }

        private IEnumerator BombSpawnController()
        {
            while (CheckIfBombCanSpawn())
            {
                SpawnBomb();
                yield return new WaitForSeconds(_bombSpawnDelay);
            }
        }
        #endregion
    }
}