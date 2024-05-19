using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace RL.Core
{

    public class RMSL_Core : MonoBehaviour
    {

        private static RMSL_PoolManager poolManager;
        private static RMSL_Core instance;

        public static RMSL_PoolManager PoolManager
        {

            get
            {
                Init();
                return poolManager;

            }
        }
        public static RMSL_Core Instance
        {
            get
            {

                Init();
                return instance;

            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {

            if (instance == null)
            {

                GameObject go = new GameObject("_@*RMSL_CORE*@_");
                DontDestroyOnLoad(go);

                instance = go.AddComponent<RMSL_Core>();

                var res = Resources.Load<RMSL_SettingSO>("RMSL/SettingSO");

                if (poolManager == null && res.usePooling)
                {

                    poolManager = new RMSL_PoolManager(res.poolingSO, go.transform);
                    SceneManager.sceneLoaded += CreateScenePool;

                }

                SceneManager.sceneLoaded += CreateSceneObj;

            }

        }

        private static void CreateScenePool(Scene scene, LoadSceneMode mode)
        {

            poolManager.CreateScenePool(scene.name);

        }

        private static void CreateSceneObj(Scene scene, LoadSceneMode mode)
        {

            GameObject go = new GameObject("_@*RMSL_SCENE*@_");

            if (poolManager != null)
            {

                poolManager.SetSceneParent(go.transform);

            }

        }

    }
}


