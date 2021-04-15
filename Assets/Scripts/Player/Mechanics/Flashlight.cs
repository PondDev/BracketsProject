using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : UnlockableMechanic
{
    public Light flashlight;
    

    public Flashlight()
    {
        //TEMP
        mechanicObject = Resources.Load<GameObject>("Prefabs/Samples/Flashlight");
        flashlight = GameObject.Find("NewPlayer(Clone)/PlayerCamera(Clone)/Spot Light").GetComponent<Light>();
        flashlight.enabled = false;
    }

    public override void Activate(PlayerMaster master){}

    public override void Down(Ray interactRay)
    {
        flashlight.enabled = !flashlight.enabled;
    }
    public override void Hold(Ray interactRay) {}
    public override void Up(Ray interactRay) {}

    public override void HoverEnter(){}
    public override void HoverExit(){}
}
