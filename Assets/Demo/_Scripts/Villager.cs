using System;
using UnityEngine;

namespace Demo._Scripts
{
    public class Villager : MonoBehaviour, ILogCarrier
    {
        public TreeComponent TargetTree;
        bool hasLog = false;
        
        private Animator _animator;
        private Vector3 _lastFramePosition;
        
        public Transform GetTargetTree() => TargetTree == null ? null : TargetTree.transform;
        
        public bool HasLog()
        {
            return hasLog;
        }

        public void PickupLog()
        {
            if (hasLog) return;
            hasLog = true;
        }

        public void DropLog()
        {
            if (!hasLog) return;
            hasLog = false;
        }

        private void Start()
        {
            _animator = GetComponentInChildren<Animator>();
            _lastFramePosition = transform.position;
        }

        private void Update()
        {
            float moved = 0;
            if (_lastFramePosition != transform.position) moved = 1;
            _lastFramePosition = transform.position;
            
            _animator.SetFloat("Velocity", moved);
        }
    }
}