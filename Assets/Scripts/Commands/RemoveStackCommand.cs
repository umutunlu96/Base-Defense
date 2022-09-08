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

        public void OnRemoveFromStack(Transform collectable)
        {
            _collectable.Remove(collectable);
            _collectable.TrimExcess();
            collectable.gameObject.SetActive(false);
            // StackSignals.Instance.onCollectableRemovedFromStack?.Invoke();
            if(_collectable.Count == 0) return;
            // StackSignals.Instance.onSetScoreControllerPosition?.Invoke(_collectable[0]);
        }
    }
}