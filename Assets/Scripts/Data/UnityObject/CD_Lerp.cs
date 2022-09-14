using Data.ValueObject;
using UnityEngine;

namespace Data.UnityObject
{
    [CreateAssetMenu(fileName = "CD_Lerp", menuName = "BaseDefense/CD_Lerp", order = 0)]
    public class CD_Lerp : ScriptableObject
    {
        public LerpData Data = new LerpData();
    }
}