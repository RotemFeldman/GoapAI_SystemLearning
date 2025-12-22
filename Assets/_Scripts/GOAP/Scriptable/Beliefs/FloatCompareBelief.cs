using BlackboardSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GOAP.Scriptable.Beliefs
{
    [LabelText("Compare Float")]
    public class FloatCompareBelief : BaseMathCompareBeliefDefinition
    {
        [Header("Compare")]
        [SerializeField] private CompareType _compareType = CompareType.Equal;

        [Header("Operands")]
        [SerializeField] private BlackboardBind<float> _aFloat = new();
        [SerializeField] private BlackboardBind<float> _bFloat = new();

        [Tooltip("If true and a key is missing, the belief evaluates to false.")]
        [SerializeField] private bool _failIfMissingKey = true;

        protected override bool Evaluate(Blackboard blackBoard)
        {
            var bindA = _aFloat;
            var bindB = _bFloat;
            var type = _compareType;
            var failMissing = _failIfMissingKey;
            
            float aVal, bVal;

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