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
        
        Villager villager;

        [SerializeField] private float timeBetweenChops = 1f;
        private float timeSinceLastChop = 0f;
        
        [SerializeField] string HaveWoodKey = "Have Wood";

        private bool _complete = false;
        
        public override void Start()
        {
            _complete = false;
            if (villager == null)
            {
                villager = blackBoard.Owner.GetComponent<Villager>();
            }
        }

        public override void Update(float deltaTime)
        {
            timeSinceLastChop += deltaTime;
            if (timeSinceLastChop >= timeBetweenChops)
            {
                timeSinceLastChop = 0f;
                if (villager.TargetTree.TryChop())
                {
                    villager.PickupLog();
                    var key = blackBoard.GetOrRegisterKey(HaveWoodKey);
                    blackBoard.SetValue(key,true);
                    
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