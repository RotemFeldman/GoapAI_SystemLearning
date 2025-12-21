using GOAP.ScriptableGoap;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GOAP
{
    public class BeliefDefinitionEditor : OdinValueDrawer<BeliefDefinition>
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

            /*// Expand rect to cover margins for better visibility when collapsed
            var expandedRect = new Rect(rect.x - 4, rect.y - 2, rect.width + 8, rect.height + 4);*/
            EditorGUI.DrawRect(rect, item.InspectorColor);

            CallNextDrawer(label);

            EditorGUILayout.EndVertical();
        }
    }
}