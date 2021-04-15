﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSelectorButton : MonoBehaviour, IInteractable
{
    public PlanetSelectorPlanet[] planets;

    public Color lineOnHover = Color.red;
    public float lineThickOnHover = 2f;

    private Material myMat;

    private Color initalCol;
    private float initalFloat;
    void Start()
    {
        //backup layer set
        this.gameObject.layer = 8;
        myMat = GetComponent<MeshRenderer>().material;
        initalCol = myMat.GetColor("Color_70BF2FCC");
        initalFloat = myMat.GetFloat("Vector1_F5D76E9B");

        planets = transform.parent.GetComponentsInChildren<PlanetSelectorPlanet>();
    }

    public void Interact(PlayerMaster player)
    {
        foreach (PlanetSelectorPlanet planet in planets)
        {
            planet.Toggle();
        }
    }

    public void OnHoverEnter()
    {
        myMat.SetColor("Color_70BF2FCC", lineOnHover);
        myMat.SetFloat("Vector1_F5D76E9B", lineThickOnHover);
    }

    public void OnHover(){}

    public void OnHoverExit()
    {
        myMat.SetColor("Color_70BF2FCC", initalCol);
        myMat.SetFloat("Vector1_F5D76E9B", initalFloat);
    }
}
