using System.Collections.Generic;
using UnityEngine;

namespace Commands
{
    public class RemoveStackCommand
    {
        private List<Transform> _collectable;

        public RemoveStackCommand(ref List<Transform> collectable)
        {
            _collectable = collectable;
        }

        public void Execute(Transform collectable)
        {
            if(_collectable.Count == 0) return;
            _collectable.Remove(collectable);
            _collectable.TrimExcess();
        }
    }
}