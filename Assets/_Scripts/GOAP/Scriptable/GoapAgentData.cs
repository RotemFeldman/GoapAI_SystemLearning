using System.Collections.Generic;
using BlackboardSystem.Unity;
using Sirenix.OdinInspector;
using Sisus.Init;
using UnityEditor;
using UnityEngine;

namespace GOAP.Scriptable
{
    [CreateAssetMenu(fileName = "new GoapAgentData", menuName = "Behavior/GOAP/Agent Data", order = 0)]
    public class GoapAgentData : SerializedScriptableObject, IScriptableAgentProvider
    {
        [SerializeField, Required("Please assign a Blackboard Data asset")]
        [InlineEditor(InlineEditorModes.FullEditor)]
        private BlackboardData blackBoardData;

        public BlackboardData BlackBoardData => blackBoardData;

        [Title("Data Definitions")]
        [SerializeField, ListDrawerSettings(OnBeginListElementGUI = nameof(DrawBeliefElementLabel))]
        private List<BeliefDefinition> _beliefs = new();

        [Space, SerializeField, ListDrawerSettings(OnBeginListElementGUI = nameof(DrawActionElementLabel))]
        private List<ActionDefinition> _actions = new();

        [Space, SerializeField, ListDrawerSettings(OnBeginListElementGUI = nameof(DrawGoalElementLabel))]
        private List<GoalDefinition> _goals = new();

        public List<GoalDefinition> Goals => _goals;

        #region IScriptableAgentProvider Implementation

        public List<BeliefDefinition> GetBeliefs() => _beliefs;
        public List<ActionDefinition> GetActions() => _actions;

        #endregion

        #region Editor Support

        [OnInspectorGUI]
        private void SetupProviders()
        {
            if(Application.isPlaying) return;
            
            Service.Set<IScriptableAgentProvider>(this);
            
            IBlackboardKeyProvider provider = (blackBoardData != null) ? blackBoardData : BlackboardKeyProvider.Empty;
            Service.Set<IBlackboardKeyProvider>(provider);
        }

#if UNITY_EDITOR
        private void DrawBeliefElementLabel(int index)
        {
            var element = _beliefs[index];

            Rect rect = GUILayoutUtility.GetRect(0, 22, GUILayout.ExpandWidth(true));

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

            Rect rect = GUILayoutUtility.GetRect(0, 22, GUILayout.ExpandWidth(true));

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

            Rect rect = GUILayoutUtility.GetRect(0, 22, GUILayout.ExpandWidth(true));

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