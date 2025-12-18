using System;
using System.Collections.Generic;
using GOAP.ScriptableGoap.Helpers;
using Sirenix.OdinInspector;
using UnityEngine;
using Sisus.Init;

namespace GOAP.ScriptableGoap
{
    [RequireComponent(typeof(AgentBlackBoard))]
    public class ScriptableGoapAgent : MonoAgent
    {
        [SerializeField] private AgentBlackBoard _blackBoard;

#if UNITY_EDITOR
        [Sirenix.OdinInspector.OnInspectorGUI]
        private void UpdateEditorContect()
        {
            Service.SetInstance<IBlackboardKeyProvider>(_blackBoard);
        }
#endif
        
        [ListDrawerSettings(ListElementLabelName = "@this.Name")]
        [SerializeField] List<BeliefDefinition> _beliefs;

        protected override void Awake()
        {
            base.Awake();
            foreach (var b in _beliefs)
            {
                b.Initialize(_blackBoard);
                beliefFactory.AddBelief(b.Name, b.Condition);
            }
        }

        private void LateUpdate()
        {
            var x = Beliefs["IsHungry"].Name + ": " + Beliefs["IsHungry"].Evaluate();
            var y = _blackBoard.TryGetFloat("a", out var aVal);
            var z = _blackBoard.TryGetFloat("b", out var bVal);
            
            Debug.Log(x + ", aVal: " + aVal + ", bVal: " + bVal);
        }
    }
}