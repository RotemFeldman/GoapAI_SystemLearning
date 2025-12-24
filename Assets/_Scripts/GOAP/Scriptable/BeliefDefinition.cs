using System;
using BlackboardSystem;
using GOAP.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace GOAP.Scriptable
{
    [Serializable]
    public abstract class BeliefDefinition
    {
        [HideInInspector]
        public Color InspectorColor = new(.2392f,.2392f,.2392f,1);
        
        [HorizontalGroup("Top"),SerializeField] 
        private string name = "New Belief";
        public string Name => name;
        
        public Func<bool> Condition => () => Evaluate(_blackBoard);
        
        private Blackboard _blackBoard;
        
        public void Initialize(Blackboard blackBoard) => _blackBoard = blackBoard;

        public BeliefDefinition Clone()
        {
            string json = JsonUtility.ToJson(this);
            return (BeliefDefinition)JsonUtility.FromJson(json, GetType());
        }

        public AgentBelief Build()
        {
            return new AgentBelief.Builder(name)
                .WithCondition(Condition)
                .Build();
        }

        protected abstract bool Evaluate(Blackboard blackBoard);
    }
}