using System.Collections.Generic;
using UnityEngine;

namespace Commands
{
    public class RemoveStackCommand
    {
        private List<Transform> _hostages;

        public RemoveStackCommand(ref List<Transform> hostages)
        {
            _hostages = hostages;
        }

        public void Execute(Transform collectable)
        {
            if(_hostages.Count == 0) return;
            _hostages.Remove(collectable);
            _hostages.TrimExcess();
        }
    }
}