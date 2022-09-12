using System;
using Abstract;
using Data.UnityObject;
using Data.ValueObject.Base;
using DG.Tweening;
using Enums;
using Signals;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class AmmoWorkerManager : MonoBehaviour, ISaveable
    {
        #region Variables

        #region Public
        public AmmoWorkerData Data;
        
        [Header("Variables")]
        [SerializeField] private int Identifier = 0;
        [SerializeField] private float delay = 0.005f;
        
        #endregion

        #region Serialized

        [Header("Referances")]
        [SerializeField] private TextMeshPro payedAmountText;

        [SerializeField] private GameObject buyPart;

        [SerializeField] private Renderer filledSquareRenderer;
        
        //Controllers
        
        #endregion

        #region Private

        private int _levelID;
        private int _uniqueId;
        private bool _canBuy;
        private float _timer;

        #endregion

        #endregion
        
        private int GetLevelID => LevelSignals.Instance.onGetLevelID();
        
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

            _uniqueId = _levelID * 10 + Identifier;
            
            if (!ES3.FileExists($"AmmoWorkerData{_uniqueId}.es3"))
            {
                if (!ES3.KeyExists("AmmoWorkerData"))
                {
                    Data = GetAmmoWorkerData();
                    Save(_uniqueId);
                }
            }
            Load(_uniqueId);
            // SetDataToControllers();
        }

        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            InputSignals.Instance.onInputReleased += EnableCanBuyState;
            InputSignals.Instance.onInputTaken += DisableCanBuyState;
        }
        
        private void UnSubscribeEvents()
        {
            InputSignals.Instance.onInputReleased -= EnableCanBuyState;
            InputSignals.Instance.onInputTaken -= DisableCanBuyState;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        #endregion

        #region EventDriven Functions
        
        private void EnableCanBuyState() => _canBuy = true;
        private void DisableCanBuyState() => _canBuy = false;
        
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

        private void UpdatePayedAmountText() => payedAmountText.text = (Data.AmmoWorkerCost - Data.AmmoWorkerPayedAmount).ToString();

        private void CheckPayedAmount()
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
            buyPart.SetActive(false);
        }
        
        private void SetRadialFilletAmount(bool isInitialize)
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
        
        #region Physic Check

        private void OnTriggerStay(Collider other)
        {
            
            if (other.CompareTag("Player"))
            {
                if (!_canBuy) return;
                if (ScoreSignals.Instance.onGetMoneyAmount() > Data.AmmoWorkerCost) return;
                
                _timer -= Time.deltaTime;

                if (!(_timer <= 0)) return;
                if (Data.AmmoWorkerPayedAmount < Data.AmmoWorkerCost)
                {
                    ScoreSignals.Instance.onSetMoneyAmount(-1);
                    Data.AmmoWorkerPayedAmount++;
                    UpdatePayedAmountText();
                    CheckPayedAmount();
                    SetRadialFilletAmount(false);
                }
                else
                {
                    Data.BuyState = BuyState.Bought;
                }
                _timer = delay;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Save(_uniqueId);
            }
        }

        #endregion
    }
}