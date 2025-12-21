using GOAP.Runtime;
using GOAP.Scriptable;
using GOAP.Unity;

namespace GOAP.Scriptable.Actions
{
    public abstract class BaseScriptableActionStrategy : IActionStrategy
    {
        public abstract bool Complete { get; }
        public abstract bool CanPerform { get; }
        
        protected BlackBoard blackBoard;
        
        public BaseScriptableActionStrategy Init(BlackBoard blackBoard)
        {
            this.blackBoard = blackBoard;
            return this;
        }

        public abstract void Start();
        public abstract void Update(float deltaTime);
        public abstract void Stop();
    }
}