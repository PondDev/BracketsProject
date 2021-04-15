using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
generally a a bit jank
*/
public class HeatRay : UnlockableMechanic
{
    Vector3 offset = new Vector3(0.13f, 0, 0.3f);
    GameObject  particlesPrefab;
    GameObject particlesInstance;
    ParticleSystem system;

    bool held = false;

    public HeatRay()
    {
        mechanicObject = Resources.Load<GameObject>("Prefabs/Samples/HeatRay");
        particlesPrefab = Resources.Load<GameObject>("Prefabs/HeatRayParticles");
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
            if (hit.transform.CompareTag("Meltable"))
            {
                //this needs to be better I think
                hit.transform.localScale = hit.transform.localScale - new Vector3(0.001f, 0.001f, 0.001f);
                hit.transform.GetComponent<SceneObject>().SetTransform();

                if (hit.transform.localScale.x <= 0.01f)
                {
                    hit.transform.GetComponent<SceneObject>().DestroySceneObj();
                    //Object.Destroy(hit.transform.gameObject);
                }
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
