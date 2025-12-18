using GOAP;
using GOAP.Actions;
using UnityEngine;

namespace _Scripts.GoapAI
{
    public class TestAgent : MonoAgent
    {
        protected override void SetupBeliefs()
        {
            base.SetupBeliefs();
            beliefFactory.AddBelief("Happy", () => false);
            beliefFactory.AddBelief("Happy1", () => false);
        }

        protected override void SetupActions()
        {
            Actions.Add(new AgentAction.Builder("Idle")
                .WithCost(1)
                .WithEffect(Beliefs["Happy"])
                .WithStrategy(new IdleStrategy(5))
                .Build());

            Actions.Add(new AgentAction.Builder("Move")
                .WithCost(1)
                .WithEffect(Beliefs["Happy1"])
                .WithStrategy(new MoveStrategy(5))
                .Build());
        }

        protected override void SetupGoals()
        {
            Goals.Add(new AgentGoal.Builder("BeHappy")
                .AddDesiredEffect(Beliefs["Happy"])
                .Build());
            
            Goals.Add(new AgentGoal.Builder("BeHappy1")
                .AddDesiredEffect(Beliefs["Happy1"])
                .Build());

        }
    }
}