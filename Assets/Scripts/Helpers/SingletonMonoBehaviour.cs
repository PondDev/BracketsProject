using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMonoBehaviour<Instance> : MonoBehaviour where Instance : SingletonMonoBehaviour<Instance>
{
    public static Instance instance;

    public virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as Instance;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}