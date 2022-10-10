using Commands;
using Data.ValueObject.Base;
using Signals;
using UnityEngine;


//Sadece Sinyaller buraya load ve save commandine gonderilmek icin kullaniyoruz.
namespace Managers
{
    public class SaveManager : MonoBehaviour
    {
        #region Self Variables

        #region Private Variables

        private LoadGameCommand _loadGameCommand;
        private SaveGameCommand _saveGameCommand;
 

        #endregion
        
        #endregion

        private void Awake()
        {
            Initialization();
        }

        private void Initialization()
        {
            _loadGameCommand = new LoadGameCommand();
            _saveGameCommand = new SaveGameCommand(); 
        }
        
        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            SaveLoadSignals.Instance.onSaveAmmoWorkerData += _saveGameCommand.Execute;
            SaveLoadSignals.Instance.onLoadAmmoWorkerData += _loadGameCommand.Execute<AmmoWorkerData>;

            SaveLoadSignals.Instance.onSaveMoneyWorkerData += _saveGameCommand.Execute;
            SaveLoadSignals.Instance.onLoadMoneyWorkerData += _loadGameCommand.Execute<MoneyWorkerData>;

            SaveLoadSignals.Instance.onSaveMineBaseData += _saveGameCommand.Execute;
            SaveLoadSignals.Instance.onLoadMineBaseData += _loadGameCommand.Execute<MineBaseData>;

            SaveLoadSignals.Instance.onSaveRoomData += _saveGameCommand.Execute;
            SaveLoadSignals.Instance.onLoadRoomData += _loadGameCommand.Execute<RoomData>;

            SaveLoadSignals.Instance.onSaveTurretData += _saveGameCommand.Execute;
            SaveLoadSignals.Instance.onLoadTurretData += _loadGameCommand.Execute<TurretData>;

            SaveLoadSignals.Instance.onSaveForceFieldData += _saveGameCommand.Execute;
            SaveLoadSignals.Instance.onLoadForceFieldData += _loadGameCommand.Execute<ForceFieldData>;
        }

        private void UnsubscribeEvents()
        {
            SaveLoadSignals.Instance.onSaveAmmoWorkerData -= _saveGameCommand.Execute;
            SaveLoadSignals.Instance.onLoadAmmoWorkerData -= _loadGameCommand.Execute<AmmoWorkerData>;
            
            SaveLoadSignals.Instance.onSaveMoneyWorkerData -= _saveGameCommand.Execute;
            SaveLoadSignals.Instance.onLoadMoneyWorkerData -= _loadGameCommand.Execute<MoneyWorkerData>;
            
            SaveLoadSignals.Instance.onSaveMineBaseData -= _saveGameCommand.Execute;
            SaveLoadSignals.Instance.onLoadMineBaseData -= _loadGameCommand.Execute<MineBaseData>;
            
            SaveLoadSignals.Instance.onSaveRoomData -= _saveGameCommand.Execute;
            SaveLoadSignals.Instance.onLoadRoomData -= _loadGameCommand.Execute<RoomData>;
            
            SaveLoadSignals.Instance.onSaveTurretData -= _saveGameCommand.Execute;
            SaveLoadSignals.Instance.onLoadTurretData -= _loadGameCommand.Execute<TurretData>;
            
            SaveLoadSignals.Instance.onSaveForceFieldData -= _saveGameCommand.Execute;
            SaveLoadSignals.Instance.onLoadForceFieldData -= _loadGameCommand.Execute<ForceFieldData>;
        }
        private void OnDisable()
        {
            UnsubscribeEvents();
        }
        
        #endregion
    }
}