using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GOAP.ScriptableGoap.SO
{
    [Serializable]
    public partial class ActionDefinition 
    {
        [BoxGroup("Action Settings")]
        [HorizontalGroup("Action Settings/Header", Width = 0.1f)]
        [HideLabel]
        public Color InspectorColor = Color.cyan;
        
        [BoxGroup("Action Settings")]
        [HorizontalGroup("Action Settings/Header")]
        [LabelWidth(50)]
        [field:SerializeField] public string Name { get; private set; }

        [BoxGroup("Action Settings")]
        [VerticalGroup("Action Settings/Details")]
        public float Cost;
        
        [BoxGroup("Action Settings")]
        [VerticalGroup("Action Settings/Details")]
        [SerializeField] private List<BeliefName> preconditions  = new ();
        
        [BoxGroup("Action Settings")]
        [VerticalGroup("Action Settings/Details")]
        [SerializeField] private List<BeliefName> effects = new ();

        [BoxGroup("Action Settings")]
        [VerticalGroup("Action Settings/Details")]
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