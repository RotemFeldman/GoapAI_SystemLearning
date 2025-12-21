using GOAP.Scriptable;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GOAP.Editor
{
    public class ActionDefinitionEditor : OdinValueDrawer<ActionDefinition>
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