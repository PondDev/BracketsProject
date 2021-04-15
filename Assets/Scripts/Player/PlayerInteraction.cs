using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction
{
    public static float interactRange = 2f;

    private PlayerMaster master;
    private InputController input;
    private PlayerCamera camera;
    private Inventory inv;

    
    private IInteractable hitLastFrame;

    public Ray interactRay;

    public bool carrying
    {get; set;}

    public PlayerInteraction(PlayerMaster _master, InputController _input, PlayerCamera _camera, Inventory _inv)
    {
        master = _master;

        input = _input;
        camera = _camera;
        inv = _inv;

        interactRay = new Ray(camera.camInstance.transform.position, camera.camInstance.transform.forward);
    }
    // Update is called once per frame
    public void Tick(float delta)
    {
        //Input lock
        if (input.inputOn)
        {
            //Interaction Cast
            RaycastHit hitt;
            interactRay = new Ray(camera.camInstance.transform.position, camera.camInstance.transform.forward);

            if (Physics.Raycast(interactRay, out hitt, interactRange, LayerMask.GetMask("Interactable")))
            {
                IInteractable obj = hitt.collider.GetComponent<IInteractable>();
                if (obj != null)
                {
                    //Hovering on interactables
                    if (obj == hitLastFrame)
                    {
                        obj.OnHover();
                    }
                    else
                    {
                        obj.OnHoverEnter();
                        if (hitLastFrame != null)
                        {
                            hitLastFrame.OnHoverExit();
                            hitLastFrame = null;
                        }
                    }

                    //Interaction on interactables
                    if (Input.GetKeyDown(input.Interact.key))
                    {
                        obj.Interact(master);
                    }
                    else if (carrying)
                    {
                        if (input.Pocket.Down)
                        {
                            SampleBehaviour sampleIn = hitt.collider.GetComponent<SampleBehaviour>();
                            if (sampleIn != null)
                            {
                                //Add sample to inv if possible then destroy
                                if (inv.AddSample(sampleIn.sample))
                                {
                                    sampleIn.GetComponent<SceneObject>().DestroySceneObj();

                                    //Object.Destroy(hitt.collider.gameObject);
                                    carrying = false;
                                }
                            }
                        }
                    }
                    hitLastFrame = obj;
                }
                else if (hitLastFrame != null)
                {
                    Cleanup();
                }
            }
            else if (hitLastFrame != null)
            {
                Cleanup();
            }
        }
    }

    public void Cleanup()
    {
        hitLastFrame.OnHoverExit();
        hitLastFrame = null;
    }
}
