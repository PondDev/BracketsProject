using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempTasksManager : MonoBehaviour
{

    #region Singleton
    public static TempTasksManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    public GameObject top;
    public List<GameObject> tasks;
    public List<bool> tracker;
    int currentTask = 0;
    
    public void UpdateCurrentTask(int id)
    {
        if (id -1 == currentTask)
        {
            if (id == 6)
            {
                top.SetActive(false);
            }
            else if (!tracker[id])
            {
                tasks[currentTask].SetActive(false);
                currentTask = id;
                tasks[currentTask].SetActive(true);
                tracker[id] = true;
            }
        }
    }
}
