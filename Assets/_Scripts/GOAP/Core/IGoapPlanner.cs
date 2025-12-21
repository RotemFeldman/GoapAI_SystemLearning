using System.Collections.Generic;

namespace GOAP.Core
{
    public interface IGoapPlanner
    {
        ActionPlan Plan(IGoapAgent agent, HashSet<IAgentGoal> goals, IAgentGoal mostRecentGoal = null);
    }

    public class Node {
        public Node Parent { get; }
        public IAgentAction Action { get; }
        public HashSet<BeliefState> RequiredEffects { get; }
        public List<Node> Leaves { get; }
        public float Cost { get; }
        
        public bool IsLeafDead => Leaves.Count == 0 && Action == null;
        
        public Node(Node parent, IAgentAction action, HashSet<BeliefState> effects, float cost) {
            Parent = parent;
            Action = action;
            RequiredEffects = new HashSet<BeliefState>(effects);
            Leaves = new List<Node>();
            Cost = cost;
        }
    }

    public class ActionPlan
    {
        public IAgentGoal AgentGoal { get;}
        public Stack<IAgentAction> Actions { get;}
        public float TotalCost { get;}
        
        public ActionPlan(IAgentGoal agentGoal, Stack<IAgentAction> actions, float totalCost)
        {
            AgentGoal = agentGoal;
            Actions = actions;
            TotalCost = totalCost;
        }
    }
}