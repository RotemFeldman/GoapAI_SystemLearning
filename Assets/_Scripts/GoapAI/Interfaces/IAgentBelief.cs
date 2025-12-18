using System;
using UnityEngine;

namespace GOAP
{
    public interface IAgentBelief
    {
        public string Name { get; }
        public Func<Vector3> Location { get; }
        public bool Evaluate();
    }
}