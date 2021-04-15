/*
Interface for interactable objects in world
*/
using UnityEngine;

public interface IInteractable
{
    void Interact(PlayerMaster player);
    void OnHover();
    void OnHoverExit();
    void OnHoverEnter();
}
