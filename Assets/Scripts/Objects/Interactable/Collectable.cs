using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//temp
using UnityEngine.UI;


/*
 * This will sort of not exist once sample system is better
 */

[RequireComponent(typeof(Collider))]
public class Collectable : MonoBehaviour, IInteractable
{
    
    public int sampleId = 0;
    Sample sample;

    public Color lineOnHover = Color.red;
    public float lineThickOnHover = 2f;

    private Material myMat;

    private Color initalCol;
    private float initalFloat;

    void Start()
    {
        sample = SampleDatabase.Instance.GetSampleByID(sampleId);

        this.gameObject.layer = 8;
        //myMat = GetComponent<MeshRenderer>().material;
        //initalCol = myMat.GetColor("Color_70BF2FCC");
        //initalFloat = myMat.GetFloat("Vector1_F5D76E9B");
    }

    public void Interact(PlayerMaster player)
    {
        player.GetPlayerInventory().AddSample(sample);
    }

    public void OnHoverEnter()
    {
        //myMat.SetColor("Color_70BF2FCC", lineOnHover);
        //myMat.SetFloat("Vector1_F5D76E9B", lineThickOnHover);
    }

    public void OnHover(){}

    public void OnHoverExit()
    {
        //myMat.SetColor("Color_70BF2FCC", initalCol);
        //myMat.SetFloat("Vector1_F5D76E9B", initalFloat);
    }
}
