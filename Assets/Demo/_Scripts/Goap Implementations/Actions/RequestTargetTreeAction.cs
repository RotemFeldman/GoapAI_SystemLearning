using BlackboardSystem;
using Demo._Scripts;
using GOAP.Scriptable;
using GOAP.Scriptable.Actions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Demo.Actions
{
    [LabelText("Request Target Tree")]
    public class RequestTargetTreeAction : BaseScriptableActionStrategy
    {
        public override bool Complete => true;
        public override bool CanPerform => true;

        [SerializeField] private Villager villager;
        
        public override void Start()
        {
            if (villager.HasLog()) return;
            
            if (TreeManager.Instance.TryGetTree(out var tree))
            {
                villager.TargetTree = tree;
            }
        }

        public override void Update(float deltaTime)
        {
            
        }

        public override void Stop()
        {
            
        }
    }
}