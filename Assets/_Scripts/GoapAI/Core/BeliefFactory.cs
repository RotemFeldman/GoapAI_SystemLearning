using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class BeliefFactory
    {
        private MonoAgent _agent;
        private Dictionary<string, IAgentBelief> _beliefs;

        public BeliefFactory(MonoAgent agent, Dictionary<string, IAgentBelief> beliefs)
        {
            _agent = agent;
            _beliefs = beliefs;
        }

        public void AddBelief(string name, Func<bool> condition)
        {
            var belief = new AgentBelief.Builder(name)
                .WithCondition(condition)
                .Build();
            
            _beliefs.Add(belief.Name, belief);
        }

        public void AddLocationBelief(string name, float distanceThreshold, Func<Vector3> location)
        {
            var belief = new AgentBelief.Builder(name)
                .WithLocation(location)
                .WithCondition(() => Vector3.Distance(location(), _agent.transform.position) <= distanceThreshold)
                .Build();
            
            _beliefs.Add(belief.Name, belief);
        }
    }
}