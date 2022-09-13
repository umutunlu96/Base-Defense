using System.Collections.Generic;
using Controllers;
using Data.ValueObject.Base;
using UnityEngine;

namespace Managers
{
    public class RoomManager : MonoBehaviour
    {
        public RoomData Data;
        [SerializeField] private int Identifier = 0;

        
        [SerializeField] private List<RoomAreaPhysicController> roomBuyArea;
        
        
        
        
        
        public void OnPlayerEnter()
        {
            
        }

        public void OnPlayerExit()
        {
            
        }
    }
}