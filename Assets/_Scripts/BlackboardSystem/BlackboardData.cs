using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace BlackboardSystem.Unity
{
    [CreateAssetMenu(fileName = "new Blackboard", menuName = "Behavior/Blackboard/Blackboard Data", order = 0)]
    public class BlackboardData : SerializedScriptableObject , IBlackboardKeyProvider
    {
        [OdinSerialize,InlineProperty,HideReferenceObjectPicker,Searchable]
        [ListDrawerSettings(ShowFoldout = false, ShowPaging = false, DraggableItems = true, HideAddButton = true)]
        [ValidateInput("ValidateUniqueKeys", "Duplicate key names found!")]
        private List<BlackboardEntryData> entries = new();
        public List<BlackboardEntryData> Entries => entries;
        
        public void SetValuesOnBlackboard(Blackboard targetBlackboard)
        {
            var data = Entries;
            if (data == null) return;

            foreach (var entry in data)
            {
                entry.SetValueOnBlackboard(targetBlackboard);
            }
        }
        
        [Title("Blackboard Values"),PropertyOrder(-1)]
        [Button("Add Key")]
        private void AddKey()
        {
            entries.Add(new BlackboardEntryData());
        }

        #region Editor Support & Validation

        private bool ValidateUniqueKeys(List<BlackboardEntryData> list, ref string errorMessage)
        {
            if (list == null) return true;
            var keys = new HashSet<string>();
            foreach (var entry in list)
            {
                if (string.IsNullOrEmpty(entry.keyName)) continue;
                if (!keys.Add(entry.keyName))
                {
                    errorMessage = $"Duplicate key name {entry.keyName}";
                    return false;
                }
            }
            return true;
        }

        #endregion

        #region IBlackboardKeyProvider implementation

        public IEnumerable<string> GetKeys<T>()
        {
            if(entries == null) yield break;
            
            foreach (var entry in entries)
            {
                if (entry.value.GetActiveType() == typeof(T))
                    yield return entry.keyName;
            }
        }

        public IEnumerable<string> GetKeys()
        {
            if (entries == null) yield break;

            foreach (var entry in entries)
            {
                yield return entry.keyName;
            }
        }

        #endregion

        
    }
}