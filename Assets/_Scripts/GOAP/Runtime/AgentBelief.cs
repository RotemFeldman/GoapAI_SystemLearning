using System;
using GOAP.Core;
using UnityEngine;

namespace GOAP.Runtime
{
    public class AgentBelief : IAgentBelief
    {
        public string Name { get; }
    
        public Func<Vector3> Location {get; private set; } = () => Vector3.zero;
    
        private Func<bool> _condition = () => false;

        private AgentBelief(string name)
        {
            Name = name;
        }
    
        public bool Evaluate()
        {
            return _condition();
        }

        public class Builder
        {
            private readonly AgentBelief _belief;

            public Builder(string name)
            {
                _belief = new AgentBelief(name);
            }

            public Builder WithLocation(Func<Vector3> location)
            {
                _belief.Location = location;
                return this;
            }

            public Builder WithCondition(Func<bool> condition)
            {
                _belief._condition = condition;
                return this;
            }
        
            public AgentBelief Build()
            {
                return _belief;
            }
        }
    }
}