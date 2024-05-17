using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RL.Core
{
    public class RMSL_PoolingSO : ScriptableObject
    {
        [Header("오브젝트 풀")]
        public List<RMSL_poolingObjType> objPooling;
        [Header("특정 씬에서 사용할 풀링")]
        public List<RMSL_scenePoolType> scenePooling;
    }
} 


