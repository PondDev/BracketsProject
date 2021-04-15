using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChilliPlant : MonoBehaviour
{
    Animator anim;

    public void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            anim.SetBool("IsOpen", true);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            anim.SetBool("IsOpen", false);
        }
    }
}
