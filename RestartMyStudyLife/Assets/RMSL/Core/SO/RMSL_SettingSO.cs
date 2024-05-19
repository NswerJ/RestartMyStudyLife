using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

namespace RL.Core
{

    public class RMSL_SettingSO : ScriptableObject
    {

        [HideInInspector] public bool usePooling;

        public RMSL_PoolingSO poolingSO;
    }

}
