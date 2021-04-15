using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WateringSoil : MonoBehaviour
{
    public static float growTime = 10f;
    bool growing = false;
    public Transform plantParent;

    float t = 0;
    Vector3 plantParentScale;
    bool flip = false;

    public void Start()
    {
        plantParentScale = plantParent.localScale;
        plantParent.localScale = Vector3.zero;
    }

    public void Grow()
    {
        if (!flip)
        {
            flip = true;
            growing = true;
        }
    }

    void Update()
    {
        if (growing)
        {
            t += Time.deltaTime;

            float fraction = t/growTime;
            if (fraction >= 1f)
            {
                fraction = 1f;
                growing = false;
                t = 0;
            }

            plantParent.localScale = Vector3.Lerp(Vector3.zero, plantParentScale, fraction);
        }
    }
}
