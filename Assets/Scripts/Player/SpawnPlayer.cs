using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    static bool flip = true;
    public GameObject playerPrefab;
    void Awake()
    {
        if (flip)
        {
            Instantiate(playerPrefab);
            flip = false;
        }
    }
}
