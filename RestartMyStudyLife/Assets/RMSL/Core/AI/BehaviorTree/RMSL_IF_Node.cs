using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Metadata;


namespace RL.Dev.AI
{
    public abstract class RMSL_IF_Node : RMSL_DecoratorNode
    {
        protected override RMSL_NodeState OnExecute()
        {

            if (Condition()) return children.Execute();
            else return RMSL_NodeState.Failure;

        }

        protected abstract bool Condition();

    }
}

