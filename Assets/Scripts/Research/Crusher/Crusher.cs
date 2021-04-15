using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crusher : MonoBehaviour
{
    public Transform outZone;
    Animator animator;
    public Transform particleParent;
    List<ParticleSystem> particles = new List<ParticleSystem>();
    Tester tester = new Tester(Test.Crusher);

    public Conveyor conveyor;

    SampleBehaviour inSample;
    Sample hold;

    void Start()
    {
        animator = GetComponent<Animator>();
        foreach(Transform trans in particleParent)
        {
            particles.Add(trans.GetComponent<ParticleSystem>());
        }
    }

    public void CrusherOn()
    {
        //play animation
        //start conveyor belt
        conveyor.on = true;
        animator.Play("CrushDoorOpen");
    }

    public void CrusherOff()
    {
        conveyor.on = false;
        animator.Play("CrushDoorClose");
    }

    private void OnTriggerEnter(Collider other)
    {
        animator.Play("Crush");
        SampleBehaviour sample = other.GetComponent<SampleBehaviour>();
        if (sample != null)
        {
            if (!sample.fresh)
            {
                inSample = sample;
            }
        }
    }

    public void DestroySample()
    {
        if (inSample != null)
        {
            hold = tester.TestSample(inSample.sample);

            //TEMP
            if (inSample.id == 3)
            {
                TempTasksManager.instance.UpdateCurrentTask(3);
            }
            //

            inSample.GetComponent<SceneObject>().DestroySceneObj();
            foreach(ParticleSystem system in particles)
            {
                system.Play();
            }
        }
    }

    public void NewSample()
    {
        if (hold != null)
        {
            SampleBehaviour.SpawnInWorld(hold, outZone.position);
            hold = null;
            inSample = null;
        }
    }
}
