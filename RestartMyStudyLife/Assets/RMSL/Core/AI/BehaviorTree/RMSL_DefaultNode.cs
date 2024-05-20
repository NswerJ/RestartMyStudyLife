using RL.Dev.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Metadata;

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

        [HideInInspector] public string guid;
        [HideInInspector] public Rect editorPos;

        protected RMSL_NodeState state;
        protected bool started;

        public RMSL_NodeState Execute()
        {

            if (!started)
            {

                Enable();
                started = true;

            }

            state = OnExecute();

            if (state == RMSL_NodeState.Failure || state == RMSL_NodeState.Success)
            {

                Disable();
                started = false;

            }

            return state;

        }

        public virtual RMSL_Node Copy()
        {

            return Instantiate(this);

        }
        public virtual void Init(Transform trm) { }

        public void Breaking()
        {

            Disable();
            started = false;

        }

        protected virtual void Enable() { }
        protected virtual void Disable() { }
        protected abstract RMSL_NodeState OnExecute();


    }

    public abstract class RMSL_ActionNode : RMSL_Node { }

    public abstract class RMSL_CompositeNode : RMSL_Node
    {

        [HideInInspector] public List<RMSL_Node> childrens = new List<RMSL_Node>();

        public override RMSL_Node Copy()
        {

            var node = Instantiate(this);




            node.childrens = new List<RMSL_Node>();

            foreach (var ch in childrens)
            {

                node.childrens.Add(ch.Copy());

            }

            return node;

        }

        public override void Init(Transform trm)
        {

            childrens.ForEach(x => x.Init(trm));

        }

    }

    public abstract class RMSL_DecoratorNode : RMSL_Node
    {

        [HideInInspector] public RMSL_Node children;

        public override void Init(Transform trm)
        {

            children.Init(trm);

        }

        public override RMSL_Node Copy()
        {

            var node = Instantiate(this);
            node.children = children.Copy();

            return node;

        }

    }

}