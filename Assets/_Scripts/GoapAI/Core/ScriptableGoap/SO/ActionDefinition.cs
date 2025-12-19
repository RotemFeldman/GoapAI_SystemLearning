using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP.ScriptableGoap.SO
{
    [Serializable]
    public partial class ActionDefinition 
    {
        [field:SerializeField] public string Name { get; private set; }
        [field:SerializeField] public float Cost { get; private set; }
        
        [SerializeField] private List<BeliefName> preconditions  = new ();
        [SerializeField] private List<BeliefName> effects = new ();

        [field: SerializeReference] public BaseScriptableActionStrategy strategy;

        public AgentAction Build(ScriptableGoapAgent agent, AgentBlackBoard blackBoard)
        {
            var builder = new AgentAction.Builder(Name).WithCost(Cost);

            foreach (var precondition in preconditions)
            {
                if (string.IsNullOrWhiteSpace(precondition.name)) continue;
                if (!agent.Beliefs.TryGetValue(precondition.name, out var belief)) continue;
                builder.WithPrecondition(belief);
            }

            foreach (var effect in effects)
            {
                if (string.IsNullOrWhiteSpace(effect.name)) continue;
                if (!agent.Beliefs.TryGetValue(effect.name, out var belief)) continue;
                builder.WithEffect(belief);
            }

            
            builder.WithStrategy(strategy.Init(blackBoard));
            
            return builder.Build();
        }
    }
    
}