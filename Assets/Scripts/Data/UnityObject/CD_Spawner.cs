using System.Collections.Generic;
using Data.ValueObject;
using UnityEngine;

namespace Data.UnityObject
{
    [CreateAssetMenu(fileName = "CD_EnemySpawner", menuName = "BaseDefense/CD_EnemySpawner", order = 0)]
    public class CD_Spawner : ScriptableObject
    {
        public List<SpawnData> SpawnDatas;
    }
}