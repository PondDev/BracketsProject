using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour
{
    void Awake()
    {
        //temp singleton init
        SampleDatabase db = SampleDatabase.Instance;
    }
}
