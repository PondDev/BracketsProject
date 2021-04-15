using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanel
{
    #region Singleton
    private static readonly InventoryPanel instance = new InventoryPanel();

    static InventoryPanel() {}
    
    public static InventoryPanel Instance
    {
        get {return instance;}
    }
    #endregion

    //Prefabs
    public string slotPath = "Prefabs/UI/Slot";
    public string uiItemPath = "Prefabs/UI/UISample";
    public string panelInstanceName = "TempUI";

    public Canvas UiCanvas;

    GameObject slot;
    GameObject uiSample;

    GameObject panelInstance;
    GameObject slotPanelInstance;

    //Slots container
    private Dictionary<int, GameObject> slots = new Dictionary<int, GameObject>();
    private Dictionary<int, SampleUI> uiSamples = new Dictionary<int, SampleUI>();

    private Inventory _inventory;
    public Inventory inventory
    {
        get {return _inventory;}
        set {_inventory = value;}
    }

    private InventoryPanel()
    {
        UiCanvas = GameObject.Find("InWorldCanvas").GetComponent<Canvas>();
        //Load
        slot = Resources.Load<GameObject>(slotPath);
        uiSample = Resources.Load<GameObject>(uiItemPath);

        //Instances panel object
        panelInstance = GameObject.Find(panelInstanceName).gameObject;
        slotPanelInstance = panelInstance.transform.Find("SlotPanel").gameObject;

        //Create slots
        for (int i = 0; i < Inventory.numSlots; i++)
        {
            GameObject newSlot = Object.Instantiate(slot, slotPanelInstance.transform);
            newSlot.GetComponent<SlotData>().SlotIndex = i;
            slots.Add(i, newSlot);
        }
    }

    public void RegisterInventory(Inventory inv)
    {
        inventory = inv;
    }

    //TODO:
    //Potential expansion on this would to have Inventory record a list of modified slots
    //then UpdateInvUI would only have to manage the listed slots
    public void UpdateInventoryUI()
    {
        foreach (KeyValuePair<int, GameObject> entry in slots)
        {
            //Check inv for slot
            Sample sampleAttempt = inventory.GetSampleAt(entry.Key);

            //if slot is empty in inventory
            if (sampleAttempt == null)
            {
                //remove from ui
                if (uiSamples.ContainsKey(entry.Key))
                {
                    Object.Destroy(uiSamples[entry.Key].gameObject);
                    uiSamples.Remove(entry.Key);
                }
            }

            //if slot is full in inventory
            //if slot is full in ui
            else if (uiSamples.ContainsKey(entry.Key))
            {
                //set item
                //if structure here avoids slight overhead on setting the icon etc.
                if (uiSamples[entry.Key].sample.id == sampleAttempt.id)
                {
                    continue;
                }
                else
                {
                    uiSamples[entry.Key].sample = sampleAttempt;
                }
            }
            //if slot is empty in ui
            else
            {
                //new ui element
                CreateItemContainer(entry.Key, entry.Value, sampleAttempt);
            }
        }
    }

    void CreateItemContainer(int key, GameObject slot, Sample sample)
    {
        //create item container in slot
        GameObject uiSampleObj = Object.Instantiate(uiSample, slot.transform);
        //uiItemObj.name = item.title;

        //set item container values
        SampleUI sampleUIData = uiSampleObj.GetComponent<SampleUI>();
        uiSamples.Add(key, sampleUIData);
        sampleUIData.sample = sample;
    }
}