using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpTracker : MonoBehaviour
{
    public GameObject target;

    // Update is called once per frame
    void Update()
    {
        transform.position = target.transform.position + target.transform.forward;
        //target.transform.rotation = target.transform.rotation;
    }
}
