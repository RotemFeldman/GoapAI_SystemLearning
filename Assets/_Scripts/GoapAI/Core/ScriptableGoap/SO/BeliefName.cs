using System;
using System.Collections;
using System.Linq;
using GOAP.ScriptableGoap.Helpers;
using Sirenix.OdinInspector;
using Sisus.Init;
using UnityEngine;

namespace GOAP.ScriptableGoap.SO
{
    
    [Serializable]
    public struct BeliefName
    {
        [ValueDropdown("GetBeliefNames")]
        [OnInspectorGUI("ValidateBeliefs")]
        public string name;
        
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
    }
    
}