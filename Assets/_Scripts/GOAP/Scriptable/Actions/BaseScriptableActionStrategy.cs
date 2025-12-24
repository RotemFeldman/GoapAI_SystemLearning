using BlackboardSystem;
using GOAP.Runtime;
using GOAP.Scriptable;
using GOAP.Unity;
using UnityEngine;

namespace GOAP.Scriptable.Actions
{
    public abstract class BaseScriptableActionStrategy : IActionStrategy
    {
        public abstract bool Complete { get; }
        public abstract bool CanPerform { get; }
        
        protected Blackboard blackBoard;
        
        public BaseScriptableActionStrategy Init(Blackboard blackBoard)
        {
            this.blackBoard = blackBoard;
            return this;
        }

        public BaseScriptableActionStrategy Clone()
        {
            string json = JsonUtility.ToJson(this);
            return (BaseScriptableActionStrategy)JsonUtility.FromJson(json, GetType());
        } 

        public abstract void Start();
        public abstract void Update(float deltaTime);
        public abstract void Stop();
    }
}