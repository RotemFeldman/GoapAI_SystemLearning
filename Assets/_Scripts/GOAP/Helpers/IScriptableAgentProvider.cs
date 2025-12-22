using System.Collections.Generic;
using GOAP.Scriptable;


public interface IScriptableAgentProvider
{
    List<BeliefDefinition> GetBeliefs();
    List<ActionDefinition> GetActions();
    
}
