using System.Collections.Generic;
using System.Linq;


public interface IBlackboardKeyProvider
{
    IEnumerable<string> GetKeys<T>();
    IEnumerable<string> GetKeys();
}

public static class BlackboardKeyProvider
{
    public static readonly IBlackboardKeyProvider Empty = new EmptyProvider();

    private class EmptyProvider : IBlackboardKeyProvider
    {
        public IEnumerable<string> GetKeys<T>()
        {
            return Enumerable.Empty<string>();
        }

        public IEnumerable<string> GetKeys()
        {
            return Enumerable.Empty<string>();
        }
    }
}
