using System.Collections.Generic;


public interface IBlackboardKeyProvider
{
    IEnumerable<string> GetKeys<T>();
}
