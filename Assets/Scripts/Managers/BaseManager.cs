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

        public BaseData BaseData;
        
        #endregion

        #region Serialized

        [SerializeField] private List<RoomManager> RoomManagers;

        #endregion

        #region Private

        private int _levelID;
        private int _uniqueId;

        #endregion

        #endregion
        
        
        private int GetLevelID => LevelSignals.Instance.onGetLevelID();
        
        private void GetBaseData() => BaseData =  Resources.Load<CD_Level>("Data/CD_Level").Levels[GetLevelID-1].
            BaseData;

        private void Start()
        {
            GetBaseData();
            SetDataToManagers();
        }

        private void SetDataToManagers()
        {
            for (int i = 0; i < RoomManagers.Count; i++)
            {
                RoomManagers[i].SetData(BaseData.BaseRoomData.RoomDatas[i], i);
            }
        }
        
    }
}