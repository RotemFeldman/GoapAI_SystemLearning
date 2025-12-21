using System.Collections.Generic;
using GOAP.Scriptable;

namespace GOAP.Helpers
{
    public interface IScriptableAgentProvider
    {
        List<BeliefDefinition> GetBeliefs();
        List<ActionDefinition> GetActions();
        
    }
}