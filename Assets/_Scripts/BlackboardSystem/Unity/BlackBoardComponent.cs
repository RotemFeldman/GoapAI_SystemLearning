
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sisus.Init;
using Sirenix.Serialization;
using UnityEngine;

namespace BlackboardSystem.Unity
{
    [HideMonoScript]
    [Service(typeof(IBlackboardKeyProvider))]
    public class BlackBoardComponent : SerializedMonoBehaviour, IBlackboardKeyProvider
    {
#if UNITY_EDITOR
        [OnInspectorGUI]
        private void UpdateEditorContext()
        {
            if (Application.isPlaying) return;
            Service.SetInstance<IBlackboardKeyProvider>(this);
        }
#endif
        private enum BlackboardSource {Local, Asset}
        [SerializeField,EnumToggleButtons,HideLabel,PropertyOrder(-5)]
        private BlackboardSource source  = BlackboardSource.Local;
        
        [ShowIf(nameof(source),BlackboardSource.Asset)]
        [SerializeField,Required("Please assign a Blackboard Data asset")]
        BlackboardData assetData;
        
        
        [Title("Blackboard Values")]
        [ShowInInspector,ReadOnly,HideInEditorMode]
        [SerializeField] private Blackboard blackboard = new();
        public Blackboard Blackboard => blackboard;
        
        [OdinSerialize,HideInPlayMode,InlineProperty,HideReferenceObjectPicker,Searchable]
        [ListDrawerSettings(
            ShowFoldout = false, ShowPaging = false, DraggableItems = true, HideAddButton = true)]
        [ShowIf(nameof(source),BlackboardSource.Local)]
        [ValidateInput("ValidateUniqueKeys", "Duplicate key names found!")]
        private List<BlackboardEntryData> entries = new();

        private List<BlackboardEntryData> actualEntries
        {
            get
            {
                if (source == BlackboardSource.Local) return entries;
                return assetData != null ? assetData.Entries : null;
            }
        }
        
        [ShowIf(nameof(source),BlackboardSource.Local)]
        [Title("Blackboard Values"),PropertyOrder(-1)]
        [Button("Add Key")]
        private void AddKey()
        {
            entries.Add(new BlackboardEntryData());
        }

        private void Awake()
        {
            SetValuesOnBlackboard(blackboard);
        }

        public void SetValuesOnBlackboard(Blackboard blackboard)
        {
            foreach (var entry in actualEntries)
            {
                entry.SetValueOnBlackboard(blackboard);
            }
        }

        public IEnumerable<string> GetKeys<T>()
        {
            if (actualEntries == null || actualEntries.Count == 0)
                yield break;
            
            foreach (var entry in actualEntries)
            {
                if(entry.value.GetActiveType() == typeof(T))
                    yield return entry.keyName;
            }
        }

        public IEnumerable<string> GetKeys()
        {
            if (actualEntries == null || actualEntries.Count == 0)
                yield break;
            
            foreach (var entry in actualEntries)
            {
                yield return entry.keyName;
            }
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