using System.Collections;
using UnityEngine;

namespace RL.Core
{
    public class RMSL_Coroutine
    {
        private MonoBehaviour owner;

        public RMSL_Coroutine(MonoBehaviour owner)
        {
            this.owner = owner;
        }

        public Coroutine StartCustomCoroutine(IEnumerator routine)
        {
            return owner.StartCoroutine(routine);
        }

        public void StopCustomCoroutine(Coroutine coroutine)
        {
            if (coroutine != null)
            {
                owner.StopCoroutine(coroutine);
            }
        }

        public void StopCustomCoroutine(IEnumerator routine)
        {
            if (routine != null)
            {
                owner.StopCoroutine(routine);
            }
        }

        public void StopAllCustomCoroutines()
        {
            owner.StopAllCoroutines();
        }
    }
}
