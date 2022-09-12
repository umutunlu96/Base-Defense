using System;
using Data.UnityObject;
using Data.ValueObject;
using Signals;
using UnityEngine;

namespace Managers
{
    public class BaseManager : MonoBehaviour
    {
        #region Variables

        public LevelData Data;
        private int levelID;

        #endregion

        private void Awake()
        {
            Data = GetData();
        }

        private int GetLevelID => LevelSignals.Instance.onGetLevelID();

        private LevelData GetData()
        {
            return Resources.Load<CD_Level>("Data/CD_Level").Levels[GetLevelID - 1];
        }

        private void SetData()
        {
        }
    }
}