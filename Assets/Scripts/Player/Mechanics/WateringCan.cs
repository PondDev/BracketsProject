using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WateringCan : UnlockableMechanic
{
    Vector3 offset = new Vector3(0.13f, 0, 0.3f);
    GameObject  particlesPrefab;
    GameObject particlesInstance;
    ParticleSystem system;

    bool held = false;

    public WateringCan()
    {
        mechanicObject = Resources.Load<GameObject>("Prefabs/Samples/WateringCan");
        particlesPrefab = Resources.Load<GameObject>("Prefabs/WateringCanParticles");
    }

    public override void Activate(PlayerMaster master)
    {
        //needs the camera transform
        particlesInstance = Object.Instantiate(particlesPrefab, Camera.main.gameObject.transform);
        particlesInstance.transform.localPosition = offset;
        system = particlesInstance.GetComponent<ParticleSystem>();
        system.Stop();
    }

    public override void Down(Ray interactRay)
    {
        system.Play();
    }

    public override void Hold(Ray interactRay)
    {
        RaycastHit hit;
        if (Physics.Raycast(interactRay, out hit, PlayerInteraction.interactRange))
        {
            if (hit.transform.CompareTag("Waterable"))
            {
                WateringSoil soil = hit.transform.GetComponent<WateringSoil>();
                soil.Grow();

                //this could also be need to water it for a certain amount of time
                
            }
        }
    }

    public override void Up(Ray interactRay)
    {
        system.Stop();
    }

    public override void HoverEnter(){}
    public override void HoverExit(){}
}
