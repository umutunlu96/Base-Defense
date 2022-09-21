using DG.Tweening;
using StateMachine.Miner;
using UnityEngine;

namespace StateMachine
{
    public class PlaceDiamondToGatherArea : IState
    {
        private readonly MinerAI _minerAI;

        public PlaceDiamondToGatherArea(MinerAI minerAI)
        {
            _minerAI = minerAI;
        }

        public void Tick()
        {
            if (_minerAI.Take())
                Debug.Log("DiamondPlacedGatherArea");
            //Diamond place edilecek.
        }

        public void OnEnter() { }

        public void OnExit() { }
    }
}