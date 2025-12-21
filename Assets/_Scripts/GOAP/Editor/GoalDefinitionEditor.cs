using GOAP.Scriptable;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace GOAP.Editor
{
    public class GoalDefinitionEditor : OdinValueDrawer<GoalDefinition>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            var item = ValueEntry.SmartValue;

            if (item == null)
            {
                CallNextDrawer(label);
                return;
            }

            var rect = EditorGUILayout.BeginVertical();
            
            EditorGUI.DrawRect(rect, item.InspectorColor);

            CallNextDrawer(label);

            EditorGUILayout.EndVertical();
        }
    }
}