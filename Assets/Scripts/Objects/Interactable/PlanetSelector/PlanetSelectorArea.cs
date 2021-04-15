using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Planet selector interaction by range.
/// <summary/>
[RequireComponent(typeof(Collider))]
public class PlanetSelectorArea : MonoBehaviour
{
    public PlanetSelectorPlanet[] planets;

    void Start()
    {
        planets = transform.parent.GetComponentsInChildren<PlanetSelectorPlanet>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (PlanetSelectorPlanet planet in planets)
            {
                planet.PlanetsOn();        // This should be some kind of SetOn()
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (PlanetSelectorPlanet planet in planets)
            {
                planet.PlanetsOff();        // This should be some kind of SetOff()
            }
        }
    }
}
