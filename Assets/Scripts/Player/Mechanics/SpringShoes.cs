using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
generally a a bit jank
*/
public class SpringShoes : UnlockableMechanic
{
    float jumpValue = 500f;
    bool shoesOn = false;

    //player controller
    PlayerController controller;

    public SpringShoes()
    {
        mechanicObject = Resources.Load<GameObject>("Prefabs/Samples/SpringShoes");
        
    }

    public override void Activate(PlayerMaster master)
    {
        controller = master.GetPlayerController();
    }

    public override void Down(Ray interactRay)
    {
        jumpValue = controller.ReplaceJump(jumpValue);

        //Toggle Shoes
        //Maybe toggle the icon too?
        if (shoesOn)
        {
            //Turn off
            
        }
        else
        {
            //Turn on
            
        }

        shoesOn = !shoesOn;
    }

    public override void Hold(Ray interactRay) {}

    public override void Up(Ray interactRay) {}

    public override void HoverEnter(){}
    public override void HoverExit(){}
}
