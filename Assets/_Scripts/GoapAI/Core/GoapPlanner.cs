using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GOAP
{
    public class GoapPlanner : IGoapPlanner
    {
        public ActionPlan Plan(IGoapAgent agent, HashSet<IAgentGoal> goals, IAgentGoal mostRecentGoal = null)
        {
            // order goals by priority - descending
            List<IAgentGoal> orderedGoals = goals
                .Where(g => g.DesiredEffects.Any(b => !b.Evaluate()))
                .OrderByDescending(g => g == mostRecentGoal ? g.Priority - 0.01f : g.Priority)
                .ToList();

            // Try to solve each goal in order
            foreach (var goal in orderedGoals) {
                Node goalNode = new Node(null, null, goal.DesiredEffects, 0);
                
                // If we can find a path to the goal, return the plan
                if (FindPath(goalNode, agent.Actions)) {
                    // If the goalNode has no leaves and no action to perform try a different goal
                    if (goalNode.IsLeafDead) continue;
                    
                    Stack<IAgentAction> actionStack = new Stack<IAgentAction>();
                    while (goalNode.Leaves.Count > 0) {
                        var cheapestLeaf = goalNode.Leaves.OrderBy(leaf => leaf.Cost).First();
                        goalNode = cheapestLeaf;
                        actionStack.Push(cheapestLeaf.Action);
                    }
                    
                    return new ActionPlan(goal, actionStack, goalNode.Cost);
                }
            }
            
            Debug.LogWarning("No plan found");
            return null;
        }
        
        // TODO: Consider a more powerful search algorithm like A* or D*
        bool FindPath(Node parent, HashSet<IAgentAction> actions) {
            // Order actions by cost, ascending
            var orderedActions = actions.OrderBy(a => a.Cost);
            
            var requiredEffects = new HashSet<IAgentBelief>(parent.RequiredEffects);
            requiredEffects.RemoveWhere(b => b.Evaluate());
            
            if(requiredEffects.Count == 0) return true;
            
            foreach (var action in orderedActions) {
                if (action.Effects.Any(requiredEffects.Contains)) {
                    var newRequiredEffects = new HashSet<IAgentBelief>(requiredEffects);
                    newRequiredEffects.ExceptWith(action.Effects);
                    newRequiredEffects.UnionWith(action.Preconditions);
                    
                    var newAvailableActions = new HashSet<IAgentAction>(actions);
                    newAvailableActions.Remove(action);
                    
                    var newNode = new Node(parent, action, newRequiredEffects, parent.Cost + action.Cost);
                    
                    // Explore the new node recursively
                    if (FindPath(newNode, newAvailableActions)) {
                        parent.Leaves.Add(newNode);
                        newRequiredEffects.ExceptWith(newNode.Action.Preconditions);
                    }
                    
                    // If all effects at this depth have been satisfied, return true
                    if (newRequiredEffects.Count == 0) {
                        return true;
                    }
                }
            }
            
            return parent.Leaves.Count > 0;
        }
    }
}