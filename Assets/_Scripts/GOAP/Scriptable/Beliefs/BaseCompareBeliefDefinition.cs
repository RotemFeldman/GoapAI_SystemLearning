using UnityEngine;

namespace GOAP.Scriptable.Beliefs
{
    public abstract class BaseMathCompareBeliefDefinition : BeliefDefinition
    {
        protected enum CompareType
        {
            Equal, NotEqual, GreaterThan, LessThan, GreaterThanOrEqual, LessThanOrEqual
        }
        
        protected bool Compare(float a, float b, CompareType type)
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