using System.Collections.Generic;

namespace GOAP.Core
{
    public interface IAgentGoal
    {
        public string Name { get; }
        public float Priority { get; }
        public HashSet<BeliefState> DesiredEffects { get; }
    }
}