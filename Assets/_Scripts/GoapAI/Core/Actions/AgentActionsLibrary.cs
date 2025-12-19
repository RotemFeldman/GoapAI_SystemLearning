using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace GOAP
{
    [Serializable]
    public class IdleStrategy : IActionStrategy
    {
        private float _time;
        [SerializeField] private float duration;
        
        public bool Complete => _time >= duration;
        public bool CanPerform => true;
        
        public IdleStrategy(){}
        
        public IdleStrategy(float duration)
        {
            this.duration = duration;
            _time = 0;
        }
        
        public void Start()
        {
            _time = 0;
        }

        public void Update(float deltaTime)
        {
            _time += deltaTime;
        }

        public void Stop()
        {
        }
    }
    
    [Serializable]
    public class MoveStrategy : IActionStrategy
    {
        [SerializeField] private Transform self; 
        [SerializeField] private Transform target; 
        
        [SerializeField] private float distanceThreshold = 0.1f;
        
        public bool Complete => Vector3.Distance(self.position, target.position) < distanceThreshold;
        public bool CanPerform => true;
        
        public MoveStrategy(){}
        
        public MoveStrategy(Transform self, Transform target, float distanceThreshold)
        {
            this.self = self;
            this.target = target;
            this.distanceThreshold = distanceThreshold;
        }
        
        public void Start()
        {
            Debug.Log("started Move action");
        }

        public void Update(float deltaTime)
        {
            self.position = Vector3.MoveTowards(self.position, target.position, deltaTime);
            Debug.Log("updating Move action");
        }

        public void Stop()
        {
            Debug.Log("stop Move action");   
        }
    }
}