using System.Collections.Generic;

namespace GOAP
{
    public class AgentGoal : IAgentGoal
    {
        public string Name { get; }
        public float Priority { get; private set; }
        public HashSet<IAgentBelief> DesiredEffects { get; private set; }

        private AgentGoal(string name)
        {
            Name = name;
            DesiredEffects = new HashSet<IAgentBelief>();
        }
        
        public class Builder
        {
            private readonly AgentGoal _goal;

            public Builder(string name)
            {
                _goal = new AgentGoal(name);
            }

            public Builder WithPriority(float priority)
            {
                _goal.Priority = priority;
                return this;
            }

            public Builder AddDesiredEffect(IAgentBelief belief)
            {
                _goal.DesiredEffects.Add(belief);
                return this;
            }
            
            public AgentGoal Build()
            {
                return _goal;
            }
        }
    }
}