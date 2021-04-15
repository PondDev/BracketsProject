using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunAwaySpider : MonoBehaviour
{
    public float dist = 1f;
    public float speed = 3f;

    float timer = 100f;
    bool off = false;

    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetFloat("Speed", 0f);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, 5f))
        {
            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            transform.position = hit.point;
        }
    }

    void Update()
    {
        if (timer < dist)
        {
            transform.position = transform.position + transform.forward * speed * Time.deltaTime;
            timer += Time.deltaTime;
        }
        else
        {
            timer = dist;
            anim.SetFloat("Speed", 0f);
        }
        //snap to normal
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, 5f))
        {
            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            transform.position = hit.point;
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (!off)
        {
            if (other.CompareTag("Player"))
            {
                timer = 0;
                anim.SetFloat("Speed", speed);
                off = true;
            }
        }
    }
}
