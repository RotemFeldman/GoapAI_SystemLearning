using System;
using System.Collections;
using GOAP.Helpers;
using GOAP.Unity;
using Sirenix.OdinInspector;
using Sisus.Init;
using Sisus.Init.Internal;
using UnityEngine;

namespace GOAP.Scriptable
{
    [Serializable,InlineProperty,HideReferenceObjectPicker]
    [LabelText("@$property.NiceName + \" (\" + Sirenix.Utilities.TypeExtensions.GetNiceName($property.ValueEntry.TypeOfValue.GetGenericArguments()[0]).ToLower() + \")\"")]
    public class BlackboardBind<T>
    {
        public enum BindType { BlackboardKey, Constant }

        [HorizontalGroup("Bind"),HideLabel,LabelWidth(60)]
        [SerializeField] private BindType _bindType = BindType.Constant;
    
        [HorizontalGroup("Bind"),HideLabel]
        [ShowIf("@this._bindTypeInt == 1")]
        [SerializeField] private T _constantValue;
    
        [HorizontalGroup("Bind"),HideLabel]
        [ShowIf("@this._bindTypeInt == 0")]
        [ValueDropdown("GetBlackboardKeys")]
        [OnInspectorGUI("ValidateKey")]
        [SerializeField] private string _blackboardKey;
    
        private int _bindTypeInt => (int)_bindType;
        public string typeName => typeof(T).Name;

        public bool TryGetValue(BlackBoard blackBoard, out T _value)
        {
            if (_bindType == BindType.Constant)
            {
                _value = _constantValue;
                return true;
            }

            if (blackBoard == null)
            {
                Debug.LogWarning($"Blackboard is null while trying to resolve key {_blackboardKey}");
                _value = _constantValue;
                return false;
            }

            return blackBoard.Get(_blackboardKey, out _value);
        }

        private IEnumerable GetBlackboardKeys()
        {
            if (Service.TryGet<IBlackboardKeyProvider>(out var provider))
            {
                return provider.GetKeys<T>();
            }

            return new string[] { "Assign a Blackboard to Scene" };
        }

        private void ValidateKey()
        {
            if (_bindType != BindType.BlackboardKey || string.IsNullOrWhiteSpace(_blackboardKey)) return;

            if (Application.isPlaying) return;

            if (Service.TryGet<IBlackboardKeyProvider>(out var provider))
            {
                var validKeys = provider.GetKeys<T>();
                bool keyExist = false;
                foreach (var key in validKeys)
                {
                    if (key == _blackboardKey)
                    {
                        keyExist = true;
                        break;
                    }
                }

                if (!keyExist)
                {
                    Sirenix.Utilities.Editor.SirenixEditorGUI.ErrorMessageBox($"Key '{_blackboardKey}' not found in Blackboard!");

                }
            }
        }

    }
}