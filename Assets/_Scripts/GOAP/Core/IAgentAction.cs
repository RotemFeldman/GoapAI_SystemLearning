using System.Collections.Generic;

namespace GOAP.Core
{
    public interface IAgentAction
    {
        public string Name { get; }
        public float Cost { get; }
        public HashSet<BeliefState> Preconditions { get; }
        public HashSet<BeliefState> Effects { get; }
        public bool Complete { get; }

        public void Start();
        void Update(float deltaTime);
        void Stop();
    }
}