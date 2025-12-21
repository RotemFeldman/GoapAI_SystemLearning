using System.Collections.Generic;
using GOAP.Core;

namespace GOAP.Runtime
{
    public class AgentAction : IAgentAction
    {
        public string Name { get; }
        public float Cost { get; private set; }
        public HashSet<BeliefState> Preconditions { get; private set;}
        public HashSet<BeliefState> Effects { get; private set; }

        public IActionStrategy Strategy { get; private set; }
        public bool Complete => Strategy.Complete;

        private AgentAction(string name)
        {
            Name = name;
        }
        
        
        public void Start()
        {
            Strategy.Start();
        }

        public void Update(float deltaTime)
        {
            if (Strategy.CanPerform)
            {
                Strategy.Update(deltaTime);
            }
            
            if (!Strategy.Complete) return;

            foreach (var eff in Effects)
            {
                eff.IsSatisfied();
            }
            
        }

        public void Stop()
        {
            Strategy.Stop();
        }

        public class Builder
        {
            readonly AgentAction _action;

            public Builder(string name)
            {
                _action = new AgentAction(name)
                {
                    Cost = 1,
                    Effects = new HashSet<BeliefState>(),
                    Preconditions = new HashSet<BeliefState>()
                };
            }

            public Builder WithCost(float cost)
            {
                _action.Cost = cost;
                return this;
            }
            
            public Builder WithPrecondition(IAgentBelief belief, bool desiredState = true)
            {
                _action.Preconditions.Add(new BeliefState(belief, desiredState));
                return this;
            }
            
            public Builder WithEffect(IAgentBelief belief, bool desiredState = true)
            {
                _action.Effects.Add(new BeliefState(belief, desiredState));
                return this;
            }

            public Builder WithStrategy(IActionStrategy strategy)
            {
                _action.Strategy = strategy;
                return this;
            }
            
            public AgentAction Build()
            {
                return _action;
            }
        }
    }

    public interface IActionStrategy
    {
        bool Complete { get; }
        bool CanPerform { get; }

        void Start();
        void Update(float deltaTime);
        void Stop();
    }
}