using System;
using System.Collections.Generic;
using UnityEngine;

namespace RL.Core
{
    public class RMSL_PoolManager
    {
        private Dictionary<string, RMSL_PoolObj> ObjPoolContainer = new Dictionary<string, RMSL_PoolObj>();
        private Dictionary<string, RMSL_PoolObj> scenePoolContainer = new Dictionary<string, RMSL_PoolObj>();
        private RMSL_PoolingSO poolingSO;
        private Transform parent;
        private Transform sceneParent;

        public RMSL_PoolManager(RMSL_PoolingSO poolingSO, Transform parent)
        {
            this.poolingSO = poolingSO;
            this.parent = parent;

            for (int i = 0; i < poolingSO.objPooling.Count; i++)
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

        public void InsertPool(GameObject obj)
        {
            if (ObjPoolContainer.ContainsKey(obj.name))
            {
                ObjPoolContainer[obj.name].poolList.Add(obj);
                obj.transform.SetParent(parent);
                obj.SetActive(false);
            }
            else if (scenePoolContainer.ContainsKey(obj.name))
            {
                scenePoolContainer[obj.name].poolList.Add(obj);
                obj.transform.SetParent(parent);
                obj.SetActive(false);
            }
            else
            {
                Debug.LogWarning($"Pool named {obj.name} does not exist");
                UnityEngine.Object.Destroy(obj);
            }
        }

        public void SetSceneParent(Transform parent)
        {
            sceneParent = parent;
        }

        public GameObject TakePool(string key, Nullable<Vector3> pos = null, Nullable<Quaternion> rot = null, Transform parent = null)
        {
            if (pos == null) pos = new Vector3(0, 0, 0);
            if (rot == null) rot = Quaternion.identity;

            if (ObjPoolContainer.ContainsKey(key))
            {
                if (ObjPoolContainer[key].poolList.Count <= 0)
                {
                    var ins = UnityEngine.Object.Instantiate(ObjPoolContainer[key].poolingObj, (Vector3)pos, (Quaternion)rot, parent);
                    ins.name = key;
                    return ins;
                }

                var obj = ObjPoolContainer[key].poolList[0];
                ObjPoolContainer[key].poolList.RemoveAt(0);
                obj.SetActive(true);
                obj.transform.SetParent(sceneParent);
                obj.transform.SetParent(parent);
                obj.transform.position = (Vector3)pos;
                obj.transform.rotation = (Quaternion)rot;

                return obj;
            }
            else if (scenePoolContainer.ContainsKey(key))
            {
                if (scenePoolContainer[key].poolList.Count <= 0)
                {
                    var ins = UnityEngine.Object.Instantiate(scenePoolContainer[key].poolingObj, (Vector3)pos, (Quaternion)rot, parent);
                    ins.name = key;
                    return ins;
                }

                var obj = scenePoolContainer[key].poolList[0];
                scenePoolContainer[key].poolList.RemoveAt(0);
                obj.SetActive(true);
                obj.transform.SetParent(sceneParent);
                obj.transform.SetParent(parent);
                obj.transform.position = (Vector3)pos;
                obj.transform.rotation = (Quaternion)rot;

                return obj;
            }
            else
            {
                Debug.LogError($"Pool named {key} does not exist");
                return null;
            }
        }

        public T TakePool<T>(string key, Nullable<Vector3> pos = null, Nullable<Quaternion> rot = null, Transform parent = null)
        {
            return TakePool(key, pos, rot, parent).GetComponent<T>();
        }
    }
}
