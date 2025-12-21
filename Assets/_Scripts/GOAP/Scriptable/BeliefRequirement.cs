using System;
using System.Collections;
using System.Linq;
using GOAP.Helpers;
using Sirenix.OdinInspector;
using Sisus.Init;
using UnityEngine;

namespace GOAP.Scriptable
{
    
    [Serializable]
    public class BeliefRequirement
    {
        [ValueDropdown("GetBeliefNames")]
        [OnInspectorGUI("ValidateBeliefs")]
        [HorizontalGroup,HideLabel]
        public string name;
        
        [HorizontalGroup(MarginRight = 20),LabelText("@StateLabel()")]
        public bool DesiredState = true;
        
        private IEnumerable GetBeliefNames()
        {
            if (Service.TryGet<IScriptableAgentProvider>(out var provider))
            {
                return provider.GetBeliefs().Select(b => b.Name);
            }
        
            return Enumerable.Empty<string>();
        }

        private void ValidateBeliefs()
        {
            if (Application.isPlaying) return;
            if (string.IsNullOrWhiteSpace(name)) return;

            if (Service.TryGet<IScriptableAgentProvider>(out var provider))
            {
                var validBeliefs = provider.GetBeliefs().Select(b => b.Name);
                if (!validBeliefs.Contains(name))
                {
                    Sirenix.Utilities.Editor.SirenixEditorGUI.ErrorMessageBox($"'{name}' not found in Agent Beliefs!");
                }
            }
        }

        private string StateLabel()
        {
            return DesiredState ? " IS TRUE" : " IS FALSE";
        }
    }
    
}