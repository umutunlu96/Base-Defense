using System;
using System.Collections.Generic;
using Enums;
using Signals;
using UnityEngine;

namespace Managers.Shop
{
    public class CanvasShopManager : MonoBehaviour
    {
        [SerializeField] private List<GameObject> Panels;

        #region EventSubscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            ShopSignals.Instance.onOpenShopPanelType += OnOpenPanel;
            ShopSignals.Instance.onCloseShopPanelType += OnClosePanel;
        }
        
        private void UnSubscribeEvents()
        {
            ShopSignals.Instance.onOpenShopPanelType -= OnOpenPanel;
            ShopSignals.Instance.onCloseShopPanelType -= OnClosePanel;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        #endregion

        #region Event Functions

        private void OnOpenPanel(ShopPanelType panelType)
        {
            Panels[(int)panelType].SetActive(true);
        }

        public void OnClosePanel(int panelOrder)
        {
            Panels[panelOrder].SetActive(true);
        }

        #endregion
        
        public void OnBuyButtonPressed(string ItemTypeName)
        {
            ShopItemType shopItemType = (ShopItemType) Enum.Parse(typeof(ShopItemType), ItemTypeName);
            ShopSignals.Instance.onPressedBuyButton?.Invoke(shopItemType);
        }

        public void OnUpgradeButtonPressed(string ItemTypeName)
        {
            ShopItemType shopItemType = (ShopItemType) Enum.Parse(typeof(ShopItemType), ItemTypeName);
            ShopSignals.Instance.onPressedUpgradeButton?.Invoke(shopItemType);
        }
    }
}