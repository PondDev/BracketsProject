using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class UnlockMechanicObject : MonoBehaviour, IInteractable
{
    public int mechanicId = 0;

    void Start()
    {
        this.gameObject.layer = 8;
    }

    public void Interact(PlayerMaster player)
    {
        player.GetPlayerMechanics().AddMechanic(mechanicId);
        GetComponent<SceneObject>().DestroySceneObj();
    }

    public void OnHoverEnter() {}

    public void OnHover(){}

    public void OnHoverExit() {}
}
