using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Metadata;


namespace RL.Dev.AI {
    public class RMSL_RepeatNode : RMSL_DecoratorNode
    {
        protected override RMSL_NodeState OnExecute()
        {

            children.Execute();

            return RMSL_NodeState.Running;
        }

    }
}

