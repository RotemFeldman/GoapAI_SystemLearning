using System.Collections.Generic;
using GOAP.Helpers;
using GOAP.Scriptable;
using UnityEngine;
using Sirenix.OdinInspector;
using Sisus.Init;

namespace GOAP.Unity
{
    [HideMonoScript]
    [Service(typeof(IBlackboardKeyProvider))]
    public class BlackBoard : SerializedMonoBehaviour, IBlackboardKeyProvider
    {
        [Space, SerializeField, Searchable, PropertyOrder(1)] 
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
        
        public bool Get<T>(string key, out T value) => TryGetValue(key, out value);
        
        public void Set<T>(string key, T value)
        {
            if (string.IsNullOrWhiteSpace(key)) return;

            BlackboardValue bbValue = value switch
            {
                bool b => new BoolValue(b),
                float f => new FloatValue(f),
                int i => new IntValue(i),
                Vector3 v => new Vector3Value(v),
                Transform t => new TransformValue(t),
                GameObject go => new GameObjectValue(go),
                _ => null
            };

            if (bbValue != null)
            {
                SetValue(key, bbValue);
            }
            else
            {
                Debug.LogError($"Blackboard: Type {typeof(T)} is not supported.");
            }
        }
        
        private void SetBool(string Key, bool Value) => SetValue(Key, new BoolValue(Value));
        private void SetFloat(string Key, float Value) => SetValue(Key, new FloatValue(Value));
        private void SetInt(string Key, int Value) => SetValue(Key, new IntValue(Value));
        private void SetVector3(string Key, Vector3 Value) => SetValue(Key, new Vector3Value(Value));
        private void SetTransform(string Key, Transform Value) => SetValue(Key, new TransformValue(Value));
        private void SetGameObject(string key, GameObject value) => SetValue(key, new GameObjectValue(value));
        
        private bool TryGetBool(string key, out bool value) => TryGetValue(key, out value);
        private bool TryGetFloat(string key, out float value) => TryGetValue(key, out value);
        private bool TryGetInt(string key, out int value) => TryGetValue(key, out value);
        private bool TryGetVector3(string key, out Vector3 value) => TryGetValue(key, out value);
        private bool TryGetTransform(string key, out Transform value) => TryGetValue(key, out value);
        private bool TryGetGameObject(string key, out GameObject value) => TryGetValue(key, out value);
        
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
                T rawValue = typedValue.Value;

                // Handle Unity-specific null check for destroyed objects
                if (rawValue is Object unityObject && unityObject == null)
                {
                    value = default;
                    return false;
                }

                value = rawValue;
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