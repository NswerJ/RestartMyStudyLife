using RL.Dev.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RL.Dev.AI
{
    public class RMSL_RootNode : RMSL_Node
    {

        public RMSL_Node children;

        public override void Init(Transform trm)
        {

            children.Init(trm);

        }

        protected override RMSL_NodeState OnExecute()
        {

            return children.Execute();

        }

        public override RMSL_Node Copy()
        {

            var node = Instantiate(this);
            node.children = children.Copy();

            return node;

        }

    }
}



