using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace BlackboardSystem.Unity
{
    [CreateAssetMenu(fileName = "new Blackboard", menuName = "Behavior/Blackboard", order = 0)]
    public class BlackboardData : SerializedScriptableObject
    {
        [OdinSerialize,InlineProperty,HideReferenceObjectPicker,Searchable]
        [ListDrawerSettings(ShowFoldout = false, ShowPaging = false, DraggableItems = true, HideAddButton = true)]
        [ValidateInput("ValidateUniqueKeys", "Duplicate key names found!")]
        private List<BlackboardEntryData> entries = new();
        public List<BlackboardEntryData> Entries => entries;
        
        [Title("Blackboard Values"),PropertyOrder(-1)]
        [Button("Add Key")]
        private void AddKey()
        {
            entries.Add(new BlackboardEntryData());
        }
        
        private bool ValidateUniqueKeys(List<BlackboardEntryData> list, ref string errorMessage)
        {
            if (list == null) return true;
            var keys = new HashSet<string>();
            foreach (var entry in list)
            {
                if(string.IsNullOrEmpty(entry.keyName)) continue;
                if (!keys.Add(entry.keyName))
                {
                    errorMessage = $"Duplicate key name {entry.keyName}";
                    return false;
                }
            }
            return true;
        }
    }
}