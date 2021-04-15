using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnlockableMechanic
{
    protected List<int> recipe;
    public GameObject mechanicObject;

    public UnlockableMechanic()
    {
        //base constructor
    }

    public abstract void Activate(PlayerMaster master);

    public abstract void Down(Ray ray);
    public abstract void Hold(Ray ray);
    public abstract void Up(Ray ray);

    public abstract void HoverEnter();
    public abstract void HoverExit();

    //Other stuff
}
