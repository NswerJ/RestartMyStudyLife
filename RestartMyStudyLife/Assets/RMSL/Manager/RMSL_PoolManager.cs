using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace RL.Core
{
    public class RMSL_PoolManager 
    {
        private Dictionary<string, RMSL_PoolObj> ObjPoolContainer = new Dictionary<string, RMSL_PoolObj> ();
        private Dictionary<string, RMSL_PoolObj> scenePoolContainer = new Dictionary<string, RMSL_PoolObj> ();
        private RMSL_PoolingSO poolingSO;
        private Transform parent;
        private Transform sceneParent;

        public RMSL_PoolManager(RMSL_PoolingSO poolingSO, Transform parent)
        {
            this.poolingSO = poolingSO;
            this.parent = parent;

            for(int i = 0; i < poolingSO.objPooling.Count; i++)
            {
                var key = poolingSO.objPooling[i].poolObjKey;
                var poolingList = poolingSO.objPooling[i];

                List<GameObject> objQ = CreatePoolingList(poolingList.poolObjCount, key, poolingList.poolObj, parent);


                if (ObjPoolContainer.ContainsKey(key))
                {

                    Debug.LogError($"Please check key duplication : keyName {key}");
                    continue;

                }

                ObjPoolContainer.Add(key, new RMSL_PoolObj(poolingList.poolObj, objQ));
            }

           
        }

        private List<GameObject> CreatePoolingList(int poolCt, string key, GameObject poolObj, Transform parent)
        {

            List<GameObject> objQ = new List<GameObject>();

            for (int j = 0; j < poolCt; j++)
            {

                var obj = UnityEngine.Object.Instantiate(poolObj, parent);
                obj.gameObject.name = key;
                obj.SetActive(false);
                objQ.Add(obj);

            }

            return objQ;

        }


        public void CreateScenePool(string sceneName)
        {

            Debug.Log(sceneName);

            var pool = poolingSO.scenePooling.Find(x => x.sceneName == sceneName);

            if (pool == null) return;

            foreach (var item in scenePoolContainer)
            {

                foreach (var obj in item.Value.poolList)
                {

                    UnityEngine.Object.Destroy(obj);

                }

            }

            scenePoolContainer = new Dictionary<string, RMSL_PoolObj>();

            foreach (var obj in pool.scenePoolingList)
            {

                var key = obj.poolObjKey;

                List<GameObject> objQ = CreatePoolingList(obj.poolObjCount, key, obj.poolObj, parent);

                if (scenePoolContainer.ContainsKey(key))
                {

                    Debug.LogError($"Please check key duplication : keyName {key}");
                    continue;

                }

                scenePoolContainer.Add(key, new RMSL_PoolObj(obj.poolObj, objQ));

            }

        }
    }
}


