using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oven : MonoBehaviour
{
    public Transform outZone;
    public float castRadius = 1f;
    
    Tester tester = new Tester(Test.Cooker);

    List<SampleBehaviour> inSamples;

    bool ticking = false;
    public float ovenTime = 2f;
    float t = 0;

    public ParticleSystem particles;

    bool doorState = false;
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (ticking)
        {
            t += Time.deltaTime;
            if (t >= ovenTime)
            {
                ticking = false;
                particles.Stop();
                FulfilTest();
            }
        }
    }

    void FulfilTest()
    {
        //can use layer mask here (probably should)
        Collider[] hits = Physics.OverlapSphere(outZone.position, castRadius);

        foreach(Collider hit in hits)
        {
            SampleBehaviour sample = hit.GetComponent<SampleBehaviour>();
            if (sample!= null)
            {
                Sample hold = tester.TestSample(sample.sample);
                if (hold != null)
                {
                    sample.GetComponent<SceneObject>().DestroySceneObj();
                    SampleBehaviour.SpawnInWorld(hold, outZone.position);
                }
            }
        }   
    }

    public void OvenOn()
    {
        Collider[] hits = Physics.OverlapSphere(outZone.position, castRadius);
        Debug.Log(hits.Length);
        foreach(Collider hit in hits)
        {
            Debug.Log(hit.name);
        }

        if (hits.Length > 0)
        {
            if (doorState)
            {
                CloseDoor();
                doorState = false;
            }

            ticking = true;
            particles.Play();
        }
    }

    public void ToggleDoor()
    {
        //Open and close door
        if (doorState)
        {
            CloseDoor();
            doorState = false;
        }
        else
        {
            OpenDoor();
            doorState = true;
        }
    }

    void OpenDoor()
    {
        animator.Play("OvenDoorOpen");
    }
    void CloseDoor()
    {
        animator.Play("OvenDoorClose");
    }
}
