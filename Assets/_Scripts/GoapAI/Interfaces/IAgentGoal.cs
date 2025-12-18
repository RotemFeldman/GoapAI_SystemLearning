using System.Collections.Generic;

namespace GOAP
{
    public interface IAgentGoal
    {
        public string Name { get; }
        public float Priority { get; }
        public HashSet<IAgentBelief> DesiredEffects { get; }
    }
}