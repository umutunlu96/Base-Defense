using System;
using Enums;
using Signals;
using UnityEngine;

namespace Abstract
{
    public abstract class ShopableMarket : MonoBehaviour
    {
        [SerializeField] protected ShopPanelType ShopPanelType;

        public virtual int GetLevelId() => LevelSignals.Instance.onGetLevelID();

        protected void Awake()
        {
            GetData();
        }

        protected abstract void GetData();

        protected abstract void SetData();
        
        public virtual void OnBuy(ShopItemType shopItemType)
        {
            SetData();
        }

        public virtual void OnUpgrade(ShopItemType shopItemType)
        {
            SetData();
        }
        
        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                ShopSignals.Instance.onOpenShopPanelType?.Invoke(ShopPanelType);
                //Ui signals open panel
                print($"{ShopPanelType} opened");
            }
        }
        
        public abstract void Save(int levelId);
        
        public abstract void Load(int levelId);
        
        #region EventSubscription

        public virtual void OnEnable()
        {
            SubscribeEvents();
        }

        public virtual void SubscribeEvents()
        {
            
        }
        
        public virtual void UnSubscribeEvents()
        {
            
        }

        public virtual void OnDisable()
        {
            UnSubscribeEvents();
        }

        #endregion

        #region Event Functions

        

        #endregion
    }
}