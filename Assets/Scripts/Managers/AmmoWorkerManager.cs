using System;
using Abstract;
using Data.UnityObject;
using Data.ValueObject.Base;
using DG.Tweening;
using Enums;
using Signals;
using StateMachine.AmmoWorker;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class AmmoWorkerManager : MonoBehaviour, ISaveable
    {
        #region Variables

        #region Public
        public AmmoWorkerData Data;
        public int UniqueId;
        
        [Header("Variables")]
        [SerializeField] private int Identifier = 0;
        [SerializeField] private float delay = 0.005f;
        
        #endregion

        #region Serialized

        [Header("Referances")]
        [SerializeField] private TextMeshPro payedAmountText;

        [SerializeField] private GameObject buyPart;

        [SerializeField] private Renderer filledSquareRenderer;

        [SerializeField] private AmmoWorkerAI ammoWorkerAI;
        
        //Controllers
        
        #endregion

        #region Private

        private int _levelID;

        #endregion

        #endregion
        
        private int GetLevelID => LevelSignals.Instance.onGetLevelCount();
        
        private AmmoWorkerData GetAmmoWorkerData() => Resources.Load<CD_Level>("Data/CD_Level").Levels[GetLevelID-1].
            BaseData.AmmoWorkerData;
        
        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            SetData();
            SetRadialFilletAmount(true);
            UpdatePayedAmountText();
            CheckPayedAmount();
            CheckBougthState(Data.BuyState);
        }
        
        private void SetData()
        {
            _levelID = GetLevelID;

            UniqueId = _levelID * 10 + Identifier;
            
            if (!ES3.FileExists($"AmmoWorkerData{UniqueId}.es3"))
            {
                if (!ES3.KeyExists("AmmoWorkerData"))
                {
                    Data = GetAmmoWorkerData();
                    Save(UniqueId);
                }
            }
            Load(UniqueId);
            // SetDataToControllers();
        }

        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {

        }
        
        private void UnSubscribeEvents()
        {

        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        #endregion
        
        #region Save-Load

        private void OnSave()
        {
            Save(_levelID);
        }

        private void OnLoad()
        {
            Load(_levelID);
        }

        public void Save(int uniqueId)
        {
            Data = new AmmoWorkerData(Data.BuyState,Data.AmmoWorkerCost,Data.AmmoWorkerPayedAmount,Data.AmmoWorkerLevel);
            
            SaveLoadSignals.Instance.onSaveAmmoWorkerData?.Invoke(Data, uniqueId);
        }
        
        public void Load(int uniqueId)
        {
            AmmoWorkerData data = SaveLoadSignals.Instance.onLoadAmmoWorkerData?.Invoke(Data.Key, uniqueId);
            
            Data.BuyState = data.BuyState;
            Data.AmmoWorkerCost = data.AmmoWorkerCost;
            Data.AmmoWorkerLevel = data.AmmoWorkerLevel;
            Data.AmmoWorkerPayedAmount = data.AmmoWorkerPayedAmount;
        }

        #endregion

        #region Controller

        public void UpdatePayedAmountText() => payedAmountText.text = (Data.AmmoWorkerCost - Data.AmmoWorkerPayedAmount).ToString();

        public void CheckPayedAmount()
        {
            if (Data.AmmoWorkerPayedAmount >= Data.AmmoWorkerCost)
            {
                Data.BuyState = BuyState.Bought;
                CheckBougthState(Data.BuyState);
            }
        }

        private void CheckBougthState(BuyState buyState)
        {
            if(buyState == BuyState.NotBought) return;
            ammoWorkerAI.IsBougth = true;
            buyPart.SetActive(false);
        }

        public void SetRadialFilletAmount(bool isInitialize)
        {
            if (isInitialize)
            {
                filledSquareRenderer.material.SetFloat("_Arc2", 360);
            }
            else
            {
                float filletAmount = 360 - (Data.AmmoWorkerPayedAmount * 360 / Data.AmmoWorkerCost);
                filledSquareRenderer.material.DOFloat(filletAmount,"_Arc2",delay);
            }
        }
        #endregion
    }
}