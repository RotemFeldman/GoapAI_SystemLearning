using System.Collections.Generic;

namespace GOAP.ScriptableGoap.Helpers
{
    public interface IBlackboardKeyProvider
    {
        IEnumerable<string> GetKeys<T>();
    }
}