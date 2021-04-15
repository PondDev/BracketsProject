using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSpiderMove : MonoBehaviour
{
    public float speed = 1f;
    public float flipTime = 2f;
    public float switchTime = 1f;

    private float curSpeed;
    private float timer = 0;
    private float switchTimer = 0;

    private bool forward = true;

    private Rigidbody rb;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        curSpeed = speed;
        anim.SetFloat("Speed", curSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(curSpeed);
        if (timer >= flipTime)
        {
            switchTimer += Time.deltaTime;
            //lerp the speed in the other direction
            if (forward)
            {
                curSpeed = Mathf.Lerp(speed, -speed, switchTimer/switchTime);
            }
            else
            {
                curSpeed = Mathf.Lerp(-speed, speed, switchTimer/switchTime);
            }
            anim.SetFloat("Speed", curSpeed);
            if (switchTimer/switchTime > 1)
            {
                timer = 0;
                switchTimer = 0;
                forward = !forward;
            }
        }
        else
        {
            timer += Time.deltaTime;
        }

        rb.MovePosition(rb.position + transform.forward * curSpeed * Time.deltaTime);
    }
}
