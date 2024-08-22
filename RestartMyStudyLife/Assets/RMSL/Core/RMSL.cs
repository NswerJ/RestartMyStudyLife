using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RL.Core;
using System;

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


        #region DelayInvoke

        public static void InvokeDelay(Action action, float delay)
        {

            RMSL_Core.DelayInvoke.InvokeDelay(action, delay);

        }
        public static void InvokeDelayRealTime(Action action, float delay)
        {

            RMSL_Core.DelayInvoke.InvokeDelayRealTime(action, delay);

        }

        #endregion

        #region CustomCoroutine

        public static Coroutine StartCoroutine(IEnumerator routine)
        {
            return RMSL_Core.Instance.StartCoroutine(routine);
        }

        public static void StopCoroutine(Coroutine coroutine)
        {
            RMSL_Core.Instance.StopCoroutine(coroutine);
        }


        public static void StopCoroutine(IEnumerator routine)
        {
            RMSL_Core.CoroutineManager.StopCustomCoroutine(routine);
        }

        public static void StopAllCoroutines()
        {
            RMSL_Core.CoroutineManager.StopAllCustomCoroutines();
        }

        #endregion
    }
}


