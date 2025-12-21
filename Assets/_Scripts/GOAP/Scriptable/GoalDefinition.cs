using System;
using System.Collections.Generic;
using GOAP.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GOAP.Scriptable
{
    [Serializable]
    public class GoalDefinition
    {
        [HideInInspector]
        public Color InspectorColor = Color.clear;

        [FoldoutGroup("Goal Settings")] 
        [SerializeField] public string name = "New Goal";
        public string Name => name;

        [FoldoutGroup("Goal Settings")]
        [SerializeField] private float priority;
        
        [FoldoutGroup("Goal Settings")]
        [SerializeField] private List<BeliefRequirement> desiredEffects = new ();

        public AgentGoal Build(ScriptableGoapAgent agent)
        {
            var builder = new AgentGoal.Builder(Name).WithPriority(priority);

            foreach (var effect in desiredEffects)
            {
                builder.AddDesiredEffect(agent.Beliefs[effect.name], effect.DesiredState);
            }
            
            return builder.Build();
        }
    }
}