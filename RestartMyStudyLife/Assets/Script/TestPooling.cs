using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RL.Dev;

public class TestPooling : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {

            var obj = RMSL.TakePool("Testobj");
            RMSL.InsertPool(obj);
        }

    }
}
