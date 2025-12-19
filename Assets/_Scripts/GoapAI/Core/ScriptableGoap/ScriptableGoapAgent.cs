using System;
using System.Collections.Generic;
using System.Linq;
using GOAP.ScriptableGoap.Helpers;
using GOAP.ScriptableGoap.SO;
using Sirenix.OdinInspector;
using UnityEngine;
using Sisus.Init;

namespace GOAP.ScriptableGoap
{
    [RequireComponent(typeof(AgentBlackBoard))]
    [Service(typeof(IScriptableAgentProvider))]
    public class ScriptableGoapAgent : MonoAgent, IScriptableAgentProvider
    {
        [SerializeField] private AgentBlackBoard _blackBoard;

#if UNITY_EDITOR
        [Sirenix.OdinInspector.OnInspectorGUI]
        private void UpdateEditorContext()
        {
            if (Application.isPlaying) return;
            
            Service.SetInstance<IBlackboardKeyProvider>(_blackBoard);
            Service.SetInstance<IScriptableAgentProvider>(this);
        }
#endif
        
        [ListDrawerSettings(ListElementLabelName = "@this.Name")]
        [SerializeField] List<BeliefDefinition> _beliefs = new();
        
        [Space(30),SerializeField]
        [ListDrawerSettings(ListElementLabelName = "@this.Name")]
        List<ActionDefinition> _actions = new();
        
        [Space(30),SerializeField]
        [ListDrawerSettings(ListElementLabelName = "@this.Name")]
        List<GoalDefinition> _goals = new();
        
        private void Start()
        {
            var beliefsInfo = string.Join("\n", Beliefs.Select(b => 
                $"  • {b.Key} (Current State: <color={(b.Value.Evaluate() ? "green" : "red")}>{b.Value.Evaluate()}</color>)"));

            var actionsInfo = string.Join("\n", Actions.Select(a => 
                $"  • <b>{a.Name}</b> (Cost: {a.Cost})\n" +
                $"    - Needs: [{string.Join(", ", a.Preconditions.Select(p => p.Name))}]\n" +
                $"    - Grants: [{string.Join(", ", a.Effects.Select(e => e.Name))}]"));

            var goalsInfo = string.Join("\n", Goals.Select(g => 
                $"  • <b>{g.Name}</b> (Priority: {g.Priority})\n" +
                $"    - Desires: [{string.Join(", ", g.DesiredEffects.Select(e => e.Name))}]"));

            Debug.Log($"<color=cyan>===== GOAP Agent Initialized =====</color>\n\n" +
                      $"<b>Runtime Beliefs ({Beliefs.Count}):</b>\n{beliefsInfo}\n\n" +
                      $"<b>Available Actions ({Actions.Count}):</b>\n{actionsInfo}\n\n" +
                      $"<b>Active Goals ({Goals.Count}):</b>\n{goalsInfo}");
        }

        public List<BeliefDefinition> GetBeliefs()
        {
            return _beliefs;
        }

        public List<ActionDefinition> GetActions()
        {
            return _actions;
        }

        protected override void SetupBeliefs(BeliefFactory factory)
        {
            base.SetupBeliefs(factory);
            foreach (var b in _beliefs)
            {
                b.Initialize(_blackBoard);
                factory.AddBelief(b.Name, b.Condition);
            }
        }

        protected override void SetupActions()
        {
            base.SetupActions();
            foreach (var a in _actions)
            {
                Actions.Add(a.Build(this, _blackBoard));
            }
        }

        protected override void SetupGoals()
        {
            foreach (var g in _goals)
            {
                Goals.Add(g.Build(this));
            }
        }
    }
}