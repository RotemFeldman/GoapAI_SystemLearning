using System.Collections.Generic;
using GOAP.ScriptableGoap.SO;

namespace GOAP.ScriptableGoap.Helpers
{
    public interface IBlackboardKeyProvider
    {
        IEnumerable<string> GetKeys<T>();
    }
    
    public interface IScriptableAgentProvider
    {
        List<BeliefDefinition> GetBeliefs();
        List<ActionDefinition> GetActions();
        
    }
}