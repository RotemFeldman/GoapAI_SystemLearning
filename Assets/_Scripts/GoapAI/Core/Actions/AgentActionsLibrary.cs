using UnityEngine;

namespace GOAP.Actions
{
    public class IdleStrategy : IActionStrategy
    {
        private float _time;
        private float _duration;
        
        public bool Complete => _time >= _duration;
        public bool CanPerform => true;
        
        public IdleStrategy(float duration)
        {
            _duration = duration;
            _time = 0;
        }
        
        public void Start()
        {
            _time = 0;
            Debug.Log("started Idle action");
        }

        public void Update(float deltaTime)
        {
            _time += deltaTime;
            Debug.Log("updating Idle action");
        }

        public void Stop()
        {
            Debug.Log("stop Idle action");   
        }
    }

    public class MoveStrategy : IActionStrategy
    {
        private float _time;
        private float _duration;
        
        public bool Complete => _time >= _duration;
        public bool CanPerform => true;
        
        public MoveStrategy(float duration)
        {
            _duration = duration;
            _time = 0;
        }
        
        public void Start()
        {
            _time = 0;
            Debug.Log("started Move action");
        }

        public void Update(float deltaTime)
        {
            _time += deltaTime;
            Debug.Log("updating Move action");
        }

        public void Stop()
        {
            Debug.Log("stop Move action");   
        }
    }
}