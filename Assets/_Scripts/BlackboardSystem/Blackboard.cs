using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sisus.Init;

namespace BlackboardSystem
{
    [Serializable]
    public readonly struct BlackboardKey : IEquatable<BlackboardKey>
    {
        readonly string name;
        readonly int hashedKey;

        public BlackboardKey(string name)
        {
            this.name = name;
            hashedKey = name.ComputeFNV1aHash();
        }


        public override bool Equals(object obj) => obj is BlackboardKey other && Equals(other);
        public bool Equals(BlackboardKey other) => hashedKey == other.hashedKey;
        public override int GetHashCode() => hashedKey;
        public override string ToString() => name;

        public static bool operator ==(BlackboardKey lhs, BlackboardKey rhs) => lhs.hashedKey == rhs.hashedKey;
        public static bool operator !=(BlackboardKey lhs, BlackboardKey rhs) => !(lhs == rhs);
    }

    [Serializable]
    public class BlackboardEntry<T>
    {
        public BlackboardKey Key { get; }
        public T Value { get; }
        public Type ValueType { get; }
        
        public BlackboardEntry(BlackboardKey key, T value)
        {
            Key = key;
            Value = value;
            ValueType = typeof(T);
        }

        public override bool Equals(object obj) => obj is BlackboardEntry<T> other && other.Key == Key;
        public override int GetHashCode() => Key.GetHashCode();
    }
    
    [Serializable]
    public class Blackboard
    {
        [OdinSerialize,NonSerialized]
        Dictionary<string, BlackboardKey> _keyRegistry = new();
        [OdinSerialize,NonSerialized]
        private Dictionary<BlackboardKey, object> _entries = new();

        public void InitializeFromInspector(Dictionary<string, object> inspectorEntries)
        {
            _entries.Clear();
            _keyRegistry.Clear();
            
            if (inspectorEntries == null) return;

            foreach (var item in inspectorEntries)
            {
                if (item.Value == null) continue;
                
                var key = GetOrRegisterKey(item.Key);
                var eType = typeof(BlackboardEntry<>).MakeGenericType(item.Value.GetType());
                var entry = Activator.CreateInstance(eType, key, item.Value);
                _entries[key] = entry;
            }
        }

        public void Debug()
        {
            foreach (var entry in _entries)
            {
                var eType = entry.Value.GetType();

                if (eType.IsGenericType && eType.GetGenericTypeDefinition() == typeof(BlackboardEntry<>))
                {
                    var valueProperty = eType.GetProperty("Value");
                    if (valueProperty == null) continue;
                    var value = valueProperty.GetValue(entry.Value);
                    UnityEngine.Debug.Log($"Key: {entry.Key}, Value: {value}");
                }
            }
        }

        public bool TryGetValue<T>(BlackboardKey key, out T value)
        {
            if (_entries.TryGetValue(key, out object entry) && entry is BlackboardEntry<T> castedEntry)
            {
                value = castedEntry.Value;
                return true;
            }
            
            value = default;
            return false;
        }
        
        public void SetVlaue<T>(BlackboardKey key, T value)
        {
            _entries[key] = new BlackboardEntry<T>(key, value);
        }

        public BlackboardKey GetOrRegisterKey(string name)
        {
            Preconditions.CheckNotNull(name);

            if (!_keyRegistry.TryGetValue(name, out BlackboardKey key))
            {
                key = new BlackboardKey(name);
                _keyRegistry[name] = key;
            }
            
            return key;
        }
        
        public bool ContainsKey(BlackboardKey key) => _entries.ContainsKey(key);
        public void RemoveKey(BlackboardKey key) => _entries.Remove(key);

        public IEnumerable<string> GetKeys()
        {
            return _keyRegistry.Keys;
        }
    }
}