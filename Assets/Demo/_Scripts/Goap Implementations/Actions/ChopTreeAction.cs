using Demo._Scripts;
using GOAP.Scriptable.Actions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Demo.Actions
{
    [LabelText("Chop Tree")]
    public class ChopTreeAction : BaseScriptableActionStrategy
    {
        public override bool Complete => _complete;
        public override bool CanPerform => true;
        
        [SerializeField] Villager villager;
        private TreeComponent tree;

        [SerializeField] private float timeBetweenChops = 1f;
        private float timeSinceLastChop = 0f;

        private bool _complete = false;
        
        public override void Start()
        {
            _complete = false;
            if(villager == null) return;
            
            tree = villager.TargetTree;
        }

        public override void Update(float deltaTime)
        {
            timeSinceLastChop += deltaTime;
            if (timeSinceLastChop >= timeBetweenChops)
            {
                timeSinceLastChop = 0f;
                if (tree.TryChop())
                {
                    villager.PickupLog();
                    villager.TargetTree = null;
                    _complete = true;
                }
            }
        }

        public override void Stop()
        {
           
        }
    }
}