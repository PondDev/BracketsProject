using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harvester : UnlockableMechanic
{
    Transform playerTransform;

    public Harvester(){}

    public override void Activate(PlayerMaster master)
    {
        playerTransform = master.gameObject.transform;
    }

    public override void Down(Ray interactRay)
    {
        RaycastHit hitt;

        if (Physics.Raycast(interactRay, out hitt, PlayerInteraction.interactRange, LayerMask.GetMask("Harvestable")))
        {
            Harvestable obj = hitt.collider.GetComponent<Harvestable>();
            if (obj != null)
            {
                //maybe 2 different types?
                //  one that pops off
                //  one that you can just grab
                Sample newSample = SampleDatabase.Instance.GetSampleByID(obj.sampleId);
                if (obj.destroy)
                {
                    Object.Destroy(obj.gameObject);
                    SampleBehaviour.SpawnInWorld(newSample, hitt.point);
                    //GameObject newObj = Object.Instantiate(newSample.worldPrefab, obj.transform.position, Quaternion.identity);
                    //newObj.transform.localScale = obj.transform.localScale;
                }
                else
                {
                    SampleBehaviour.SpawnInWorld(newSample, hitt.point);
                }
            }
        }
    }
    public override void Hold(Ray interactRay) {}
    public override void Up(Ray interactRay) {}

    public override void HoverEnter(){}
    public override void HoverExit(){}
}
