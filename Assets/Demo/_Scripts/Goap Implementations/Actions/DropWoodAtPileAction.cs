using Demo._Scripts;
using GOAP.Scriptable.Actions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Demo.Actions
{
    [LabelText("Drop Wood At Pile")]
    public class DropWoodAtPileAction : BaseScriptableActionStrategy
    {
        public override bool Complete => true;
        public override bool CanPerform => true;
        
        [SerializeField] Villager villager;
        
        public override void Start()
        {
            if (villager == null) return;

            if (villager.HasLog())
            {
                villager.DropLog();
                LogPile.Instance.AddLog();
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