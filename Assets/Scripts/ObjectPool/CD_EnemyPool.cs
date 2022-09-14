using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace ObjectPool
{
    [CreateAssetMenu(fileName = "CD_EnemyPool", menuName = "BaseDefense/CD_EnemyPool", order = 0)]
    public class CD_EnemyPool : ScriptableObject
    {
        public Dictionary<EnemyType, EnemyPoolData> PoolList = new Dictionary<EnemyType, EnemyPoolData>();
    }
}