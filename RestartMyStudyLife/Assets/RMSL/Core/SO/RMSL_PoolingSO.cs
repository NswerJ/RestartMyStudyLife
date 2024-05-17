using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RL.Core
{
    public class RMSL_PoolingSO : ScriptableObject
    {
        [Header("������Ʈ Ǯ")]
        public List<RMSL_poolingObjType> objPooling;
        [Header("Ư�� ������ ����� Ǯ��")]
        public List<RMSL_scenePoolType> scenePooling;
    }
} 


