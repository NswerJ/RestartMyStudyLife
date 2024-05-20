using RL.Dev.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Metadata;

namespace RL.Dev.AI
{
    public class RMSL_SelectorNode : RMSL_CompositeNode
    {

        private int count;

        protected override void Enable()
        {

            count = 0;

        }

        protected override RMSL_NodeState OnExecute()
        {

            var state = childrens[count].Execute();

            if (state == RMSL_NodeState.Failure)
            {
                count++;

                if (count == childrens.Count) return RMSL_NodeState.Failure;
                else return RMSL_NodeState.Running;

            }

            return state;

        }

    }

}
