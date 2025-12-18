namespace GOAP.ScriptableGoap.SO
{
    public abstract class BaseCompareBeliefDefinition : BeliefDefinition
    {
        public enum CompareType
        {
            Equal, NotEqual, GreaterThan, LessThan, GreaterThanOrEqual, LessThanOrEqual
        }
    }
}