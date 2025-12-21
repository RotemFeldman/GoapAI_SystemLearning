using GOAP.Scriptable;
using GOAP.Unity;
using UnityEngine;

namespace GOAP.Scriptable.Beliefs
{
    public class TransformDistanceBeliefDefinition : BaseDistanceBeliefDefinition
    {
        [Header("Distance Check Type")]
        public DistanceCheckType DistanceCheckType = DistanceCheckType.InRange;
        
        [Header("Operands")]
        [SerializeField] private BlackboardBind<Transform> TransformA = new();
        [SerializeField] private BlackboardBind<Transform> TransformB = new();
        [Space(20)]
        [SerializeField] private BlackboardBind<float> Threshold = new();
        
        [Tooltip("If true and a key is missing, the belief evaluates to false.")]
        [SerializeField] private bool _failIfMissingKey = true;
        
        protected override bool Evaluate(BlackBoard blackBoard)
        {
            var _bindA = TransformA;
            var _bindB = TransformB;
            var _threshold = Threshold;

            Transform aVal;
            Transform bVal;
            float thresholdVal;

            if (_failIfMissingKey)
            {
                if (!_bindA.TryGetValue(blackBoard, out  aVal)) return false;
                if (!_bindB.TryGetValue(blackBoard, out  bVal)) return false;
                if (!_threshold.TryGetValue(blackBoard, out  thresholdVal)) return false;
            }
            else
            {
                _bindA.TryGetValue(blackBoard,out  aVal); 
                _bindB.TryGetValue(blackBoard,out  bVal); 
                _threshold.TryGetValue(blackBoard,out  thresholdVal);
            }
            
            var aPos = aVal.position;
            var bPos = bVal.position;
            var dist = Vector3.Distance(aPos, bPos);
            
            return DistanceCheckType == DistanceCheckType.InRange ? dist <= thresholdVal : dist >= thresholdVal;
        }
    }
}