using System.Collections.Generic;
using GOAP.Scriptable;


public interface IScriptableAgentProvider
{
    List<BeliefDefinition> GetBeliefs();
    List<ActionDefinition> GetActions();
    
}

public static class ScriptableAgentProvider
{
    public static IScriptableAgentProvider Empty = new EmptyProvider();
    
    private class EmptyProvider : IScriptableAgentProvider
    {
        public List<BeliefDefinition> GetBeliefs()
        {
            return new List<BeliefDefinition>();
        }

        public List<ActionDefinition> GetActions()
        {
            return new List<ActionDefinition>();
        }
    }
}
