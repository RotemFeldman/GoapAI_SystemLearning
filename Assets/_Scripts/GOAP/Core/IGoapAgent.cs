
using System.Collections.Generic;

namespace GOAP.Core
{
   public interface IGoapAgent
   {
       IAgentGoal CurrentGoal { get;}
       ActionPlan ActionPlan { get; }
       IAgentAction CurrentAction { get;  }
       Dictionary<string, IAgentBelief> Beliefs { get;  }
       HashSet<IAgentAction> Actions { get; }
       HashSet<IAgentGoal> Goals { get;  }
   }
}


