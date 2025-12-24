using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using GOAP.Core;
using GOAP.Runtime;


namespace GOAP.Unity
{
    public class MonoAgent : SerializedMonoBehaviour , IGoapAgent
    {
        public IAgentGoal CurrentGoal { get; private set; }
        public ActionPlan ActionPlan { get; private set; }
        public IAgentAction CurrentAction { get; private set; }

        public Dictionary<string, IAgentBelief> Beliefs { get; private set; } = new();
        public HashSet<IAgentAction> Actions { get; private set; } = new();
        public HashSet<IAgentGoal> Goals { get; private set; } = new();
        
        private GoapPlanner _planner;
        private IAgentGoal _lastGoal;
        private BeliefFactory _beliefFactory;

        protected virtual void Awake()
        {
            InitializeAgent();
        }

        protected virtual void Update()
        {
            if (CurrentAction == null)
            {
                CalculateGoapPlan();

                if (ActionPlan != null && ActionPlan.Actions.Count > 0)
                {
                    OnNewActionStarted();
                    CurrentGoal = ActionPlan.AgentGoal;
                    CurrentAction = ActionPlan.Actions.Pop();

                    if (CurrentAction.Preconditions.All(b => b.IsSatisfied())) // make sure all conditions are still met
                    {
                        CurrentAction.Start();
                    }
                    else
                    {
                        // force new plan calculation
                        CurrentAction = null;
                        CurrentGoal = null;
                    }
                }
            }

            if (CurrentAction != null && CurrentGoal != null)
            {
                CurrentAction.Update(Time.deltaTime);

                if (CurrentAction.Complete)
                {
                    CurrentAction.Stop();
                    CurrentAction = null;

                    if (ActionPlan.Actions.Count == 0)
                    {
                        _lastGoal = CurrentGoal;
                        CurrentGoal = null;
                    }
                }
            }
        }

        private void CalculateGoapPlan()
        {
            {
                // If we already have a plan and an active action, 
                // only look for goals that are HIGHER priority than what we are doing.
                if (CurrentAction != null && CurrentGoal != null)
                {
                    var higherPrioGoals = new HashSet<IAgentGoal>(Goals.Where(g => g.Priority > CurrentGoal.Priority));
                    if (higherPrioGoals.Count == 0) return; // Stay on current plan

                    var newPlan = _planner.Plan(this, higherPrioGoals, _lastGoal);
                    if (newPlan != null)
                    {
                        // Stop current action to switch to the better one
                        CurrentAction.Stop();
                        CurrentAction = null;
                        ActionPlan = newPlan;
                    }
                }
                else
                {
                    // We are idle, look at all goals
                    var newPlan = _planner.Plan(this, Goals, _lastGoal);
                    if (newPlan != null)
                    {
                        ActionPlan = newPlan;
                    }
                }
            }
        }

        private void InitializeAgent()
        {
            _planner = new GoapPlanner();
            
            /*Beliefs = new Dictionary<string, IAgentBelief>();
            Actions = new HashSet<IAgentAction>();
            Goals = new HashSet<IAgentGoal>();*/
            
            _beliefFactory = new BeliefFactory(this, Beliefs);
            
            SetupBeliefs();
            SetupActions();
            SetupGoals();
        }
        
        protected virtual void SetupBeliefs()
        {
            _beliefFactory.AddBelief("Nothing", () => false);
        }
        protected virtual void SetupActions()
        {
            
        }
        protected virtual void SetupGoals()
        {
            
        }

        protected virtual void OnNewActionStarted()
        {
            
        }
    }
}