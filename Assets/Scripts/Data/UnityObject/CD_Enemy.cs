using Data.ValueObject;
using Data.ValueObject.AI;
using Enums;
using UnityEngine;
using UnityEngine.Rendering;

namespace Data.UnityObject
{
    [CreateAssetMenu(fileName = "CD_Enemy", menuName = "BaseDefense/CD_Enemy", order = 0)]
    public class CD_Enemy : ScriptableObject
    {
        public SerializedDictionary<EnemyType, EnemyData> EnemyDatas;
    }
}