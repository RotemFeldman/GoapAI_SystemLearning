using System;
using System.Collections.Generic;
using GOAP.Runtime;
using GOAP.Scriptable.Actions;
using GOAP.Unity;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GOAP.Scriptable
{
    [Serializable]
    public class ActionDefinition 
    {
        [FoldoutGroup("Action Settings")]
        [SerializeField] private string _name = "New Action";
        public string Name => _name;
        
        
        [HideInInspector]
        public Color InspectorColor = new(.2392f,.2392f,.2392f,1);
        
        /*[BoxGroup("Action Settings")]
        [HorizontalGroup("Action Settings/Header")]
        [LabelWidth(50)]*/
        

        [FoldoutGroup("Action Settings")]
        [VerticalGroup("Action Settings/Details")]
        public float Cost;
        
        [FoldoutGroup("Action Settings")]
        [VerticalGroup("Action Settings/Details")]
        [SerializeField] private List<BeliefRequirement> preconditions  = new ();
        
        [FoldoutGroup("Action Settings")]
        [VerticalGroup("Action Settings/Details")]
        [SerializeField] private List<BeliefRequirement> effects = new ();

        [FoldoutGroup("Action Settings")]
        [VerticalGroup("Action Settings/Details")]
        [field: SerializeReference] public BaseScriptableActionStrategy strategy;

        public AgentAction Build(ScriptableGoapAgent agent, BlackBoard blackBoard)
        {
            var builder = new AgentAction.Builder(Name).WithCost(Cost);

            foreach (var precondition in preconditions)
            {
                if (string.IsNullOrWhiteSpace(precondition.name)) continue;
                if (!agent.Beliefs.TryGetValue(precondition.name, out var belief)) continue;
                builder.WithPrecondition(belief,precondition.DesiredState);
            }

            foreach (var effect in effects)
            {
                if (string.IsNullOrWhiteSpace(effect.name)) continue;
                if (!agent.Beliefs.TryGetValue(effect.name, out var belief)) continue;
                builder.WithEffect(belief,effect.DesiredState);
            }

            
            builder.WithStrategy(strategy.Init(blackBoard));
            
            return builder.Build();
        }
    }
    
}