using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RL.Core
{
    [System.Serializable]
    public class RMSL_scenePoolType
    {
        public string sceneName;
        public List<RMSL_poolingObjType> scenePoolingList;
    }


    [System.Serializable]
    public class RMSL_poolingObjType
    {
        public string poolObjKey;
        public GameObject poolObj;
        public int poolObjCount; 
    }

    public class RMSL_PoolObj
    {
        public Object poolingObj;
        public List<GameObject> poolList;

        public RMSL_PoolObj(Object poolingObj, List<GameObject> poolList)
        {
            this.poolingObj = poolingObj;
            this.poolList = poolList;
        }
    }
}

