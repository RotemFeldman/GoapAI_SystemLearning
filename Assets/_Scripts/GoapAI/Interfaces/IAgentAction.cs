using System.Collections.Generic;

namespace GOAP
{
    public interface IAgentAction
    {
        public string Name { get; }
        public float Cost { get; }
        public HashSet<IAgentBelief> Preconditions { get; }
        public HashSet<IAgentBelief> Effects { get; }
        public bool Complete { get; }

        public void Start();
        void Update(float deltaTime);
        void Stop();
    }
}