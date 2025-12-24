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
        
        [SerializeField] private string TargetTreeKeyName = "Target Tree";
        private Villager villager;
        
        public override void Start()
        {
            if (villager == null)
            {
                villager = blackBoard.Owner.GetComponent<Villager>();
            }
            
            if (villager.HasLog()) return;
            
            if (TreeManager.Instance.TryGetTree(out var tree))
            {
                villager.TargetTree = tree;
                var key = blackBoard.GetOrRegisterKey(TargetTreeKeyName);
                blackBoard.SetValue<Transform>(key, tree.transform);
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