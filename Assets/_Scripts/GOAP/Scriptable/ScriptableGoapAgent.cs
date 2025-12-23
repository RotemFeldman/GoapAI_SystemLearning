using System.Collections.Generic;
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
    [RequireComponent(typeof(BlackBoardComponent))]
    [Service(typeof(IScriptableAgentProvider),FindFromScene = true)]
    public class ScriptableGoapAgent : MonoAgent, IScriptableAgentProvider
    {
        [Title("Data Definitions")]
        [SerializeField, ListDrawerSettings(OnBeginListElementGUI = nameof(DrawBeliefElementLabel))]
        private List<BeliefDefinition> _beliefs = new();

        [Space,SerializeField, ListDrawerSettings(OnBeginListElementGUI = nameof(DrawActionElementLabel))]
        private List<ActionDefinition> _actions = new();

        [Space,SerializeField, ListDrawerSettings(OnBeginListElementGUI = nameof(DrawGoalElementLabel))]
        private List<GoalDefinition> _goals = new();

        private BlackBoardComponent _blackBoardComponent;
        
        private void OnValidate()
        {
            if (_blackBoardComponent == null)
            {
                _blackBoardComponent = GetComponent<BlackBoardComponent>();
            }
        }
        

        #region IScriptableAgentProvider Implementation

        public List<BeliefDefinition> GetBeliefs() => _beliefs;
        public List<ActionDefinition> GetActions() => _actions;

        #endregion

        #region GOAP Setup Overrides

        protected override void SetupBeliefs(BeliefFactory factory)
        {
            base.SetupBeliefs(factory);
            foreach (var b in _beliefs)
            {
                b.Initialize(_blackBoardComponent.Blackboard);
                factory.AddBelief(b.Name, b.Condition);
            }
        }

        protected override void SetupActions()
        {
            base.SetupActions();
            foreach (var a in _actions)
            {
                Actions.Add(a.Build(this, _blackBoardComponent.Blackboard));
            }
        }

        protected override void SetupGoals()
        {
            foreach (var g in _goals)
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
            Service.SetInstance<IScriptableAgentProvider>(this);
        }

        private void DrawBeliefElementLabel(int index)
        {
            var element = _beliefs[index];
            
            Rect rect = GUILayoutUtility.GetRect(0,22,GUILayout.ExpandWidth(true));
            
            float colorFieldWidth = 60f;
            Rect labelRect = new Rect(rect.x, rect.y, rect.width - colorFieldWidth - 5f, rect.height);
            Rect colorRect = new Rect(rect.x + rect.width - colorFieldWidth, rect.y, colorFieldWidth, rect.height);

            Color bg = element.InspectorColor;

            EditorGUI.DrawRect(labelRect, bg);
            GUI.Label(labelRect, element.Name, EditorStyles.boldLabel);

            element.InspectorColor = EditorGUI.ColorField(colorRect, GUIContent.none, element.InspectorColor);
        }
        
        private void DrawActionElementLabel(int index)
        {
            var element = _actions[index];
            
            Rect rect = GUILayoutUtility.GetRect(0,22,GUILayout.ExpandWidth(true));
            
            float colorFieldWidth = 60f;
            Rect labelRect = new Rect(rect.x, rect.y, rect.width - colorFieldWidth - 5f, rect.height);
            Rect colorRect = new Rect(rect.x + rect.width - colorFieldWidth, rect.y, colorFieldWidth, rect.height);

            Color bg = element.InspectorColor;

            EditorGUI.DrawRect(labelRect, bg);
            GUI.Label(labelRect, element.Name, EditorStyles.boldLabel);

            element.InspectorColor = EditorGUI.ColorField(colorRect, GUIContent.none, element.InspectorColor);
        }
        
        private void DrawGoalElementLabel(int index)
        {
            var element = _goals[index];
            
            Rect rect = GUILayoutUtility.GetRect(0,22,GUILayout.ExpandWidth(true));
            
            float colorFieldWidth = 60f;
            Rect labelRect = new Rect(rect.x, rect.y, rect.width - colorFieldWidth - 5f, rect.height);
            Rect colorRect = new Rect(rect.x + rect.width - colorFieldWidth, rect.y, colorFieldWidth, rect.height);

            Color bg = element.InspectorColor;

            EditorGUI.DrawRect(labelRect, bg);
            GUI.Label(labelRect, element.Name, EditorStyles.boldLabel);

            element.InspectorColor = EditorGUI.ColorField(colorRect, GUIContent.none, element.InspectorColor);
        }
#endif
        #endregion
    }
}