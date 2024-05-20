using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RL.Dev.AI
{

    public class RMSL_BehaviorTreeRunner : MonoBehaviour
    {

        [SerializeField] protected RMSL_BehaviorTreeSaveData aiData;

        protected RMSL_BehaviorTree behaviorTree;

        protected virtual void Awake()
        {

            behaviorTree = Instantiate(aiData.behaviorTree);

        }

        protected virtual void Start()
        {

            behaviorTree.rootNode = behaviorTree.rootNode.Copy();
            behaviorTree.rootNode.Init(transform);

        }

        private void Update()
        {

            behaviorTree.rootNode.Execute();

        }

    }
}


