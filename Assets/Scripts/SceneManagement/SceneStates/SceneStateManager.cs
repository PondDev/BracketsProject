using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
TODO
Implement a write to json so we can properly save and load the game
*/

public class SceneStateManager : PersistentMonoBehaviour
{
    #region Singleton
    public static SceneStateManager Instance;
    protected override void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            base.Awake();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    int currentScene = 0;

    //complete data storage
    Dictionary<int, SceneData> sceneData = new Dictionary<int, SceneData>();

    Dictionary<int, SceneObject> additionDict = new Dictionary<int, SceneObject>();
    Dictionary<float, SceneObject> transformDict = new Dictionary<float, SceneObject>();
    List<float> deletionDict = new List<float>();

    #region Obj Recording Functions

    //Clone obj functions
    public void RecordAddition(int instanceId, SceneObject obj)
    {
        additionDict.Add(instanceId, obj);
    }

    public void RecordDeletion(int instanceId)
    {
        //if the object exists in the additions this state list, remove it.
        //shouldn't need safety as a deleted clone will always have been instantiated
        additionDict.Remove(instanceId);
    }

    //Base obj functions
    public void RecordTransform(float objectId, SceneObject obj)
    {
        if (!transformDict.ContainsKey(objectId))
        {
            transformDict.Add(objectId, obj);
        }
    }

    public void RecordDeletion(float objectId)
    {
        //remove if it exists in transform list
        transformDict.Remove(objectId);
        deletionDict.Add(objectId);
    }
    #endregion

    public override void OnNewScene()
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;
        if (!sceneData.ContainsKey(currentScene))
        {
            sceneData.Add(currentScene, new SceneData(currentScene, new Dictionary<float, SceneObject.ObjectData>(), new List<SceneObject.ObjectData>()));
        }

        
        //this kind of sucks but it works for now
        SceneObject[] objs = FindObjectsOfType<SceneObject>();
        Dictionary<float, SceneObject> tempSceneObjs = new Dictionary<float, SceneObject>();
        foreach (SceneObject obj in objs)
        {
            tempSceneObjs.Add(obj.objectId, obj);
        }

        foreach (KeyValuePair<float, SceneObject.ObjectData> data in sceneData[currentScene].objects)
        {
            switch (data.Value.type)
            {
                case SceneObject.ChangeType.Transform:
                    //if its a transform, apply the transform to the pre-existing object
                    tempSceneObjs[data.Key].ApplyTransform(data.Value);
                    break;
                case SceneObject.ChangeType.Deletion:
                    //if its a deletion, destroy the existing object
                    Destroy(tempSceneObjs[data.Key].gameObject);
                    break;
            }
        }
        
        foreach(SceneObject.ObjectData data in sceneData[currentScene].additionObjects)
        {
            //if its an addition, instantiate the prefab and apply transform
            GameObject obj = Instantiate(data.prefab, data.position, data.rotation);
            obj.GetComponent<SceneObject>().InitInstance(data.prefab);
        }
    }

    public void OnSceneUnload()
    {
        //prepare scene object data

        List<SceneObject.ObjectData> additionList = new List<SceneObject.ObjectData>();
        foreach (KeyValuePair<int, SceneObject> entry in additionDict)
        {
            SceneObject.ObjectData data = entry.Value.UpdateAdditionData();
            additionList.Add(data);
        }

        Dictionary<float, SceneObject.ObjectData> sceneObjectData = new Dictionary<float, SceneObject.ObjectData>(sceneData[currentScene].objects);
        foreach (KeyValuePair<float, SceneObject> entry in transformDict)
        {
            SceneObject.ObjectData data = entry.Value.UpdateTransformData();
            if (sceneObjectData.ContainsKey(entry.Key))
            {
                sceneObjectData.Remove(entry.Key);
            }
            sceneObjectData.Add(entry.Key, data);
        }

        foreach (float id in deletionDict)
        {
            SceneObject.ObjectData data = new SceneObject.ObjectData(){type = SceneObject.ChangeType.Deletion};
            if (sceneObjectData.ContainsKey(id))
            {
                sceneObjectData.Remove(id);
            }
            sceneObjectData.Add(id, data);
        }

        //replace saved scene data with updated version
        sceneData[currentScene] = new SceneData(currentScene, sceneObjectData, additionList);

        additionDict.Clear();
        transformDict.Clear();
        deletionDict.Clear();
    }

    struct SceneData
    {
        public int sceneId;
        public Dictionary<float, SceneObject.ObjectData> objects;
        public List<SceneObject.ObjectData> additionObjects;

        public SceneData(int _sceneId, Dictionary<float, SceneObject.ObjectData> _objects, List<SceneObject.ObjectData> _additionObjects)
        {
            sceneId = _sceneId;
            objects = _objects;
            additionObjects =_additionObjects;
        }
    }
}
