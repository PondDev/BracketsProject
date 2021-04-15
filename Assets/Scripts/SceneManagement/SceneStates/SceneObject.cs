using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
There are 2 types of SceneObjects:
 1. Base object that exists in scene
 2. Instantiated object

Base objects are permanently recorded
Instantiated exist from scene to scene
*/
public class SceneObject : MonoBehaviour
{
    //register to manager
    //check change
    [System.NonSerialized]
    public float objectId;
    [System.NonSerialized]
    public ObjectData data;
    [System.NonSerialized]
    public GameObject trackedPrefab;
    [System.NonSerialized]
    public bool clone = false;

    void Awake()
    {
        objectId = GenerateId();
    }

    public void DestroySceneObj()
    {
        SetDeletion();
        Destroy(gameObject);
    }

    public void InitInstance(GameObject _prefab)
    {
        clone = true;
        objectId = GetInstanceID();
        trackedPrefab = _prefab;
        SetAddition();
    }

    protected float GenerateId()
    {
        return transform.position.sqrMagnitude;
    }

    //Register vs Manager
    //These are called from player interaction
    public void SetAddition()
    {
        SceneStateManager.Instance.RecordAddition(GetInstanceID(), this);
    }

    public void SetTransform()
    {
        //clones can't get transform events
        if (!clone)
        {
            SceneStateManager.Instance.RecordTransform(objectId, this);
        }   
    }

    public void SetDeletion()
    {
        if (clone)
        {
            SceneStateManager.Instance.RecordDeletion(GetInstanceID());
        }
        else
        {
            SceneStateManager.Instance.RecordDeletion(objectId);
        }
    }

    //Functions to update data before the write
    //These are called on scene unload from the manager
    public ObjectData UpdateAdditionData()
    {
        return new ObjectData()
        {
            type = ChangeType.Addition,
            position = transform.position,
            rotation = transform.rotation,
            scale = transform.localScale,
            prefab = trackedPrefab
        };
    }
    
    public ObjectData UpdateTransformData()
    {
        return new ObjectData()
        {
            type = ChangeType.Transform,
            position = transform.position,
            rotation = transform.rotation,
            scale = transform.localScale
        };
    }

    public ObjectData UpdateDeletionData()
    {
        return new ObjectData()
        {
            type = ChangeType.Deletion
        };
    }

    public void ApplyTransform(ObjectData data)
    {
        transform.position = data.position;
        transform.rotation = data.rotation;
        transform.localScale = data.scale;
    }

    public enum ChangeType
    {
        Transform,
        Addition,
        Deletion
    }

    public struct ObjectData
    {
        public ChangeType type;
        //Transform change
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;

        //addition change
        public GameObject prefab;
        //parenting?
    }
}
