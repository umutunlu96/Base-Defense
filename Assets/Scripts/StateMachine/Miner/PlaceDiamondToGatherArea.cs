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

        }

        public void OnEnter()
        {
            _minerAI.PlaceDiamondToGatherArea();
        }

        public void OnExit() { }
    }
}