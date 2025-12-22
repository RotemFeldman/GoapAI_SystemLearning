using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sirenix.OdinInspector;
using Sisus.Init;
using UnityEngine;

namespace BlackboardSystem
{
    [Serializable,InlineProperty,HideReferenceObjectPicker]
    [LabelText("@$property.NiceName + \" (\" + Sirenix.Utilities.TypeExtensions.GetNiceName($property.ValueEntry.TypeOfValue.GetGenericArguments()[0]).ToLower() + \")\"")]
    public class BlackboardBind<T>
    {
        public enum BindType { BlackboardKey, Constant, Function }

        [HorizontalGroup("Bind"),HideLabel,LabelWidth(60)]
        [SerializeField] private BindType _bindType = BindType.Constant;
    
        [HorizontalGroup("Bind"),HideLabel]
        [ShowIf("@this._bindTypeInt == 1")]
        [SerializeField] private T _constantValue;
    
        [HorizontalGroup("Bind"),HideLabel]
        [ShowIf("@this._bindTypeInt == 0")]
        [ValueDropdown("GetBlackboardKeysOfType")]
        [OnInspectorGUI("ValidateKey")]
        [SerializeField] private string _blackboardKey;
        
        //function bind
        [HorizontalGroup("Bind"), HideLabel] 
        [ShowIf("@this._bindTypeInt == 2")] 
        [SerializeField, Required] private Component _targetComponent; 
        
        [HorizontalGroup("Bind"), HideLabel] 
        [ShowIf("@this._bindTypeInt == 2 && this._targetComponent != null")] 
        [ValueDropdown("GetValidMethods")]
        [SerializeField] private string _methodName;
        
        private MethodInfo _cachedMethod;
        private bool _cachedInitialized;
    
        private int _bindTypeInt => (int)_bindType;
        private Type _type => typeof(T);

        public bool TryGetValue(Blackboard blackBoard, out T _value)
        {
            if (_bindType == BindType.Constant)
            {
                _value = _constantValue;
                return true;
            }

            if (_bindType == BindType.Function)
            {
                if (!_cachedInitialized) InitializeCache();

                if (_cachedMethod != null && _targetComponent != null)
                {
                    _value = (T)_cachedMethod.Invoke(_targetComponent, null);
                    return true;
                }
                _value = default;
                return false;
            }

            if (blackBoard == null)
            {
                Debug.LogWarning($"Blackboard is null while trying to resolve key {_blackboardKey}");
                _value = default;
                return false;
            }

            return blackBoard.TryGetValue(blackBoard.GetOrRegisterKey(_blackboardKey), out _value);
        }

        private void InitializeCache()
        {
            _cachedInitialized = true;
            if (_targetComponent == null || string.IsNullOrEmpty(_methodName)) return;
            
            _cachedMethod = _targetComponent.GetType().GetMethod(_methodName,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        private IEnumerable GetBlackboardKeysOfType()
        {
            if (Service.TryGet<IBlackboardKeyProvider>(out var provider))
            {
                var keys = provider.GetKeys<T>(); 
                if (keys != null) return keys;

                return new string[] { $"No keys of type {typeof(T).Name} found" };
            }

            return new string[] { "Assign a Blackboard to Scene" };
        }

        private void ValidateKey()
        {
            if (_bindType != BindType.BlackboardKey || string.IsNullOrWhiteSpace(_blackboardKey)) return;

            if (Application.isPlaying) return;

            if (Service.TryGet<IBlackboardKeyProvider>(out var provider))
            {
                IEnumerable<string> validKeys = null;
                try
                {
                    validKeys = provider.GetKeys();
                }
                catch { /*provider not ready*/ }

                if (validKeys == null) return;
                
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

        private IEnumerable GetValidMethods()
        {
            if (_targetComponent == null) return Enumerable.Empty<string>();
            
            return _targetComponent.GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(m => m.ReturnType == typeof(T) && m.GetParameters().Length == 0)
                .Select(m => m.Name);
        }

    }
}