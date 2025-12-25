using System.Collections.Generic;
using BlackboardSystem;
using BlackboardSystem.Unity;
using GOAP.Runtime;
using GOAP.Unity;
using Sirenix.OdinInspector;
using UnityEngine;
using Sisus.Init;
using UnityEditor;

namespace GOAP.Scriptable
{
    [HideMonoScript]
    public class ScriptableGoapAgent : MonoAgent
    {
        [SerializeField, Required("Please assign a Goap Agent Data asset")]
        [InlineEditor(InlineEditorModes.FullEditor)]
        private GoapAgentData agentData;

        private Blackboard _blackboard = new Blackboard();
        public Blackboard Blackboard => _blackboard;
        private List<BeliefDefinition> localBeliefs = new List<BeliefDefinition>();

        protected override void Awake()
        {
            _blackboard.Initialize(gameObject);
            if (agentData != null && agentData.BlackBoardData != null)
            {
                agentData.BlackBoardData.SetValuesOnBlackboard(_blackboard);
            }

            base.Awake();
        }

        #region GOAP Setup Overrides

        protected override void SetupBeliefs()
        {
            base.SetupBeliefs();
            if (agentData == null) return;

            foreach (var belief in agentData.GetBeliefs())
            {
                localBeliefs.Add(belief.Clone());
            }

            foreach (var belief in localBeliefs)
            {
                belief.Initialize(_blackboard);
                var b = belief.Build();
                
                Beliefs[b.Name] = b;
            }
        }

        protected override void SetupActions()
        {
            base.SetupActions();
            if (agentData == null) return;

            /*foreach (var action in agentData.GetActions())
            {
                localActions.Add(action.);
            }*/
            
            foreach (var a in agentData.GetActions())
            {
                Actions.Add(a.Build(this, _blackboard));
            }
        }

        protected override void SetupGoals()
        {
            if (agentData == null) return;

            foreach (var g in agentData.Goals)
            {
                Goals.Add(g.Build(this));
            }
        }

        #endregion

        #region Editor Support

#if UNITY_EDITOR
        [OnInspectorGUI]
        private void UpdateEditorContext()
        {
            if (Application.isPlaying) return;
            
            if (agentData == null)
            {
                Service.Set<IScriptableAgentProvider>(ScriptableAgentProvider.Empty);
                Service.Set<IBlackboardKeyProvider>(BlackboardKeyProvider.Empty);
            }
        }
#endif
        #endregion
    }
}