using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoGravZone : MonoBehaviour
{
    GravitySource planet;
    public float internalGravity;
    float srcGravity;

    void Start()
    {
        SceneData data = GameObject.FindObjectOfType<SceneData>();
        planet = data.planet;
        srcGravity = planet.gravity;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("ping");
            planet.gravity = internalGravity;
        }
    }

        void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            planet.gravity = srcGravity;
        }
    }
}
