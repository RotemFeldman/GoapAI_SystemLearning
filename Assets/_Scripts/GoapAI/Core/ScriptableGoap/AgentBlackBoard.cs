using System;
using System.Collections;
using System.Collections.Generic;
using GOAP.ScriptableGoap.Helpers;
using UnityEngine;
using Sirenix.OdinInspector;
using Sisus.Init;

namespace GOAP.ScriptableGoap
{
    [Service(typeof(IBlackboardKeyProvider))]
    public class AgentBlackBoard : SerializedMonoBehaviour, IBlackboardKeyProvider
    {
        [Space,SerializeField, PropertyOrder(1)] 
        [DictionaryDrawerSettings(
            DisplayMode = DictionaryDisplayOptions.ExpandedFoldout
            )]
        private Dictionary<string, BlackboardValue> _blackboard = new();

        [SerializeField, HideLabel, HorizontalGroup("Split")] private string _newKeyName;
        [SerializeField, HideLabel, HorizontalGroup("Split")] private BlackboardValueType _newType;

        [Button("Add Key")]
        public void AddKeyFromInspector()
        {
            if (string.IsNullOrWhiteSpace(_newKeyName))
                throw new System.ArgumentException("Key cannot be null or whitespace.");
            
            if (HasKey(_newKeyName))
                throw new System.ArgumentException($"Key '{_newKeyName}' already exists.");
            
            SetFromInspector();
            _newKeyName = string.Empty;
        }
        
        public bool HasKey(string key) => !string.IsNullOrWhiteSpace(key) && _blackboard.ContainsKey(key);
        
        public void Remove(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) return;
            _blackboard.Remove(key);
        }
        
        public void Clear() => _blackboard.Clear();
        
        public void SetBool(string Key, bool Value) => SetValue(Key, new BoolValue(Value));
        public void SetFloat(string Key, float Value) => SetValue(Key, new FloatValue(Value));
        public void SetInt(string Key, int Value) => SetValue(Key, new IntValue(Value));
        public void SetVector3(string Key, Vector3 Value) => SetValue(Key, new Vector3Value(Value));
        public void SetTransform(string Key, Transform Value) => SetValue(Key, new TransformValue(Value));
        public void SetGameObject(string key, GameObject value) => SetValue(key, new GameObjectValue(value));
        
        public bool TryGetBool(string key, out bool value) => TryGetValue(key, out value);
        public bool TryGetFloat(string key, out float value) => TryGetValue(key, out value);
        public bool TryGetInt(string key, out int value) => TryGetValue(key, out value);
        public bool TryGetVector3(string key, out Vector3 value) => TryGetValue(key, out value);
        public bool TryGetTransform(string key, out Transform value) => TryGetValue(key, out value);
        public bool TryGetGameObject(string key, out GameObject value) => TryGetValue(key, out value);
        
        public bool Get<T>(string key, out T value) => TryGetValue(key, out value);
        
        private void SetValue(string key, BlackboardValue value)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new System.ArgumentException("Key cannot be null or whitespace.", nameof(key));
            
            if(value == null)
                throw new System.ArgumentNullException(nameof(value));
            
            _blackboard[key] = value;
        }
        
        private bool TryGetValue<T>(string key, out T value)
        {
            value = default;
            
            if (string.IsNullOrWhiteSpace(key)) return false;
            if (!_blackboard.TryGetValue(key, out var bbValue) || bbValue == null) return false;

            if (bbValue is BlackboardValue<T> typedValue)
            {
                value = typedValue.Value;
                return true;
            }
            
            Debug.LogWarning($"Blackboard key '{key}' exists but type mismatch. Expected {typeof(T)}, found {bbValue?.ValueType}");

            return false;
        }

        private bool SetFromInspector()
        {
            switch (_newType)
            {
                case BlackboardValueType.Bool:
                    SetBool(_newKeyName, false);
                    break;
                case BlackboardValueType.Float:
                    SetFloat(_newKeyName, 0f);
                    break;
                case BlackboardValueType.Int:
                    SetInt(_newKeyName, 0);
                    break;
                case BlackboardValueType.Vector3:
                    SetVector3(_newKeyName, Vector3.zero);
                    break;
                case BlackboardValueType.Transform:
                    SetTransform(_newKeyName, null);
                    break;
                case BlackboardValueType.GameObject:
                    SetGameObject(_newKeyName, null);
                    break;
                default:
                    throw new System.ArgumentOutOfRangeException(nameof(_newType), _newType, "Unsupported blackboard value type.");
            }

            return true;
        }

        

        public IEnumerable<string> GetKeys<T>()
        {
            foreach (var kvp in _blackboard)
            {
                if (kvp.Value.ValueType == typeof(T))
                {
                    yield return kvp.Key;
                }
            }
        }
    }
}