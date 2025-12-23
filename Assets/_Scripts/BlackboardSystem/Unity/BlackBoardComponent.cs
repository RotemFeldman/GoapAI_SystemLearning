
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
        private enum BlackboardSource { Local, Asset }
        
        [TitleGroup("Blackboard Configuration"),PropertyOrder(-5)]
        [SerializeField, EnumToggleButtons, HideLabel]
        private BlackboardSource source = BlackboardSource.Local;

        [SerializeField, Required("Please assign a Blackboard Data asset")]
        [ShowIf(nameof(source), BlackboardSource.Asset)]
        private BlackboardData assetData;
        
        [TitleGroup("Runtime Blackboard")]
        [ShowInInspector, ReadOnly, HideInEditorMode]
        private Blackboard blackboard=new();
        public Blackboard Blackboard => blackboard;

        [TitleGroup("Data Entries")]
        [OdinSerialize, HideInPlayMode, InlineProperty, HideReferenceObjectPicker, Searchable]
        [ListDrawerSettings(ShowFoldout = false, ShowPaging = false, DraggableItems = true, HideAddButton = true)]
        [ShowIf(nameof(source), BlackboardSource.Local)]
        [ValidateInput(nameof(ValidateUniqueKeys), "Duplicate key names found!")]
        private List<BlackboardEntryData> entries = new();

        private List<BlackboardEntryData> ActualEntries => source switch
        {
            BlackboardSource.Local => entries,
            BlackboardSource.Asset => assetData != null ? assetData.Entries : null,
            _ => null
        };

        [TitleGroup("Data Entries"),PropertyOrder(-1)]
        [Button(ButtonSizes.Medium)]
        [ShowIf(nameof(source), BlackboardSource.Local)]
        private void AddKey() => entries.Add(new BlackboardEntryData());

        private void Awake()
        {
            blackboard.Initialize(gameObject);
            SetValuesOnBlackboard(blackboard);
        }

        public void SetValuesOnBlackboard(Blackboard targetBlackboard)
        {
            var data = ActualEntries;
            if (data == null) return;

            foreach (var entry in data)
            {
                entry.SetValueOnBlackboard(targetBlackboard);
            }
        }

        #region IBlackboardKeyProvider Implementation

        public IEnumerable<string> GetKeys<T>()
        {
            var data = ActualEntries;
            if (data == null) yield break;

            foreach (var entry in data)
            {
                if (entry.value.GetActiveType() == typeof(T))
                    yield return entry.keyName;
            }
        }

        public IEnumerable<string> GetKeys()
        {
            var data = ActualEntries;
            if (data == null) yield break;

            foreach (var entry in data)
            {
                yield return entry.keyName;
            }
        }

        #endregion

        #region Editor Support & Validation

#if UNITY_EDITOR
        [OnInspectorGUI]
        private void UpdateEditorContext()
        {
            if (Application.isPlaying) return;
            Service.SetInstance<IBlackboardKeyProvider>(this);
        }

        private bool ValidateUniqueKeys(List<BlackboardEntryData> list, ref string errorMessage)
        {
            if (list == null) return true;
            var keys = new HashSet<string>();
            foreach (var entry in list)
            {
                if (string.IsNullOrEmpty(entry.keyName)) continue;
                if (!keys.Add(entry.keyName))
                {
                    errorMessage = $"Duplicate key name: {entry.keyName}";
                    return false;
                }
            }
            return true;
        }
#endif
        #endregion
    }
}