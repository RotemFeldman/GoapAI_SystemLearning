using System.Collections.Generic;

namespace GOAP.Helpers
{
    public interface IBlackboardKeyProvider
    {
        IEnumerable<string> GetKeys<T>();
    }
}