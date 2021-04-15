using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentMonoBehaviour : MonoBehaviour
{
    protected virtual void Awake()
    {
        DontDestroyOnLoad(gameObject);
        PersistenceManager.Instance.RegisterPersistent(this);
    }

    public virtual void OnNewScene() {}
}
