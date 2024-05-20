using RL.Dev.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RMSL_BehaviorTree : ScriptableObject
{
    [HideInInspector] public bool isMemory;

    public RMSL_Node rootNode;
    public List<RMSL_Node> nodes = new List<RMSL_Node>();
}
