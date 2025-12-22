using BlackboardSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GOAP.Scriptable.Beliefs
{
    [LabelText("Boolean")]
    public class BooleanBelief : BeliefDefinition
    {
        [SerializeField] BlackboardBind<bool> _boolean = new();
        
        protected override bool Evaluate(Blackboard blackBoard)
        {
            if (_boolean.TryGetValue(blackBoard, out bool val))
            {
                return val;
            }
            return false;
        }
    }
}