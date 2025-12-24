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
        
        Villager villager;
        [SerializeField] string HaveWoodKey = "Have Wood";
        
        
        public override void Start()
        {
            if (villager == null)
            {
                villager = blackBoard.Owner.GetComponent<Villager>();
            }

            if (villager.HasLog())
            {
                villager.DropLog();
                var key = blackBoard.GetOrRegisterKey(HaveWoodKey);
                blackBoard.SetValue(key,false);
                
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