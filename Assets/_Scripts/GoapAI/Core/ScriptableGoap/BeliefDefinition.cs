using System;
using UnityEngine;

namespace GOAP.ScriptableGoap
{
    [Serializable]
    public abstract class BeliefDefinition
    {
        [SerializeField] private string _name;
        public string Name => _name;
        
        public Func<bool> Condition => () => Evaluate(_blackBoard);
        
        private AgentBlackBoard _blackBoard;
        
        public void Initialize(AgentBlackBoard blackBoard) => _blackBoard = blackBoard;

        protected abstract bool Evaluate(AgentBlackBoard blackBoard);

    }
}