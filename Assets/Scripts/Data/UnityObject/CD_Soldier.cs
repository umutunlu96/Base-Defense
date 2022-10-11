using Data.ValueObject.AI;
using UnityEngine;

namespace Data.UnityObject
{
    [CreateAssetMenu(fileName = "CD_Soldier", menuName = "BaseDefense/CD_Soldier", order = 0)]
    public class CD_Soldier : ScriptableObject
    {
        public SoldierData SoldierData;
    }
}