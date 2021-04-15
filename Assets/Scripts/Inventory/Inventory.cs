/*
Generic inventory behaviour, applied to any obj to give that obj an inv.
All invs give info to InventoryUI to be displayed.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{   
    public static int numSlots = 4;

    private Sample[] inv = new Sample[numSlots];
    //private Dictionary<int, int> itemAmounts = new Dictionary<int, int>();

    private bool isDirty;

    void Start()
    {
        InventoryPanel.Instance.RegisterInventory(this);
        //Item Load From File
    }

    void Update()
    {
        if (isDirty)
        {
            InventoryPanel.Instance.UpdateInventoryUI();   
        }
    }

    public bool CheckWin()
    {
        int counter = 0;
        for (int i = 0; i < inv.Length; i ++)
        {
            if (inv[i] != null)
            {
                if(inv[i].id == 4)
                {
                    counter++;
                }
            }
        }

        if(counter == numSlots)
        {
            return true;
        }
        return false;
    }

    //Adds Item to first available slot
    public bool AddSample(Sample sample)
    {   
        for (int i = 0; i < inv.Length; i ++)
        {
            if (inv[i] == null)
            {
                inv[i] = sample;
                isDirty = true;
                return true;
            }
        }
        return false;
    }

  
    //Adds Item to specific slot
    public bool AddSampleAt(Sample item, int slot)
    {
        if (inv[slot] == null)
        {
            inv[slot] = item;
            isDirty = true;
            return true;
        }
        return false;
    }
    

    public void RemoveSampleAt(int key)
    {
        inv[key] = null;
        isDirty = true;
    }

    public Sample GetSampleAt(int key)
    {
        return inv[key];
    }

    public void SwapByIndex(int originId, int destinationId)
    {
        if (inv[destinationId] != null)
        {
            Sample hold = inv[destinationId];
            inv[destinationId] = inv[originId];
            inv[originId] = hold;
        }
        else
        {
            inv[destinationId] = inv[originId];
            inv[originId] = null;
        }
        isDirty = true;
    }

    public int GetSampleIdAt (int slotNum) {
        return inv[slotNum].id;
    }

    public void setInventoryFromSave(int slotNumber, int itemNumber) {
        inv[slotNumber].id = itemNumber;
    }
}
