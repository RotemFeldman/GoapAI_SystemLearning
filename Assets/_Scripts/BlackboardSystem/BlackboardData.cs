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
        [ListDrawerSettings(
            ShowFoldout = false, ShowPaging = false, DraggableItems = true, HideAddButton = true)]
        private List<BlackboardEntryData> entries = new();
        public List<BlackboardEntryData> Entries => entries;
        
        [Title("Blackboard Values"),PropertyOrder(-1)]
        [Button("Add Key")]
        private void AddKey()
        {
            entries.Add(new BlackboardEntryData());
        }
    }
}