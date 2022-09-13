using Data.ValueObject.Base;
using UnityEngine;

namespace Managers
{
    public class TurretManager : MonoBehaviour
    {
        #region Variables
        
        public int Identifier = 0;
        public TurretData TurretData;

        #endregion
        
        #region Private

        private int _levelID;
        private int _uniqueId;

        #endregion
    }
}