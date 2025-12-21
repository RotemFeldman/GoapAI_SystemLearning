using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GOAP.Scriptable.Actions
{
    [Serializable]
    [LabelText("Move To Target")]
    public class MoveToTargetAction : BaseScriptableActionStrategy
    {
        //[SerializeField] private BlackboardBind<Transform> self;
        [SerializeField] private BlackboardBind<Transform> target;
        [SerializeField] private BlackboardBind<float> distanceThreshold;
        [SerializeField] private BlackboardBind<float> moveSpeed;
        
        private Transform _self;
        private Transform _target;
        private float distance;
        private float speed;

        public override bool Complete => InRange();
        public override bool CanPerform => ValidateField();
        public override void Start()
        {
            if (_self == null)
                _self = blackBoard.transform;
            
            target.TryGetValue(blackBoard, out _target);
            distanceThreshold.TryGetValue(blackBoard, out distance);
            moveSpeed.TryGetValue(blackBoard, out speed);
        }

        public override void Update(float deltaTime)
        {
            _self.position = Vector3.MoveTowards(_self.position, _target.position, deltaTime * speed);
        }

        public override void Stop()
        {
        }

        private bool InRange()
        {
            return Vector3.Distance(_self.position, _target.position) < distance;
        }

        private bool ValidateField()
        {
            return _self != null && _target != null;
        }
    }
}