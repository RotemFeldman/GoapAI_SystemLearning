using GOAP.Unity;
using Sirenix.OdinInspector;
using UnityEngine;
using BlackboardSystem;

namespace GOAP.Scriptable.Beliefs
{
    [LabelText("Compare Bool")]
    public class BoolCompareBelief : BeliefDefinition
    {
        [SerializeField] private BlackboardBind<bool> _aBool = new();
        [SerializeField] private BlackboardBind<bool> _bBool = new();
        
        
        protected override bool Evaluate(Blackboard blackBoard)
        {
            if(!_aBool.TryGetValue(blackBoard,out var aVal)) return false; 
            if(!_bBool.TryGetValue(blackBoard,out var bVal)) return false; 
            
            return aVal == bVal;
        }
    }
}