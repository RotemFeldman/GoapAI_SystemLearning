using BlackboardSystem;
using GOAP.Unity;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GOAP.Scriptable.Beliefs
{
    [LabelText("Compare Int")]
    public class IntCompareBelief : BaseMathCompareBeliefDefinition
    {
        [Header("Compare")]
        [SerializeField] private CompareType _compareType = CompareType.Equal;

        [Header("Operands")]
        [SerializeField] private BlackboardBind<int> _aInt = new();
        [SerializeField] private BlackboardBind<int> _bInt = new();

        [Tooltip("If true and a key is missing, the belief evaluates to false.")]
        [SerializeField] private bool _failIfMissingKey = true;

        protected override bool Evaluate(Blackboard blackBoard)
        {
            var bindA = _aInt;
            var bindB = _bInt;
            var type = _compareType;
            var failMissing = _failIfMissingKey;
            
            int aVal, bVal;

            if (failMissing) 
            {
                if (!bindA.TryGetValue(blackBoard, out aVal)) return false;
                if (!bindB.TryGetValue(blackBoard, out bVal)) return false;
            }
            else
            {
                bindA.TryGetValue(blackBoard, out aVal); 
                bindB.TryGetValue(blackBoard, out bVal);
            }
            
            return Compare(aVal, bVal, type);
        }
    }
}