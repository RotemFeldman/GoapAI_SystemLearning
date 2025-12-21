using System;
using System.Collections.Generic;
using GOAP.ScriptableGoap.SO;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GOAP.ScriptableGoap
{
    [Serializable]
    public class GoalDefinition
    {
        [HorizontalGroup("Top", Width = 0.1f)]
        [HideLabel]
        public Color InspectorColor = Color.white;
        
        [HorizontalGroup("Top")]
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public float Priority { get; private set; }
        
        
        [SerializeField] private List<BeliefName> desiredEffects = new ();

        public AgentGoal Build(ScriptableGoapAgent agent)
        {
            var builder = new AgentGoal.Builder(Name).WithPriority(Priority);

            foreach (var effect in desiredEffects)
            {
                builder.AddDesiredEffect(agent.Beliefs[effect.name]);
            }
            
            return builder.Build();
        }
    }
}