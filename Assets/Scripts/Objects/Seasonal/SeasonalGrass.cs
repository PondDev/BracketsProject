using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonalGrass : MonoBehaviour
{
    public GameObject grassToChange;
    public GameObject leaves;
    public float changeDuration = 10f;
    public Color[] colors = new Color[8];

    float stepDuration;
    float curTime = 0;

    Color curColorTop;
    Color curColorBottom;

    int stage = 0;
    [System.NonSerialized]
    public bool running = false;

    Material sharedMaterial;
    Material sharedLeavesMaterial;

    void Start()
    {
        Collider coll = GetComponentInChildren<Collider>();

        stepDuration  = changeDuration /4;

        curColorTop = colors[0];
        curColorBottom = colors[1];

        sharedMaterial = grassToChange.GetComponentInChildren<MeshRenderer>().sharedMaterial;
        sharedLeavesMaterial = leaves.GetComponentInChildren<MeshRenderer>().sharedMaterial;

        sharedMaterial.SetColor("Color_6C220024", curColorTop);
        sharedMaterial.SetColor("Color_32C59315", curColorBottom);
        sharedLeavesMaterial.SetColor("Color_E08900E6", curColorTop);
        //sharedLeavesMaterial.SetColor("Color_70BF2FCC", curColorBottom);
    }

    void Update()
    {
        if (running)
        {
            int colorIndex = stage * 2;
            if (stage == 3)
            {
                curColorTop = Color.Lerp(colors[colorIndex], colors[0], curTime/stepDuration);
                curColorBottom = Color.Lerp(colors[colorIndex+1], colors[1], curTime/stepDuration);
                
            }
            else
            {
                curColorTop = Color.Lerp(colors[colorIndex], colors[colorIndex+2], curTime/stepDuration);
                curColorBottom = Color.Lerp(colors[colorIndex+1], colors[colorIndex+3], curTime/stepDuration);
            }

            sharedMaterial.SetColor("Color_6C220024", curColorTop);
            sharedMaterial.SetColor("Color_32C59315", curColorBottom);

            sharedLeavesMaterial.SetColor("Color_E08900E6", curColorTop);
            //sharedLeavesMaterial.SetColor("Color_70BF2FCC", curColorBottom);

            curTime += Time.deltaTime;
            if (curTime >= stepDuration)
            {
                stage +=1;
                curTime = 0;
                if (stage >= 4)
                {
                    stage = 0;
                }
            }
        }
    }

    void OnTriggerEnter()
    {
        running = true;
    }

    void OnTriggerExit()
    {
        running = false;
    }
}
