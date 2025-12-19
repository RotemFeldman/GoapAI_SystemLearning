using System;
using UnityEngine;

namespace GOAP.ScriptableGoap
{
    public abstract class BaseScriptableActionStrategy : IActionStrategy
    {
        public abstract bool Complete { get; }
        public abstract bool CanPerform { get; }
        
        protected AgentBlackBoard blackBoard;
        
        public BaseScriptableActionStrategy Init(AgentBlackBoard blackBoard)
        {
            this.blackBoard = blackBoard;
            return this;
        }

        public abstract void Start();
        public abstract void Update(float deltaTime);
        public abstract void Stop();
    }
    
    [Serializable]
    public class ScriptableMoveStrategy : BaseScriptableActionStrategy
    {
        [SerializeField] private BlackboardBind<Transform> self;
        [SerializeField] private BlackboardBind<Transform> target;
        [SerializeField] private float distanceThreshold = 0.1f;
        
        private Transform _self;
        private Transform _target;

        public override bool Complete => InRange();
        public override bool CanPerform => ValidateField();
        public override void Start()
        {
            self.TryGetValue(blackBoard, out _self);
            target.TryGetValue(blackBoard, out _target);
            
            UnityEngine.Debug.Log("Started Move Strategy");
        }

        public override void Update(float deltaTime)
        {
            Debug.Log("Update Move Strategy");
            _self.position = Vector3.MoveTowards(_self.position, _target.position, deltaTime);
        }

        public override void Stop()
        {
            Debug.Log("Stop Move Strategy");
        }

        private bool InRange()
        {
            return Vector3.Distance(_self.position, _target.position) < distanceThreshold;
        }

        private bool ValidateField()
        {
            return _self != null && _target != null;
        }
    }
}