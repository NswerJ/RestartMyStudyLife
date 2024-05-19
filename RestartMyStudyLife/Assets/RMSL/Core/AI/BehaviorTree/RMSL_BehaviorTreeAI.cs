using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace RL.Dev.AI
{

    public enum RMSL_NodeState
    {

        Success,
        Failure,
        Running

    }

    public abstract class RMSL_Node : ScriptableObject
    {

        [HideInInspector] public GUID guid;
        [HideInInspector] public Vector2 editorPos;
        [HideInInspector] public Type currentObjectType;


        public abstract RMSL_NodeState Execute();
        public virtual void Init() { }
        public virtual void Kill() { }

    }

    public abstract class RMSL_ControlFlowNode : RMSL_Node
    {

        public List<RMSL_Node> childrens = new List<RMSL_Node>();

    }

    public abstract class RMSL_DecoratorNode : RMSL_Node
    {

        public RMSL_Node children;

    }


}
