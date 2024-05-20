using RL.Dev.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RL.Dev.AI
{
    public class RMSL_BehaviorTree : ScriptableObject
    {

        [HideInInspector] public List<RMSL_Node> nodes = new List<RMSL_Node>();
        [HideInInspector] public RMSL_Node rootNode;

        public void SettingRootNode()
        {

            rootNode = nodes.Find(x => x as RMSL_RootNode != null);

        }

    }

}
