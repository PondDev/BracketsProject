/*
ui display of an item, also handles dragging and dropping items around inv
TODO:
Cleanup SlotData - the sampleUI could probably just track it's current slot somehow?
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SampleUI : MonoBehaviour,
IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private Sample _sample;

    public Sample sample
    {
        get {return _sample; }
        set 
        {
            _sample = value;

            //Set sprite / visual of item. Potentially to change from sprites
            sampleImg.overrideSprite = _sample.icon;
        }
    }
    private Image sampleImg;

    private Transform curSlotTransform;

    void Awake()
    {
        sampleImg = GetComponent<Image>();

        curSlotTransform = transform.parent;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (sample != null)
        {
            this.transform.SetParent(InventoryPanel.Instance.UiCanvas.transform);

            Canvas canvas = InventoryPanel.Instance.UiCanvas;
            
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, canvas.worldCamera, out pos);
            this.transform.position = canvas.transform.TransformPoint(pos);

            sampleImg.raycastTarget = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (sample != null)
        {
            Canvas canvas = InventoryPanel.Instance.UiCanvas;
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, canvas.worldCamera, out pos);
            this.transform.position = canvas.transform.TransformPoint(pos);
        }
    }

    public void OnPointerUp(PointerEventData eventData) 
    {
        sampleImg.raycastTarget = true;
        this.transform.SetParent(curSlotTransform);
        this.transform.position = curSlotTransform.position;

        //cast for ui element
        GameObject castHit = eventData.pointerCurrentRaycast.gameObject;
        if (castHit)
        {
            GameObject destinationSlot = null;
            //if we find an Item
            if (castHit.GetComponent<SampleUI>())
            {
                //Get the slot and carry on
                destinationSlot = castHit.transform.parent.gameObject;
            }
            else if (castHit.GetComponent<SlotData>())
            {
                destinationSlot = castHit;
            }

            if (destinationSlot != null)
            {
                int origin = curSlotTransform.GetComponent<SlotData>().SlotIndex;
                int destination = destinationSlot.GetComponent<SlotData>().SlotIndex;

                InventoryPanel.Instance.inventory.SwapByIndex(origin, destination);
            }
            else
            {
                SpawnInWorld();
            }
        }
        else
        {
            SpawnInWorld();
        }
    }

    public void SpawnInWorld()
    {
        //remove from inv
        //this will need to change
        InventoryPanel.Instance.inventory.RemoveSampleAt(curSlotTransform.GetComponent<SlotData>().SlotIndex);

        //finding component for player isn't great
        PlayerMaster player = gameObject.GetComponentInParent<PlayerMaster>();
        SampleBehaviour.SpawnInWorld(sample, player.transform.position + player.transform.forward);
    }
}