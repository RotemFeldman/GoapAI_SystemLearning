using System.Collections.Generic;
using GOAP.Core;

namespace GOAP.Runtime
{
    public class AgentGoal : IAgentGoal
    {
        public string Name { get; }
        public float Priority { get; private set; }
        public HashSet<BeliefState> DesiredEffects { get; private set; }

        private AgentGoal(string name)
        {
            Name = name;
            DesiredEffects = new HashSet<BeliefState>();
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

            public Builder AddDesiredEffect(IAgentBelief belief, bool desiredState = true)
            {
                _goal.DesiredEffects.Add(new BeliefState(belief, desiredState));
                return this;
            }
            
            public AgentGoal Build()
            {
                return _goal;
            }
        }
    }
}