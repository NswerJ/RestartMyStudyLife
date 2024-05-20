using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RL.Dev.AI
{
    public class RMSL_BehaviorTreeSaveData : ScriptableObject
    {

        [HideInInspector] public RMSL_BehaviorTree behaviorTree;
        [HideInInspector] public List<RMSL_ConnectData> connectData = new List<RMSL_ConnectData>();

    }
}


