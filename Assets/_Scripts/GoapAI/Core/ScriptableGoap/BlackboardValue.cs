using System;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace GOAP.ScriptableGoap
{
    public enum BlackboardValueType
    {
        Bool, Int, Float, Vector3, Transform, GameObject
    }
    
    [Serializable]
    [HideReferenceObjectPicker]
    [InlineProperty]
    public abstract class BlackboardValue
    {
        public abstract Type ValueType { get; }
    }

    [Serializable]
    public abstract class BlackboardValue<T> : BlackboardValue
    {
        [SerializeField]
        [LabelText("@\"Value (\" + niceName.ToLower() + \")\"")]
        private T _value;
        public T Value => _value;
        
        protected BlackboardValue(T value) => _value = value;
        public override Type ValueType => typeof(T);
        private string niceName => ValueType.GetNiceName();
    }
    
    [Serializable]
    public sealed class BoolValue : BlackboardValue<bool>
    {
        public BoolValue(bool value) : base(value) { }
    }
    
    [Serializable]
    public sealed class IntValue : BlackboardValue<int>
    {
        public IntValue(int value) : base(value) { }
    }

    [Serializable]
    public sealed class FloatValue : BlackboardValue<float>
    {
        public FloatValue(float value) : base(value) { }
    }

    [Serializable]
    public sealed class Vector3Value : BlackboardValue<Vector3>
    {
        public Vector3Value(Vector3 value) : base(value) { }
    }

    [Serializable]
    public sealed class TransformValue : BlackboardValue<Transform>
    {
        public TransformValue(Transform value) : base(value) { }
    }

    [Serializable]
    public sealed class GameObjectValue : BlackboardValue<GameObject>
    {
        public GameObjectValue(GameObject value) : base(value) { }
    }
    
}