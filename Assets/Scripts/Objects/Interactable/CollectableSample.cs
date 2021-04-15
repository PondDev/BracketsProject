using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
WIP
*/
public class CollectableSample : MonoBehaviour
{
    public int sampleId;
    public GameObject obj;
    public bool spawnObject = true;

    Sample sample;

    //this can be hidden if 
    //spawnObject = false
    GameObject activeObject;

    Transform playerTransform;
    Vector3 origin;
    Vector3 originScale;

    bool ticking = false;
    public float floatTime = 2f;
    float timer = 0;

    // Likely we have two types:
    //  1. The object already exists in the world (chilli)
    //      These will use this script

    //  2. The sample is extracted (rocks, vine, grass)
    //      These will


    // Start is called before the first frame update
    void Start()
    {
        sample = SampleDatabase.Instance.GetSampleByID(sampleId);
    }

    // Update is called once per frame
    void Update()
    {
        if (ticking)
        {
            timer += Time.deltaTime;

            float fraction = floatTime/timer;
            if (fraction >= 1)
            {
                ticking = false;
                timer = 0;
                fraction = 1;
            }

            activeObject.transform.position = Vector3.Lerp(origin, playerTransform.position, fraction);
            activeObject.transform.localScale = Vector3.Lerp(Vector3.zero, originScale, fraction);
        }
    }

    public void OnHarvest(Vector3 position, Transform trans)
    {
        if (spawnObject)
        {
            // spawn the sample
            activeObject = Instantiate(obj, position, Quaternion.identity);

            //activeObject.gameObject.GetComponent<>().enabled = false;
        }
        else
        {
            // the sample already exists
        }

        playerTransform = trans;
        origin = position;
        originScale = activeObject.transform.localScale;
        activeObject.transform.localScale = Vector3.zero;
        ticking = true;
    }
}
