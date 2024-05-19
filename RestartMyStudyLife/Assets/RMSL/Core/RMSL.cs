using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RL.Core;

namespace RL.Dev
{
    public static class RMSL
    {

        #region Pooling

        public static void InsertPool(GameObject obj)
        {

            RMSL_Core.PoolManager.InsertPool(obj);

        }
        public static GameObject TakePool(string poolName)
        {

            return RMSL_Core.PoolManager.TakePool(poolName);

        }
        public static T TakePool<T>(string poolName)
        {

            return RMSL_Core.PoolManager.TakePool<T>(poolName);

        }
        public static GameObject TakePool(string poolName, Vector3 pos)
        {

            return RMSL_Core.PoolManager.TakePool(poolName, pos);

        }
        public static T TakePool<T>(string poolName, Vector3 pos)
        {

            return RMSL_Core.PoolManager.TakePool<T>(poolName, pos);

        }
        public static GameObject TakePool(string poolName, Vector3 pos, Quaternion rot)
        {

            return RMSL_Core.PoolManager.TakePool(poolName, pos, rot);

        }
        public static T TakePool<T>(string poolName, Vector3 pos, Quaternion rot)
        {

            return RMSL_Core.PoolManager.TakePool<T>(poolName, pos, rot);

        }
        public static GameObject TakePool(string poolName, Vector3 pos, Quaternion rot, Transform parent)
        {

            return RMSL_Core.PoolManager.TakePool(poolName, pos, rot, parent);

        }
        public static T TakePool<T>(string poolName, Vector3 pos, Quaternion rot, Transform parent)
        {

            return RMSL_Core.PoolManager.TakePool<T>(poolName, pos, rot, parent);

        }

        #endregion
    }
}


