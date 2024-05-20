using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Metadata;

namespace RL.Dev.AI
{
    public class RMSL_SequenceNode : RMSL_CompositeNode
    {

        protected int count;

        protected override void Enable()
        {

            count = 0;

        }

        protected override RMSL_NodeState OnExecute()
        {

            var state = childrens[count].Execute();

            if (state == RMSL_NodeState.Success)
            {
                count++;

                if (count == childrens.Count) return RMSL_NodeState.Success;
                else return RMSL_NodeState.Running;

            }

            return state;

        }

    }


}
