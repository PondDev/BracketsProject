using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Toolbar : MonoBehaviour
{
    public Color highlighted;
    public Color nonHighlighted;

    List<Image> images = new List<Image>();
    List<Image> mechanicImgs = new List<Image>();

    void Awake()
    {
        foreach(Transform trans in transform)
        {
            images.Add(trans.GetComponent<Image>());
            foreach(Transform subtrans in trans)
            {
                mechanicImgs.Add(subtrans.GetComponent<Image>());
            }
        }
    }

    public void UpdateToolbar(int index)
    {
        foreach(Image image in images)
        {
            image.color = nonHighlighted;
        }
        images[index].color = highlighted;
    }

    public void RevealMechanic(int index)
    {
        Color alteredCol = mechanicImgs[index].color;
        alteredCol.a = 1f;
        mechanicImgs[index].color = alteredCol;
    }
}
