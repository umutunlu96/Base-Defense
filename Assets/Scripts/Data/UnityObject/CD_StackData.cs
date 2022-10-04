using System.Collections.Generic;
using Data.ValueObject;
using Enums;
using UnityEngine;
using UnityEngine.Rendering;

namespace Data.UnityObject
{
    [CreateAssetMenu(fileName = "CD_StackData", menuName = "BaseDefense/CD_StackData", order = 0)]
    public class CD_StackData : ScriptableObject
    {
        // public List<StackData> StackDatas = new List<StackData>();
        public SerializedDictionary<StackType, StackData> StackDatas;
    }
}