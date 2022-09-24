using System;
using Data.ValueObject.Base;
using Keys;
using Signals;
using UnityEngine;

namespace Managers
{
    public class ScoreManager : MonoBehaviour
    {
        #region Variables

        #region Serialized

        [SerializeField] private int _money;
        [SerializeField] private int _diamond;

        #endregion

        #region Private

        private ScoreParams _scoreParams;
        private int _levelID;
        private int _uniqueId;

        #endregion

        #endregion

        private int GetLevelID => LevelSignals.Instance.onGetLevelID();
        
        private int OnGetMoney() => _money;
        private int OnGetDiamond() => _diamond;
        private void OnSetMoney(int value)
        {
            _money += value;
            UISignals.Instance.onUpdateScore?.Invoke();
        }

        private void OnSetDiamond(int value)
        {
            _diamond += value;
            UISignals.Instance.onUpdateScore?.Invoke();
        }
        
        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            SetData();
        }
        
        private void SetData()
        {
            _levelID = GetLevelID;

            _uniqueId = _levelID * 10 /* + Identifier*/;
            
            if (!ES3.FileExists($"ScoreParams{_uniqueId}.es3"))
            {
                if (!ES3.KeyExists("ScoreParams"))
                {
                    _scoreParams = new ScoreParams(){Money = 0,Diamond = 0};
                    Save(_uniqueId);
                }
            }
            Load(_uniqueId);
            
            UISignals.Instance.onUpdateScore?.Invoke();
        }


        #region EventSubscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            ScoreSignals.Instance.onGetMoneyAmount += OnGetMoney;
            ScoreSignals.Instance.onGetDiamondAmount += OnGetDiamond;

            ScoreSignals.Instance.onSetMoneyAmount += OnSetMoney;
            ScoreSignals.Instance.onSetDiamondAmount += OnSetDiamond;

            SaveLoadSignals.Instance.onSaveScoreParams += OnSave;
            SaveLoadSignals.Instance.onLoadScoreParams += OnLoad;
        }
        
        private void UnSubscribeEvents()
        {
            ScoreSignals.Instance.onGetMoneyAmount -= OnGetMoney;
            ScoreSignals.Instance.onGetDiamondAmount -= OnGetDiamond;
            
            ScoreSignals.Instance.onSetMoneyAmount -= OnSetMoney;
            ScoreSignals.Instance.onSetDiamondAmount -= OnSetDiamond;
            
            SaveLoadSignals.Instance.onSaveScoreParams -= OnSave;
            SaveLoadSignals.Instance.onLoadScoreParams -= OnLoad;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        #endregion
        
        #region Save Load

        private void OnSave(ScoreParams scoreParams, int uniqueID)
        {
            _scoreParams = scoreParams;
            Save(uniqueID);
        }
        
        private ScoreParams OnLoad(string Key, int uniqueID)
        {
            Load(uniqueID);
            return _scoreParams;
        }
        
        public void Save(int uniqueId)
        {
            _scoreParams = new ScoreParams() {Money = _money, Diamond = _diamond};
            
            ES3.Save("ScoreParams",_scoreParams,$"ScoreParams{uniqueId}.es3");
        }
        
        public void Load(int uniqueId)
        {
            _scoreParams = ES3.Load<ScoreParams>("ScoreParams", $"ScoreParams{uniqueId}.es3");
            _money = _scoreParams.Money;
            _diamond = _scoreParams.Diamond;
        }

        #endregion
    }
}