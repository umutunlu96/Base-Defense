using System.Collections.Generic;
using UnityEngine;

namespace Commands
{
    public class AddStackCommand
    {

        private List<Transform> _collectable;
        private Transform _transform;
        private MonoBehaviour _monoBehaviour;

        public AddStackCommand(ref List<Transform> collectable, Transform transform, MonoBehaviour monoBehaviour)
        {
            _collectable = collectable;
            _transform = transform;
            _monoBehaviour = monoBehaviour;
        }

        public void OnAddStack(Transform collectable)
        {
            collectable.tag = "Collected";
            collectable.SetParent(_transform);
            _collectable.Add(collectable);
            _collectable.TrimExcess();
        }
    }
}