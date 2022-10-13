using System;
using Enums;
using Extentions;

namespace Signals
{
    public class ShopSignals : MonoSingleton<ShopSignals>
    {
        public Action<ShopPanelType> onOpenShopPanelType;
        public Action<int> onCloseShopPanelType;
        
        public Action<ShopItemType> onPressedBuyButton;
        public Action<ShopItemType> onPressedUpgradeButton;
    }
}