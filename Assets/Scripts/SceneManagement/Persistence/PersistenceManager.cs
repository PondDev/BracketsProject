using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistenceManager
{
    #region Singleton
    private static readonly PersistenceManager instance = new PersistenceManager();

    static PersistenceManager()
    {
        Activate();
    }

    ~PersistenceManager()
    {
        Deactivate();
    }
    
    public static PersistenceManager Instance
    {
        get {return instance;}
    }
    #endregion

    private HashSet<PersistentMonoBehaviour> persistents = new HashSet<PersistentMonoBehaviour>();

    //Detect Scene Load
    static void Activate()
    {
        SceneManager.sceneLoaded += Instance.OnLevelLoaded;
    }

    static void Deactivate()
    {
        SceneManager.sceneLoaded -= Instance.OnLevelLoaded;
    }
    
    //Calls before Start
    void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        foreach(PersistentMonoBehaviour behaviour in persistents)
        {
            behaviour.OnNewScene();
        }
    }

    public void RegisterPersistent(PersistentMonoBehaviour persistent)
    {
        persistents.Add(persistent);
    }
    public void UnregisterPersistent(PersistentMonoBehaviour persistent)
    {
        persistents.Remove(persistent);
    }
}
