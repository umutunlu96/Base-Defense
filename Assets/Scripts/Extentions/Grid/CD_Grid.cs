using UnityEngine;
using UnityEngine.Rendering;

namespace Extentions.Grid
{
    [CreateAssetMenu(fileName = "CD_Grid", menuName = "BaseDefense/CD_Grid", order = 0)]
    public class CD_Grid : ScriptableObject
    {
        public SerializedDictionary<GridType, GridData> GridDatas;
    }
}