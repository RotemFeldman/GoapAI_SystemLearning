using Sirenix.OdinInspector;
using UnityEngine;

namespace GOAP.Scriptable.Actions
{
    [LabelText( "Wait")]
    public class WaitAction : BaseScriptableActionStrategy
    {
        public override bool Complete => _complete;
        public override bool CanPerform => true;

        [SerializeField] private BlackboardBind<float> _duration = new();
        
        private float _durationValue;
        private float _time;

        private bool _complete = false;
        
        public override void Start()
        {
            _complete = false;
            _time = 0;
            if(!_duration.TryGetValue(blackBoard, out _durationValue)) _durationValue = 0;
        }

        public override void Update(float deltaTime)
        {
            _time += deltaTime;
            if (_time >= _durationValue) _complete = true;
        }

        public override void Stop()
        {
            blackBoard.Set<bool>("HasEnergy", true);
        }
    }
}