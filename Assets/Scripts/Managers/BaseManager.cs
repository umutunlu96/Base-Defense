using System;
using System.Collections.Generic;
using Data.UnityObject;
using Data.ValueObject;
using Data.ValueObject.Base;
using Signals;
using UnityEngine;

namespace Managers
{
    public class BaseManager : MonoBehaviour
    {
        #region Variables

        #region Public
        
        
        #endregion

        #region Serialized

        [SerializeField] private List<RoomManager> RoomManagers;

        #endregion

        #region Private

        private int _levelID;
        private int _uniqueId;

        #endregion

        #endregion
    }
}