using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;

namespace GOAP
{
    public class MonoAgent : SerializedMonoBehaviour , IGoapAgent
    {
        public IAgentGoal CurrentGoal { get; private set; }
        public ActionPlan ActionPlan { get; private set; }
        public IAgentAction CurrentAction { get; private set; }
        
        public Dictionary<string, IAgentBelief> Beliefs { get; private set; }
        public HashSet<IAgentAction> Actions { get; private set; }
        public HashSet<IAgentGoal> Goals { get; private set; }
        
        private GoapPlanner _planner;
        private IAgentGoal _lastGoal;
        protected BeliefFactory beliefFactory;

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

                    if (CurrentAction.Preconditions.All(b => b.Evaluate())) // make sure all conditions are still met
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
            var prio = CurrentGoal?.Priority ?? 0;

            HashSet<IAgentGoal> legitGoals = Goals;

            if (CurrentGoal != null)
            {
                legitGoals = new HashSet<IAgentGoal>(Goals.Where(g => g.Priority > prio));
            }
            
            var newPlan = _planner.Plan(this, legitGoals, _lastGoal);
            if (newPlan != null)
            {
                ActionPlan = newPlan;
            }
        }

        private void InitializeAgent()
        {
            _planner = new GoapPlanner();
            
            Beliefs = new Dictionary<string, IAgentBelief>();
            Actions = new HashSet<IAgentAction>();
            Goals = new HashSet<IAgentGoal>();
            
            beliefFactory = new BeliefFactory(this, Beliefs);
            
            SetupBeliefs();
            SetupActions();
            SetupGoals();
        }
        
        protected virtual void SetupBeliefs()
        {
            beliefFactory.AddBelief("Nothing", () => false);
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