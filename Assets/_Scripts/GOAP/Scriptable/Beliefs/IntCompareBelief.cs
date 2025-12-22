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


        private bool Compare(float a, float b, CompareType type)
        {
            switch (type)
            {
                case CompareType.Equal:
                    return Mathf.Approximately(a, b);

                case CompareType.NotEqual:
                    return !Mathf.Approximately(a, b);

                case CompareType.GreaterThan:
                    return a > b; 

                case CompareType.LessThan:
                    return a < b; 

                case CompareType.GreaterThanOrEqual:
                    return a >= b;

                case CompareType.LessThanOrEqual:
                    return a <= b;

                default:
                    return false;
            }
        }
    }
}