using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GOAP.ScriptableGoap
{
    [Serializable]
    public abstract class BeliefDefinition
    {
        
        [HorizontalGroup("Top", Width = 0.1f),HideLabel]
        public Color InspectorColor = Color.white;
        
        [HorizontalGroup("Top"),SerializeField] private string _name = "New Belief";
        public string Name => _name;
        
        
        public Func<bool> Condition => () => Evaluate(_blackBoard);
        
        private BlackBoard _blackBoard;
        
        public void Initialize(BlackBoard blackBoard) => _blackBoard = blackBoard;

        protected abstract bool Evaluate(BlackBoard blackBoard);

    }
}