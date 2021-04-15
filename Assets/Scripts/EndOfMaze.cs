using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfMaze : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TempTasksManager.instance.UpdateCurrentTask(6);
        }
    }
}
