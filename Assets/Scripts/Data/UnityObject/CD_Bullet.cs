using Data.ValueObject;
using Enums;
using UnityEngine;
using UnityEngine.Rendering;

namespace Data.UnityObject
{
    [CreateAssetMenu(fileName = "CD_Bullet", menuName = "BaseDefense/CD_Bullet", order = 0)]
    public class CD_Bullet : ScriptableObject
    {
        public SerializedDictionary<WeaponType, BulletData> BulletDatas;
    }
}