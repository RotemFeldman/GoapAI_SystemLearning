using System.Collections.Generic;
using System.Linq;
using BlackboardSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GOAP.Scriptable.Beliefs
{
    [LabelText("Is Transform Null")]
    public class TransformNullCheckBelief : BeliefDefinition
    {
        [SerializeField] private BlackboardBind<Transform> _transform = new();
        
        [Tooltip("If true, the belief evaluates to true if the key exists and is not null.")]
        [SerializeField] bool reverse = false;
        protected override bool Evaluate(Blackboard blackBoard)
        {
            bool exists = _transform.TryGetValue(blackBoard, out var transform);
            
            if(reverse) return exists && transform != null;
            return !exists || transform == null;
        }
    }
}