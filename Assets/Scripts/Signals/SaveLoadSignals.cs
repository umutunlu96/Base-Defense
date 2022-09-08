using System;
using Extentions;
using UnityEngine.Events;

namespace Signals
{
    public class SaveLoadSignals : MonoSingleton<SaveLoadSignals>
    {
        // public UnityAction<LevelIdData,int> onSaveGameData = delegate { };
        //
        // public Func<string, int, LevelIdData> onLoadGameData;
        //
        // public UnityAction<BuildingsData,int> onSaveBuildingsData = delegate { };
        //
        // public Func<string, int, BuildingsData> onLoadBuildingsData;
        //
        // public UnityAction<IdleLevelData,int> onSaveIdleData = delegate {  };
        //
        // public Func<string, int, IdleLevelData> onLoadIdleData;
    }
}