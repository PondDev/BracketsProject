using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleBehaviour : MonoBehaviour
{
    public int id;

    public Material currentMat;

    [System.NonSerialized]
    public Sample sample;
    [System.NonSerialized]
    public bool fresh = false;
    public float freshTimer = 2f;

    void Start()
    {
        currentMat = GetComponentInChildren<MeshRenderer>().material;
        sample = SampleDatabase.Instance.GetSampleByID(id);
    }

    private void Update()
    {
        if (fresh)
        {
            freshTimer -= Time.deltaTime;
            if (freshTimer <= 0)
            {
                fresh = false;
            }
        }
    }

    public static GameObject SpawnInWorld(Sample sample, Vector3 pos)
    {
        GameObject obj = Instantiate(sample.worldPrefab, pos, Quaternion.identity);
        obj.GetComponent<SceneObject>().InitInstance(sample.worldPrefab);
        return obj;
    }
}