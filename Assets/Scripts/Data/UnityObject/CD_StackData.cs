using System.Collections.Generic;
using Data.ValueObject;
using UnityEngine;

namespace Data.UnityObject
{
    [CreateAssetMenu(fileName = "CD_StackData", menuName = "BaseDefense/CD_StackData", order = 0)]
    public class CD_StackData : ScriptableObject
    {
        public List<StackData> StackDatas = new List<StackData>();
    }
}