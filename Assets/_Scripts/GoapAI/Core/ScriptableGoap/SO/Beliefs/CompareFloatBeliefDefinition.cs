using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace GOAP.ScriptableGoap.SO
{
    public class CompareFloatBeliefDefinition : BaseCompareBeliefDefinition
    {
        [Header("Compare")]
        [SerializeField] private CompareType _compareType = CompareType.Equal;

        [Header("Operands")]
        [InlineProperty,HideReferenceObjectPicker]
        [SerializeField] private BlackboardBind<float> _aFloat;
        
        [InlineProperty,HideReferenceObjectPicker]
        [SerializeField] private BlackboardBind<float> _bFloat;

        [Tooltip("If true and a key is missing, the belief evaluates to false.")]
        [SerializeField] private bool _failIfMissingKey = true;

        protected override bool Evaluate(AgentBlackBoard blackBoard)
        {
            var bindA = _aFloat;
            var bindB = _bFloat;
            var type = _compareType;
            var failMissing = _failIfMissingKey;
            
            float aVal, bVal;

            if (failMissing) 
            {
                Debug.Log(bindA.TryGetValue(blackBoard, out aVal));
                
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