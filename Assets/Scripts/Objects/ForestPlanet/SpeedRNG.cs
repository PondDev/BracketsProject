using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedRNG : MonoBehaviour
{
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetFloat("Speed", Random.Range(1f,1.4f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
