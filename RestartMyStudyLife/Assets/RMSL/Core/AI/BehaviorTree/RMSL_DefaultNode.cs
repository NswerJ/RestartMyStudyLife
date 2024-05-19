using RL.Dev.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Metadata;

namespace RL.Dev.AI
{

    public class RMSL_SequenceNode : RMSL_ControlFlowNode
    {

        private int count;

        public override void Init()
        {

            count = 0;

        }

        public override RMSL_NodeState Execute()
        {

            var state = childrens[count].Execute();
            count++;
            if (state == RMSL_NodeState.Failure) return state;

            if (count != childrens.Count) return RMSL_NodeState.Running;

            return RMSL_NodeState.Success;

        }

    }

    public class RMSL_RootNode : RMSL_Node
    {
        public RMSL_Node children;
        public override RMSL_NodeState Execute()
        {
            throw new System.NotImplementedException();
        }
    }

}