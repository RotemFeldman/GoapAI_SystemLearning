namespace GOAP.Scriptable.Beliefs
{
    public abstract class BaseMathCompareBeliefDefinition : BeliefDefinition
    {
        protected enum CompareType
        {
            Equal, NotEqual, GreaterThan, LessThan, GreaterThanOrEqual, LessThanOrEqual
        }
    }
}