using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory
{
    InputController input;
    PlayerCamera cam;

    //InvArm should probably be moved into this class
    InvArm inventoryUI;

    private StateMachine fsm;
    
    public PlayerInventory(InputController _input, PlayerCamera _cam, InvArm _inventoryUI)
    {
        input = _input;
        cam = _cam;
        inventoryUI = _inventoryUI;

        inventoryUI.cam = _cam;
    }

    public void Tick(float delta)
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventoryUI.Toggle())
            {
                if (input.inputOn)
                {
                    input.UnlockCursor();
                }
                else
                {
                    input.LockCursor();
                }
                input.SetInputState(!input.inputOn);
            }
        }
    }
}
