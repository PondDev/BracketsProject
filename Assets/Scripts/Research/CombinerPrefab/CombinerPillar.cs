using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombinerPillar : MonoBehaviour
{
    public SampleBehaviour curSample;
    public Vector3 sampleScale;

    public void DestroySample()
    {
        if (curSample != null)
        {
            curSample.GetComponent<SceneObject>().DestroySceneObj();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //TO DO
        // add a check so a second collision doesn't overwrite/wipe
        SampleBehaviour sample = other.GetComponent<SampleBehaviour>();
        if (sample != null)
        {
            curSample = sample;
            sampleScale = curSample.transform.localScale;
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        curSample = null;
        sampleScale = Vector3.zero;
    }
}
