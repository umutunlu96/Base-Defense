namespace StateMachine.Miner
{
    public class PlaceDiamondToStockpileArea : IState
    {
        private readonly MinerAI _minerAI;

        public PlaceDiamondToStockpileArea(MinerAI minerAI)
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