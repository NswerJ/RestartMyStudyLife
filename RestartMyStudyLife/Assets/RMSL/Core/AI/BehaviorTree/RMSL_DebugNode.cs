using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RL.Dev.AI
{
    public class RMSL_DebugNode : RMSL_ActionNode
    {

        public string debugText;

        protected override RMSL_NodeState OnExecute()
        {

            Debug.Log(debugText);

            return RMSL_NodeState.Failure;

        }
    }

}
