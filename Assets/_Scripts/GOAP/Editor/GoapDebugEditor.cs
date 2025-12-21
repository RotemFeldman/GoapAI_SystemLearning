using UnityEditor;
using UnityEngine;
using GOAP.Core;
using GOAP.Unity;
using Sirenix.OdinInspector.Editor;

namespace GOAP.Editor
{
    [CustomEditor(typeof(GoapDebug))]
    public class GoapDebugEditor : OdinEditor
    {
        private IGoapAgent _agent;

        protected override void OnEnable()
        {
            base.OnEnable();
            var debugTag = (GoapDebug)target;
            _agent = debugTag.GetComponent<IGoapAgent>();
        }

        public override void OnInspectorGUI()
        {
            if (!Application.isPlaying)
            {
                EditorGUILayout.HelpBox("Enter Play Mode to see GOAP Runtime State", MessageType.Info);
                return;
            }

            if (_agent == null)
            {
                EditorGUILayout.HelpBox("No IGoapAgent found on this GameObject.", MessageType.Warning);
                return;
            }

            DrawRuntimeStatus();
            
            // Force repaint to update UI in real-time
            Repaint();
        }

        private void DrawRuntimeStatus()
        {
            // --- Current Plan Section ---
            Sirenix.Utilities.Editor.SirenixEditorGUI.BeginBox("Current Execution");
            
            var goalName = _agent.CurrentGoal?.Name ?? "None";
            EditorGUILayout.LabelField("Target Goal:", goalName, EditorStyles.boldLabel);

            var actionName = _agent.CurrentAction?.Name ?? "Idle";
            GUI.color = Color.cyan;
            EditorGUILayout.LabelField("Active Action:", actionName, EditorStyles.boldLabel);
            GUI.color = Color.white;
            
            Sirenix.Utilities.Editor.SirenixEditorGUI.EndBox();

            EditorGUILayout.Space(5);

            // --- Beliefs Section ---
            // Fix: Reordered parameters to (key, label, visible)
            if (Sirenix.Utilities.Editor.SirenixEditorGUI.BeginFadeGroup(this, "Beliefs", true))
            {
                foreach (var belief in _agent.Beliefs)
                {
                    bool val = belief.Value.Evaluate();
                    GUI.color = val ? Color.green : Color.red;
                    EditorGUILayout.Toggle(belief.Key, val);
                }
            }
            GUI.color = Color.white;
            Sirenix.Utilities.Editor.SirenixEditorGUI.EndFadeGroup();

            EditorGUILayout.Space(5);

            // --- Planned Actions Section ---
            // Fix: Reordered parameters to (key, label, visible)
            if (Sirenix.Utilities.Editor.SirenixEditorGUI.BeginFadeGroup(this, "Plan Queue", true))
            {
                var plan = _agent.ActionPlan?.Actions;
                if (plan != null && plan.Count > 0)
                {
                    int i = 0;
                    foreach (var action in plan)
                    {
                        EditorGUILayout.LabelField($"[{i++}] {action.Name}");
                    }
                }
                else
                {
                    EditorGUILayout.LabelField("No active plan.");
                }
            }
            Sirenix.Utilities.Editor.SirenixEditorGUI.EndFadeGroup();
        }
    }
}
