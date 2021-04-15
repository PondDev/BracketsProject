using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FogVolumes : MonoBehaviour
{
    public Color color;
    public float maxDensity;
    public float switchTime;

    public SkyboxPlayer skybox;

    bool switching = false;
    float timer;

    //true for in false for out
    bool InOrOut;

    // Update is called once per frame
    void Update()
    {
        if (switching)
        {
            if (timer <= 0)
            {
                timer = 0;
                switching = false;
                if (!InOrOut)
                {
                    RenderSettings.fog = false;
                    RenderSettings.fogDensity = 0f;

                    skybox.setColors = true;
                }
            }

            float fraction = timer / switchTime;
            if (InOrOut)
            {
                //switch in
                fraction = 1 - fraction;
            }
            RenderSettings.fogDensity = maxDensity * fraction;

            skybox.skyGradient.Evaluate(skybox.MagnitudeCalc());

            float magnitude = skybox.MagnitudeCalc();
            Color curSkyCol = skybox.skyGradient.Evaluate(magnitude);
            skybox.SetSkyboxColors(skybox.horizonGradient.Evaluate(magnitude), new Color((color.r * fraction) + (curSkyCol.r * (1-fraction)), (color.g * fraction) + (curSkyCol.g * (1-fraction)),(color.b * fraction) + (curSkyCol.b * (1-fraction)), 1f));


            timer -= Time.deltaTime;    
        }
        else if (!skybox.setColors)
        {
            float magnitude = skybox.MagnitudeCalc();
            skybox.SetSkyboxColors(skybox.horizonGradient.Evaluate(magnitude), color);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RenderSettings.fog = true;
            RenderSettings.fogColor = color;
            RenderSettings.fogDensity = 0f;
            switching = true;
            InOrOut = true;
            timer = switchTime;

            skybox.setColors = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switching = true;
            InOrOut = false;
            timer = switchTime;
        }
    }
}
