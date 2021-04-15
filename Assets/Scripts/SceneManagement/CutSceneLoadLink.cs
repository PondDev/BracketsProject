using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutSceneLoadLink : MonoBehaviour
{
    #region Singelton
    static CutSceneLoadLink _instance;
    public static CutSceneLoadLink instance
    {
        get 
        {
            return _instance;
        }
    }

    void Awake ()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } 
        else {
            _instance = this;
        }
    }
    #endregion

    int sceneToLoad;
    PlayableDirector director;
    public Camera cutsceneCam;

    void Start()
    {
        director = GetComponent<PlayableDirector>();
    }

    public void StartCutScene(int index, PlayerMaster player)
    {
        sceneToLoad = index;
        director.Play();

        
        cutsceneCam.enabled = true;

        player.DeActivatePlayer();
    }

    public void Load()
    {
        SceneLoader.instance.LoadLevel(sceneToLoad);
    }
}
