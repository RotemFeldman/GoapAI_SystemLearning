using System;
using UnityEngine;

namespace GOAP.Scriptable.Actions
{
    [Serializable]
    public class ScriptableMoveStrategy : BaseScriptableActionStrategy
    {
        [SerializeField] private BlackboardBind<Transform> self;
        [SerializeField] private BlackboardBind<Transform> target;
        [SerializeField] private BlackboardBind<float> distanceThreshold;
        
        private Transform _self;
        private Transform _target;
        private float distance;

        public override bool Complete => InRange();
        public override bool CanPerform => ValidateField();
        public override void Start()
        {
            self.TryGetValue(blackBoard, out _self);
            target.TryGetValue(blackBoard, out _target);
            distanceThreshold.TryGetValue(blackBoard, out distance);
        }

        public override void Update(float deltaTime)
        {
            _self.position = Vector3.MoveTowards(_self.position, _target.position, deltaTime);
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