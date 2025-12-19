using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GOAP.ScriptableGoap
{
    [Serializable]
    //[GUIColor("@$value.InspectorColor")]
    public abstract class BeliefDefinition
    {
        [HorizontalGroup("Top", Width = 0.1f),HideLabel]
        public Color InspectorColor = Color.white;
        
        [HorizontalGroup("Top"),SerializeField] private string _name;
        public string Name => _name;
        
        public string InspectorName => $"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(InspectorColor)}>{(string.IsNullOrEmpty(_name) ? "Unnamed" : _name)}</color>";
        
        public Func<bool> Condition => () => Evaluate(_blackBoard);
        
        private AgentBlackBoard _blackBoard;
        
        public void Initialize(AgentBlackBoard blackBoard) => _blackBoard = blackBoard;

        protected abstract bool Evaluate(AgentBlackBoard blackBoard);

    }
}