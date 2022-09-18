using System.Collections.Generic;
using UnityEngine;

namespace Commands
{
    public class AddStackCommand
    {
        private List<Transform> _collected;
        private Transform _transform;
        private Vector3 _nextPos;
        
        public AddStackCommand(ref List<Transform> collected,Transform transform, ref Vector3 nextPos)
        {
            _collected = collected;
            _transform = transform;
            _nextPos = nextPos;
        }

        public void Execute(Transform collected)
        {
            collected.SetParent(_transform);
            
            _collected.Add(collected);
            _collected.TrimExcess();
        }
    }
}