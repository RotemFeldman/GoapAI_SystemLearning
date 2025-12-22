using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sisus.Init;
using BlackboardSystem;
using GOAP.Scriptable;
using Sirenix.Serialization;
using UnityEditor;
using UnityEngine;

namespace GOAP.Unity
{
    [HideMonoScript]
    [Service(typeof(IBlackboardKeyProvider))]
    public class BlackBoardComponent : SerializedMonoBehaviour, IBlackboardKeyProvider
    {
        [Title("New"),PropertyOrder(-1)]
        [Button("Add Key")]
        private void AddKey()
        {
            entries.Add(new BlackboardEntryData());
        }
        
        [Title("Runtime Blackboard")]
        [ShowInInspector,ReadOnly,HideInEditorMode]
        [SerializeField] private Blackboard blackboard = new();
        public Blackboard Blackboard => blackboard;
        
        
        [Title("Blackboard Values")]
        [OdinSerialize,HideInPlayMode,InlineProperty,HideLabel,HideReferenceObjectPicker]
        private List<BlackboardEntryData> entries = new();

        private void Awake()
        {
            SetValuesOnBlackboard(blackboard);
        }

        public void SetValuesOnBlackboard(Blackboard blackboard)
        {
            foreach (var entry in entries)
            {
                entry.SetValueOnBlackboard(blackboard);
            }
        }

        public IEnumerable<string> GetKeys<T>()
        {
            foreach (var entry in entries)
            {
                yield return entry.keyName;
            }
        }

        [Serializable]
        public class BlackboardEntryData : ISerializationCallbackReceiver
        {
            [HorizontalGroup,HideLabel]
            public string keyName;
            [HorizontalGroup(Width = 100),HideLabel,OnValueChanged("OnTypeValueChanged")]
            [OnInspectorGUI("BeginDrawEnum","EndDrawEnum")]
            public AnyValue.ValueType valueType;
            [HorizontalGroup,HideLabel]
            public AnyValue value;

            private static Dictionary<AnyValue.ValueType, Action<Blackboard, BlackboardKey, AnyValue>>
                setValueDispatchTable = new()
                {
                    { AnyValue.ValueType.Int, (blackboard, key, anyValue) => blackboard.SetVlaue<int>(key, anyValue) },
                    { AnyValue.ValueType.Float, (blackboard, key, anyValue) => blackboard.SetVlaue<float>(key, anyValue) },
                    { AnyValue.ValueType.Bool, (blackboard, key, anyValue) => blackboard.SetVlaue<bool>(key, anyValue) },
                    { AnyValue.ValueType.String, (blackboard, key, anyValue) => blackboard.SetVlaue<string>(key, anyValue) },
                    { AnyValue.ValueType.Vector3, (blackboard, key, anyValue) => blackboard.SetVlaue<Vector3>(key, anyValue) },
                    { AnyValue.ValueType.Transform, (blackboard, key, anyValue) => blackboard.SetVlaue<Transform>(key, anyValue) },
                    { AnyValue.ValueType.GameObject, (blackboard, key, anyValue) => blackboard.SetVlaue<GameObject>(key, anyValue) },
                };

            public void SetValueOnBlackboard(Blackboard blackboard)
            {
                var key = blackboard.GetOrRegisterKey(keyName);
                if(string.IsNullOrWhiteSpace(keyName)) return;
                setValueDispatchTable[valueType](blackboard, key, value);
            }
            
            public void OnBeforeSerialize(){}
            public void OnAfterDeserialize() => value.type = valueType;
            
            private void OnTypeValueChanged() => value.type = valueType;

            private void BeginDrawEnum()
            {
                Color bgColor = GetEnumColorValue();
                Color textColor = Color.white;

                GUIStyle customEnumStyle = new GUIStyle(EditorStyles.popup);
                customEnumStyle.normal.textColor = textColor;
                customEnumStyle.hover.textColor = textColor;
                
                GUI.backgroundColor = bgColor;
            }
            
            private void EndDrawEnum() => GUI.backgroundColor = Color.white;

            private Color GetEnumColorValue()
            {
                return valueType switch
                {
                    AnyValue.ValueType.Bool => new Color(0.6f,0.2f,0.2f),
                    AnyValue.ValueType.Int => new Color(0.2f,0.4f,0.2f),
                    AnyValue.ValueType.Float => new Color(0.2f,0.9f,0.7f),
                    AnyValue.ValueType.String => new Color(0.2f,0.4f,0.6f),
                    AnyValue.ValueType.Vector3 => new Color(0.5f,0.5f,1f),
                    AnyValue.ValueType.Transform => new Color(1,0.5f,0),
                    AnyValue.ValueType.GameObject => new Color(0.8f,0.7f,0.2f),
                    _ => Color.white
                };
            }
        }
        
        [Serializable]
        public struct AnyValue
        {
            public enum ValueType { Bool, Int, Float, String, Vector3, Transform, GameObject }
            [HideInInspector]
            public ValueType type;
            
            [ShowIf(nameof(type), ValueType.Bool),HideLabel]
            public bool boolValue;
            [ShowIf(nameof(type), ValueType.Float),HideLabel]
            public float floatValue;
            [ShowIf(nameof(type), ValueType.Int),HideLabel]
            public int intValue;
            [ShowIf(nameof(type), ValueType.String),HideLabel]
            public string stringValue;
            [ShowIf(nameof(type), ValueType.Vector3),HideLabel]
            public Vector3 vector3Value;
            [ShowIf(nameof(type), ValueType.Transform),HideLabel]
            public Transform transformValue;
            [ShowIf(nameof(type), ValueType.GameObject),HideLabel]
            public GameObject gameObjectValue;
            
            public static implicit operator int(AnyValue value) => value.ConvertValue<int>();
            public static implicit operator float(AnyValue value) => value.ConvertValue<float>();
            public static implicit operator bool(AnyValue value) => value.ConvertValue<bool>();
            public static implicit operator string(AnyValue value) => value.ConvertValue<string>();
            public static implicit operator Vector3(AnyValue value) => value.ConvertValue<Vector3>();
            public static implicit operator Transform(AnyValue value) => value.ConvertValue<Transform>();
            public static implicit operator GameObject(AnyValue value) => value.ConvertValue<GameObject>();

            private T ConvertValue<T>()
            {
                return type switch
                {
                    ValueType.Bool => AsBool<T>(boolValue),
                    ValueType.Int => AsInt<T>(intValue),
                    ValueType.Float => AsFloat<T>(floatValue),
                    ValueType.String => (T)(object)stringValue,
                    ValueType.Vector3 => (T)(object)vector3Value,
                    ValueType.Transform => (T)(object)transformValue,
                    ValueType.GameObject => (T)(object)gameObjectValue,
                    _ => throw new NotSupportedException($"Not supported value type: {typeof(T)}")
                };
            }
            
            private T AsBool<T>(bool value) => typeof(T) == typeof(bool) && value is T correctType ? correctType : default;
            private T AsInt<T>(int value) => typeof(T) == typeof(int) && value is T correctType ? correctType : default;
            private T AsFloat<T>(float value) => typeof(T) == typeof(float) && value is T correctType ? correctType : default;
        }
    }
}