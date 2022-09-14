using System.Collections.Generic;
using UnityEngine;

namespace Commands
{
    public class AddStackCommand
    {
        private List<Transform> _hostages;
        private Transform _transform;

        public AddStackCommand(ref List<Transform> hostages, ref Transform transform)
        {
            _hostages = hostages;
            _transform = transform;
        }

        public void Execute(Transform hostage)
        {
            hostage.tag = "Rescued";
            hostage.SetParent(_transform);
            _hostages.Add(hostage);
            _hostages.TrimExcess();
        }
    }
}