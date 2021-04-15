using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMechanics
{
    private PlayerMaster master;
    InputController input;
    PlayerInteraction interaction;
    Toolbar toolbar;

    //this can probably be static tbh
    SortedList<int, UnlockableMechanic> mechanics = new SortedList<int, UnlockableMechanic>()
    {
        {0, new Harvester()},
        {1, new Flashlight()},
        {2, new HeatRay()},
        {3, new WateringCan()},
        {4, new SpringShoes()}
    };

    //this tracks what current mechanics are unlocked
    List<int> unlockedMechanics = new List<int>();

    int curToolbarIndex = 0;

    public PlayerMechanics(PlayerMaster _master, InputController _input,  PlayerInteraction _interaction, Toolbar _toolbar)
    {
        master = _master;
        input = _input;
        toolbar = _toolbar;
        interaction = _interaction;
    }

    //for execution order
    public void Start()
    {
        AddMechanic(0);

        SetCurrentMechanic(curToolbarIndex);
    }

    public void Tick(float delta)
    {
        if (input.Numbers.recentNum != curToolbarIndex + 1)
        {
            if (unlockedMechanics.Contains(curToolbarIndex))
            {
                mechanics[curToolbarIndex].Up(interaction.interactRay);
            }
            SetCurrentMechanic(input.Numbers.recentNum - 1);
        }

        if (input.inputOn)
        {
            bool mechanicUnlocked = unlockedMechanics.Contains(curToolbarIndex);

            if (mechanicUnlocked)
            {
                if (Input.GetKeyDown(input.Mechanic.key))
                {
                    mechanics[curToolbarIndex].Down(interaction.interactRay);
                }
                if (input.Mechanic.Held)
                {
                    mechanics[curToolbarIndex].Hold(interaction.interactRay);
                }
                if (input.Mechanic.Up)
                {
                    mechanics[curToolbarIndex].Up(interaction.interactRay);
                }
            }
        }
    }

    public void AddMechanic(int index)
    {
        mechanics[index].Activate(master);
        unlockedMechanics.Add(index);
        toolbar.RevealMechanic(index);
    }

    /*
    public void AddMechanic(int index, UnlockableMechanic mechanic)
    {
        mechanics.Add(index, mechanic);
        mechanic.Activate();

        toolbar.RevealMechanic(index);
    }
    */

    void SetCurrentMechanic(int index)
    {
        curToolbarIndex = index;
        
        toolbar.UpdateToolbar(index);
    }
}
