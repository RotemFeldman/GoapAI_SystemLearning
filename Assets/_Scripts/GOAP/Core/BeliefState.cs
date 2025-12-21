using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace GOAP.Core
{
    [Serializable]
    public struct BeliefState : IEquatable<BeliefState>
    {
        [SerializeField] private IAgentBelief belief;
        [SerializeField] private bool desiredState;
        
        public BeliefState(IAgentBelief belief, bool state = true)
        {
            this.belief = belief;
            this.desiredState = state;
        }

        public bool IsSatisfied()
        {
            return belief.Evaluate() == desiredState;
        }

        public bool Equals(BeliefState other)
        {
            return Equals(belief, other.belief) && desiredState == other.desiredState;
        }

        public override bool Equals(object obj)
        {
            return obj is BeliefState other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(belief, desiredState);
        }
    }
}