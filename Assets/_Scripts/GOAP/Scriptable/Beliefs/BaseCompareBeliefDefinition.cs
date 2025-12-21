namespace GOAP.Scriptable.Beliefs
{
    public abstract class BaseCompareBeliefDefinition : BeliefDefinition
    {
        public enum CompareType
        {
            Equal, NotEqual, GreaterThan, LessThan, GreaterThanOrEqual, LessThanOrEqual
        }
    }
}