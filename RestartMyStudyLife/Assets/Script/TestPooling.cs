using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RL.Dev;
using System;

public class TestPooling : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(poolTest());
    }

    IEnumerator poolTest()
    {
        while (true)
        {
            var obj = RMSL.TakePool("TestObj", Vector3.zero, Quaternion.identity, transform);
            yield return new WaitForSeconds(1f);
            RMSL.InsertPool(obj);
            yield return new WaitForSeconds(1f);
        }
    }


    void Update()
    {




    }
}
