using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonalTrigger : MonoBehaviour
{
    private SeasonalGrass seasonalGrass;
    void Start()
    {
        seasonalGrass = transform.parent.gameObject.GetComponent<SeasonalGrass>();
    }

    void OnTriggerEnter()
    {
        seasonalGrass.running = true;
    }

    void OnTriggerExit()
    {
        seasonalGrass.running = false;
    }
}
