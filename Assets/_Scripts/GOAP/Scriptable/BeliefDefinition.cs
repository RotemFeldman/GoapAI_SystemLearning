using System;
using GOAP.Unity;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GOAP.Scriptable
{
    [Serializable]
    public abstract class BeliefDefinition
    {
        
        [HideInInspector]
        public Color InspectorColor = Color.clear;
        
        [HorizontalGroup("Top")]
        [SerializeField] private string _name = "New Belief";
        public string Name => _name;
        
        
        public Func<bool> Condition => () => Evaluate(_blackBoard);
        
        private BlackBoard _blackBoard;
        
        public void Initialize(BlackBoard blackBoard) => _blackBoard = blackBoard;

        protected abstract bool Evaluate(BlackBoard blackBoard);
    }
}