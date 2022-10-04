﻿using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace StateMachine.Enemy
{
    [RequireComponent(typeof(SphereCollider))]
    public class EnemyCloseRangeDetection : MonoBehaviour
    {
        [SerializeField] private EnemyAI manager;

        private void Awake()
        {
            GetComponent<SphereCollider>().radius = manager.AttackRange;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                manager.CanAttack = true;
            }
            
            if (other.CompareTag("GateOutside"))
            {
                ChangeReachedBaseState(true);
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                manager.CanAttack = false;
            }
            
            if (other.CompareTag("GateOutside"))
            {
                ChangeReachedBaseState(false);
            }
        }

        private async void ChangeReachedBaseState(bool value)
        {
            await Task.Delay(Random.Range(50,250));
            manager.ReachedAtBase = value;
        }
    }
}