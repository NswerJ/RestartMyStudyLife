using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager<T> : MonoBehaviour where T : MonoBehaviour
{
    public static ObjectPoolManager<T> Instance;

    private Queue<T> poolQueue = new Queue<T>();

    public float maxPoolSize = 100;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void Pool(T poolObj, int poolSize)
    {
        for (int i = 0; i < poolSize; i++)
        {
            T obj = Instantiate(poolObj, Vector3.zero, Quaternion.identity);
            obj.gameObject.SetActive(false);
            poolQueue.Enqueue(obj);
        }
    }

    public void ReturnToPool(T obj)
    {
        if (poolQueue.Count >= maxPoolSize)
        {
            Destroy(obj.gameObject);
        }
        else
        {
            obj.gameObject.SetActive(false);
            poolQueue.Enqueue(obj);
        }
    }

    public T PopPool(T popObj, Vector3 pos)
    {
        if (poolQueue.Count == 0)
        {
            Pool(popObj, 10);
        }
        T obj = poolQueue.Dequeue();
        obj.transform.position = pos;
        obj.gameObject.SetActive(true);
        return obj;
    }
}
