using Abstract;
using Data.UnityObject;
using Data.ValueObject.Base;
using DG.Tweening;
using Enums;
using Signals;
using StateMachine.MoneyWorkerAI;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class MoneyWorkerManager : MonoBehaviour, ISaveable
    {
        #region Variables

        #region Public
        
        public MoneyWorkerData Data;
        public int UniqueId;
        
        [Header("Variables")]
        public int Identifier = 0;
        public float delay = 0.005f;
        
        
        #endregion

        #region Serialized

        [Header("Referances")]
        
        [SerializeField] private MoneyWorkerAI moneyWorkerAI;
        
        [SerializeField] private TextMeshPro payedAmountText;
        
        [SerializeField] private GameObject buyPart;
        
        [SerializeField] private Renderer filledSquareRenderer;
        
        //Controllers
        
        #endregion

        #region Private

        private int _levelID;

        #endregion

        #endregion
        
        private int GetLevelID => LevelSignals.Instance.onGetLevelCount();
        
        private MoneyWorkerData GetMoneyWorkerData() => Resources.Load<CD_Level>("Data/CD_Level").Levels[GetLevelID-1].
            BaseData.MoneyWorkerData;
        
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
            
            if (!ES3.FileExists($"MoneyWorkerData{UniqueId}.es3"))
            {
                if (!ES3.KeyExists("MoneyWorkerData"))
                {
                    Data = GetMoneyWorkerData();
                    Save(UniqueId);
                }
            }
            Load(UniqueId);
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
        
        #region EventDriven Functions
        
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
            Data = new MoneyWorkerData(Data.BuyState,Data.MoneyWorkerCost,Data.MoneyWorkerPayedAmount,Data.MoneyWorkerLevel);
            
            SaveLoadSignals.Instance.onSaveMoneyWorkerData?.Invoke(Data, uniqueId);
        }
        
        public void Load(int uniqueId)
        {
            MoneyWorkerData data = SaveLoadSignals.Instance.onLoadMoneyWorkerData?.Invoke(Data.Key, uniqueId);
            
            Data.BuyState = data.BuyState;
            Data.MoneyWorkerCost = data.MoneyWorkerCost;
            Data.MoneyWorkerPayedAmount = data.MoneyWorkerPayedAmount;
            Data.MoneyWorkerLevel = data.MoneyWorkerLevel;
        }

        #endregion
        
        #region Controller

        public void UpdatePayedAmountText() => payedAmountText.text = (Data.MoneyWorkerCost - Data.MoneyWorkerPayedAmount).ToString();

        public void CheckPayedAmount()
        {
            if (Data.MoneyWorkerPayedAmount >= Data.MoneyWorkerCost)
            {
                Data.BuyState = BuyState.Bought;
                CheckBougthState(Data.BuyState);
            }
        }

        private void CheckBougthState(BuyState buyState)
        {
            if(buyState == BuyState.NotBought) return;
            moneyWorkerAI.IsBougth = true;
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
                float filletAmount = 360 - (Data.MoneyWorkerPayedAmount * 360 / Data.MoneyWorkerCost);
                filledSquareRenderer.material.DOFloat(filletAmount,"_Arc2",delay);
            }
        }
        #endregion
    }
}