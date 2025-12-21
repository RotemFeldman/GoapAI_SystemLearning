using System;
using UnityEngine;

namespace GOAP.Core
{
    public interface IAgentBelief
    {
        public string Name { get; }
        public Func<Vector3> Location { get; }
        public bool Evaluate();
    }
}